using System;
using System.Collections.Generic;
using FluentAssertions;
using Travelo.Core.Domain;
using Travelo.UnitTests.Helpers;
using Xunit;

namespace Travelo.UnitTests.Domain
{
    public class CustomerTests
    {
        public static IEnumerable<object[]> InvalidNames => new List<object[]>
        {
            new object[] { "", "Adamski"},
            new object[] {"A", ""},
            new object[] { "", ""},
            new object[] {null, "A"},
            new object[] {null, null},
            new object[] {"", null},
            new object[] {null, ""},
            new object[] {null, "Adamski"},
            new object[] {"A", null}
        };

        [Fact]
        public void create_customer_with_first_and_last_name_expect_success()
        {
            var firstName = "Johny";
            var lastName = "Rambo";
            var customer = new Customer(Guid.NewGuid(), firstName, lastName);
            customer.Should().NotBeNull();
            customer.FirstName.Should().Be(firstName);
            customer.LastName.Should().Be(lastName);
        }

        [Fact]
        public void edit_customer_expect_success()
        {
            var customer = DomainTestsHelper.ValidCustomer;
            var firstName = "Catoholic";
            var lastName = "Pospolitus";
            customer.EditCustomer(firstName, lastName);
            customer.FirstName.Should().Be(firstName);
            customer.LastName.Should().Be(lastName);
        }

        [Theory]
        [MemberData(nameof(InvalidNames))]
        public void create_customer_with_incorrect_data_expect_fail(string firstName, string lastName)
        {
            Action act = () => new Customer(Guid.NewGuid(), firstName, lastName);
            act.Should().Throw<DomainException>().Where(x => x.ErrorCode == DomainErrorCodes.ArgumentNullOrEmpty);
        }

        [Fact]
        public void add_multiple_unique_customer_trips_expect_success()
        {
            var customer = new Customer(Guid.NewGuid(), "Grumpy", "Cat");
            var firstTrip = DomainTestsHelper.ValidTrip;
            customer.AddTrip(new CustomerTrip(Guid.NewGuid(), customer, firstTrip));
            var secondTripStartDate = firstTrip.EndDateUTC + TimeSpan.FromDays(1);

            customer.AddTrip(new CustomerTrip(Guid.NewGuid(), customer,
                new Trip(Guid.NewGuid(),"TripName", "Destination", secondTripStartDate,
                    secondTripStartDate + TimeSpan.FromDays(1), new MockedDateOffsetProvider(DateTimeOffset.UtcNow))));
        }

        [Fact]
        public void assign_customer_to_null_trip_expect_exception()
        {
            var customer = DomainTestsHelper.ValidCustomer;
            Action act = () => customer.AddTrip(null);
            act.Should().Throw<DomainException>()
                .Where(ex => ex.ErrorCode == DomainErrorCodes.ArgumentNullOrEmpty);
        }

        [Fact]
        public void add_duplicated_trip_expect_exception()
        {
            var customer = new Customer(Guid.NewGuid(), "Jar", "Jar");
            var now = DateTimeOffset.Now;
            var trip = new Trip(Guid.NewGuid(),"TripName", "Destination",now,
                now + TimeSpan.FromDays(2), new MockedDateOffsetProvider(now.ToUniversalTime()));
            var customerTrip = new CustomerTrip(Guid.NewGuid(), customer, trip);

            customer.AddTrip(customerTrip);
            Action act = () => customer.AddTrip(customerTrip);
            act.Should().Throw<DomainException>().Where(ex =>
                ex.ErrorCode == DomainErrorCodes.CustomerAlreadyAssignedToThisTrip);
        }
    }
}