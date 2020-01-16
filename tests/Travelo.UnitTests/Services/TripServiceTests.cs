using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Travelo.Core.Domain;
using Travelo.Core.Domain.DTO;
using Travelo.Core.Domain.Providers;
using Travelo.Core.Mappers;
using Travelo.Core.Repositories;
using Travelo.Core.Services;
using Travelo.UnitTests.Helpers;
using Xunit;

namespace Travelo.UnitTests.Services
{
    public class TripServiceTests
    {
        [Fact]
        public async Task get_trip_async_should_invoke_repository_method()
        {
            var mockedRepository = new Mock<ITripRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            var mockedMapper = new Mock<ITraveloMapper>();
            mockedRepository.Setup(x => x.GetTripAsync(It.IsAny<Guid>())).ReturnsAsync(DomainTestsHelper.ValidTrip);
            var mockedDateTimeProvider = new Mock<IDateTimeOffsetProvider>();
            var tripService =
                new TripService(mockedRepository.Object, customerRepository.Object, mockedMapper.Object,
                    mockedDateTimeProvider.Object);

            await tripService.GetTripAsync(Guid.NewGuid());
            mockedRepository.Verify(x => x.GetTripAsync(It.IsAny<Guid>()), Times.Once());
        }

        [Fact]
        public async Task get_trips_async_should_invoke_repository_method()
        {
            var mockedRepository = new Mock<ITripRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            var mockedMapper = new Mock<ITraveloMapper>();
            var mockedDateTimeProvider = new Mock<IDateTimeOffsetProvider>();
            var tripService =
                new TripService(mockedRepository.Object, customerRepository.Object, mockedMapper.Object,
                    mockedDateTimeProvider.Object);

            await tripService.GetTripsAsync();
            mockedRepository.Verify(x => x.GetTripsAsync(), Times.Once());
        }

        [Fact]
        public async Task add_trip_async_should_invoke_repository_method()
        {
            var mockedRepository = new Mock<ITripRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            var mockedMapper = new Mock<ITraveloMapper>();
            var mockedDateTimeProvider = new MockedDateOffsetProvider(DateTimeOffset.Now);

            var tripService =
                new TripService(mockedRepository.Object, customerRepository.Object, mockedMapper.Object,
                    mockedDateTimeProvider);

            var now = DateTimeOffset.Now;
            await tripService.AddTripAsync(new TripDTO()
            {
                StartDate = now, EndDate = now + TimeSpan.FromDays(7), Name = "Trip name", Destination = "Berlin",
                Description = "Long trip"
            });
            mockedRepository.Verify(x => x.AddTripAsync(It.IsAny<Trip>()), Times.Once());
        }

        [Fact]
        public async Task cancel_trip_async_should_invoke_repository_method()
        {
            var mockedRepository = new Mock<ITripRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            mockedRepository.Setup(x => x.GetTripAsync(It.IsAny<Guid>())).ReturnsAsync(DomainTestsHelper.ValidTrip);
            var mockedMapper = new Mock<ITraveloMapper>();
            var mockedDateTimeProvider = new Mock<IDateTimeOffsetProvider>();

            var tripService =
                new TripService(mockedRepository.Object, customerRepository.Object, mockedMapper.Object,
                    mockedDateTimeProvider.Object);

            var id = Guid.NewGuid();
            await tripService.EditTripAsync(id,
                new TripDTO()
                {
                    Customers = new List<CustomerDTO>(), StartDate = DateTimeOffset.Now,
                    EndDate = DateTimeOffset.Now + TimeSpan.FromDays(7),
                    Name ="Hyper trip",
                    Destination = "Barzil"
                });
            mockedRepository.Verify(x => x.GetTripAsync(It.IsAny<Guid>()), Times.Once);
            mockedRepository.Verify(x => x.UpdateTripAsync(It.IsAny<Trip>()), Times.Once);
        }
    }
}