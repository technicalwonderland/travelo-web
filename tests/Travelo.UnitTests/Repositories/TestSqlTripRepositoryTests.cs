using System;
using System.Threading.Tasks;
using FluentAssertions;
using Travelo.Core.Domain;
using Travelo.Core.Repositories;
using Travelo.UnitTests.Helpers;
using Xunit;

namespace Travelo.UnitTests.Repositories
{
    public class TestSqlTripRepositoryTests : BaseTestSqlRepository
    {
        private ITripRepository tripRepository;

        public TestSqlTripRepositoryTests()
        {
            tripRepository = GetTripRepository();
        }

        private ITripRepository GetTripRepository()
        {
            return new SqlTripRepository(GetMockedTraveloDataContext());
        }

        [Fact]
        public async Task add_async_proper_data_should_create_trip_expect_success()
        {
            var tripId = Guid.NewGuid();
            var trip = DomainTestsHelper.ValidTripWithIdAndName(tripId,"trip");
            await tripRepository.AddTripAsync(trip);
            var fetchedTrip = await tripRepository.GetTripAsync(tripId);
            Assert.Same(trip, fetchedTrip); 
        }
        
        [Fact]
        public async Task add_async_multiple_trips_get_customers_return_all_expect_success()
        {
            var tripId1 = Guid.NewGuid();
            var tripId2 = Guid.NewGuid();
            var trip1 = DomainTestsHelper.ValidTripWithIdAndName(tripId1,"trip1");
            var trip2 = DomainTestsHelper.ValidTripWithIdAndName(tripId2, "trip2");

            await tripRepository.AddTripAsync(trip1);
            await tripRepository.AddTripAsync(trip2);
            var trips = await tripRepository.GetTripsAsync();
            trips.Should().Contain(trip1);
            trips.Should().Contain(trip2);
        }
        
        [Fact]
        public async Task add_async_same_customer_multiple_times_expect_exception()
        {
            var tripId = Guid.NewGuid();
            var trip = DomainTestsHelper.ValidTripWithIdAndName(tripId, "tripName");
            await tripRepository.AddTripAsync(trip);

            var exception = await Assert.ThrowsAsync<DomainException>(() => tripRepository.AddTripAsync(trip));
            exception.ErrorCode.Should().Be(DomainErrorCodes.TripAlreadyExists);
        }
        
        
    }
}