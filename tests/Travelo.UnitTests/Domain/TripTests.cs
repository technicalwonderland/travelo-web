using System;
using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using Travelo.Core.Domain;
using Travelo.Core.Domain.Providers;
using Travelo.UnitTests.Helpers;
using Xunit;

namespace Travelo.UnitTests.Domain
{
    public class TripTests
    {
        public static IDateTimeOffsetProvider DateTimeOffsetProvider =>
            new MockedDateOffsetProvider(DateTime.Parse("1970-01-01T00:00:00", CultureInfo.InvariantCulture));

        public static IEnumerable<object[]> ValidDateRanges => new List<object[]>
        {
            new object[] {DateTimeOffsetProvider.Now, DateTimeOffsetProvider.Now + Trip.TripMinimumTimeLimit},
            new object[] {DateTimeOffsetProvider.Now, DateTimeOffsetProvider.Now + Trip.TripMaximumTimeLimit},
            new object[] {DateTimeOffsetProvider.Now, DateTimeOffsetProvider.Now + TimeSpan.FromDays(1)}
        };

        public static IEnumerable<object[]> OverlappingTripDates => new List<object[]>
        {
            new object[]
            {
                DateTimeOffsetProvider.Now, DateTimeOffsetProvider.Now + Trip.TripMinimumTimeLimit,
                DateTimeOffsetProvider.Now, DateTimeOffsetProvider.Now + Trip.TripMaximumTimeLimit
            },
            new object[]
            {
                DateTimeOffsetProvider.Now + TimeSpan.FromDays(1), DateTimeOffsetProvider.Now + TimeSpan.FromDays(7),
                DateTimeOffsetProvider.Now, DateTimeOffsetProvider.Now + TimeSpan.FromDays(6)
            },
            new object[]
            {
                DateTimeOffsetProvider.Now, DateTimeOffsetProvider.Now + TimeSpan.FromDays(7),
                DateTimeOffsetProvider.Now + TimeSpan.FromDays(1), DateTimeOffsetProvider.Now + TimeSpan.FromDays(6)
            },
            new object[]
            {
                DateTimeOffsetProvider.Now, DateTimeOffsetProvider.Now + TimeSpan.FromDays(7),
                DateTimeOffsetProvider.Now + TimeSpan.FromDays(1), DateTimeOffsetProvider.Now + TimeSpan.FromDays(9)
            },
            new object[]
            {
                DateTimeOffsetProvider.Now + TimeSpan.FromDays(1), DateTimeOffsetProvider.Now + TimeSpan.FromDays(6),
                DateTimeOffsetProvider.Now, DateTimeOffsetProvider.Now + TimeSpan.FromDays(7)
            },
            new object[]
            {
                DateTimeOffsetProvider.Now + TimeSpan.FromDays(1), DateTimeOffsetProvider.Now + TimeSpan.FromDays(6),
                DateTimeOffsetProvider.Now, DateTimeOffsetProvider.Now + TimeSpan.FromDays(7)
            },
            new object[]
            {
                DateTimeOffsetProvider.Now + TimeSpan.FromDays(1), DateTimeOffsetProvider.Now + TimeSpan.FromDays(9),
                DateTimeOffsetProvider.Now, DateTimeOffsetProvider.Now + TimeSpan.FromDays(7)
            }
        };

        [Theory]
        [MemberData(nameof(ValidDateRanges))]
        public void create_trip_with_proper_dates_expect_success(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var trip = new Trip(Guid.NewGuid(), "TripName", "Destination", startDate, endDate,
                DateTimeOffsetProvider);
            trip.Should().NotBeNull();
        }

        [Theory]
        [MemberData(nameof(ValidDateRanges))]
        public void edit_trip_with_proper_dates_expect_success(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var trip = new Trip(Guid.NewGuid(), "TripName", "Destination", startDate, endDate,
                DateTimeOffsetProvider);
            trip.Edit("NewName", "Berlin", TripStatus.Active, startDate, endDate);
            trip.StartDateUTC.Should().Be(startDate.ToUniversalTime());
            trip.EndDateUTC.Should().Be(endDate.ToUniversalTime());
        }

        [Fact]
        public void create_trip_with_valid_data_expect_success()
        {
            var trip = DomainTestsHelper.ValidTrip;
            trip.Should().NotBeNull();
            trip.StartDateUTC.Should().Be(trip.StartDateUTC);
            trip.EndDateUTC.Should().Be(trip.EndDateUTC);
            trip.TripStatus.Should().Be(TripStatus.Active);
        }

        [Fact]
        public void cancel_trip()
        {
            var trip = DomainTestsHelper.ValidTrip;
            trip.Edit("EuroTrip", "Trip to berlin", TripStatus.Cancelled, trip.StartDateUTC, trip.EndDateUTC);
            trip.TripStatus.Should().Be(TripStatus.Cancelled);
        }

        [Fact]
        public void create_trip_with_invalid_username_expect_exception()
        {
            var now = DateTimeOffsetProvider.Now;
            Action act = () =>
                new Trip(Guid.NewGuid(), "TripName", "Destination", now, now + TimeSpan.FromDays(7),
                    DateTimeOffsetProvider);
        }

        [Fact]
        public void create_too_short_trip_expect_exception()
        {
            Action act = () => new Trip(Guid.NewGuid(), "TripName", "Destination", DateTimeOffsetProvider.Now,
                DateTimeOffsetProvider.Now + Trip.TripMinimumTimeLimit - TimeSpan.FromSeconds(1),
                DateTimeOffsetProvider);
            act.Should().Throw<DomainException>().Where(ex => ex.ErrorCode == DomainErrorCodes.TooShortTrip);
        }

        [Fact]
        public void create_too_long_trip_expect_exception()
        {
            Action act = () => new Trip(Guid.NewGuid(), "TripName", "Destination", DateTimeOffsetProvider.Now,
                DateTimeOffsetProvider.Now + Trip.TripMaximumTimeLimit + TimeSpan.FromSeconds(1),
                DateTimeOffsetProvider);
            act.Should().Throw<DomainException>().Where(ex => ex.ErrorCode == DomainErrorCodes.TooLongTrip);
        }

        [Fact]
        public void add_new_customer_to_trip_expect_success()
        {
            DomainTestsHelper.ValidTrip.AddCustomer(DomainTestsHelper.ValidCustomer);
        }

        [Fact]
        public void add_two_unique_customers_to_trip_expect_success()
        {
            var trip = DomainTestsHelper.ValidTrip;
            trip.AddCustomer(new Customer(Guid.NewGuid(), "adam", "badowski"));
            trip.AddCustomer(new Customer(Guid.NewGuid(), "omg", "zaduzo"));
        }

        [Theory]
        [MemberData(nameof(OverlappingTripDates))]
        public void add_customer_trip_with_overlapping_trips_expect_exception(DateTimeOffset firstStart,
            DateTimeOffset firstEnd, DateTimeOffset secondStart, DateTimeOffset secondEnd)
        {
            var trip = new Trip(Guid.NewGuid(), "TripName", "Destination", firstStart, firstEnd,
                DateTimeOffsetProvider);
            var customer = DomainTestsHelper.ValidCustomer;

            trip.AddCustomer(customer);
            var overlappingTrip = new Trip(Guid.NewGuid(), "TripName", "Destination", secondStart, secondEnd,
                DateTimeOffsetProvider);

            Action act = () =>
                overlappingTrip.AddCustomer(customer);
            act.Should().Throw<DomainException>()
                .Where(ex => ex.ErrorCode == DomainErrorCodes.OverlappingTripDates);
        }

        [Fact]
        public void add_same_customer_multiple_times_expect_exception()
        {
            var trip = DomainTestsHelper.ValidTrip;
            var customer = DomainTestsHelper.ValidCustomer;
            trip.AddCustomer(customer);
            Action act = () => trip.AddCustomer(customer);
            act.Should().Throw<DomainException>()
                .Where(ex => ex.ErrorCode == DomainErrorCodes.CustomerAlreadyAssignedToThisTrip);
        }

        [Fact]
        public void create_trip_with_default_datetime_expect_exception()
        {
            var date = new DateTimeOffset();
            var domainException =
                Assert.Throws<DomainException>(() =>
                    new Trip(Guid.NewGuid(), "TripName", "Destination", date, date, DateTimeOffsetProvider));
            domainException.ErrorCode.Should().Be(DomainErrorCodes.TooShortTrip);
        }

        [Fact]
        public void create_trip_with_end_date_before_start_date_expect_exception()
        {
            var now = DateTimeOffsetProvider.Now;
            var domainException =
                Assert.Throws<DomainException>(() =>
                    new Trip(Guid.NewGuid(), "TripName", "Destination", now - TimeSpan.FromDays(1), now,
                        DateTimeOffsetProvider));
            domainException.ErrorCode.Should().Be(DomainErrorCodes.TripStartDateIsInThePast);
        }
    }
}