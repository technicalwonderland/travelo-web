using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelo.Core.Domain;
using Travelo.Core.Domain.DTO;
using Travelo.Core.Domain.Providers;
using Travelo.Core.Mappers;
using Travelo.Core.Repositories;

namespace Travelo.Core.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITraveloMapper _mapper;
        private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;
        private readonly ICustomerRepository _customerRepository;

        public TripService(ITripRepository tripRepository, ICustomerRepository customerRepository,
            ITraveloMapper mapper,
            IDateTimeOffsetProvider dateTimeOffsetProvider)
        {
            _tripRepository = tripRepository;
            _mapper = mapper;
            _dateTimeOffsetProvider = dateTimeOffsetProvider;
            _customerRepository = customerRepository;
        }

        public async Task<TripDTO> GetTripAsync(Guid id)
        {
            var trip = await _tripRepository.GetTripAsync(id);
            CheckTripNullOrFail(trip);
            return _mapper.MapToTripDto(trip);
        }

        public async Task<IEnumerable<TripDTO>> GetTripsAsync()
        {
            var trips = await _tripRepository.GetTripsAsync();
            return trips.Select(x => _mapper.MapToTripDto(x));
        }

        public async Task<Guid> AddTripAsync(TripDTO tripDto)
        {
            var id = Guid.NewGuid();
            await _tripRepository.AddTripAsync(new Trip(id, tripDto.Name, tripDto.Destination, tripDto.StartDate,
                tripDto.EndDate,
                _dateTimeOffsetProvider));
            return id;
        }

        public async Task EditTripAsync(Guid tripId, TripDTO tripDto)
        {
            if (tripDto == null)
            {
                throw new DomainException(DomainErrorCodes.ArgumentNullOrEmpty, null, "Trip is null!");
            }
            var trip = await _tripRepository.GetTripAsync(tripId);
            CheckTripNullOrFail(trip);
            trip.Edit(tripDto.Name, tripDto.Destination, tripDto.TripStatus, tripDto.StartDate, tripDto.EndDate);
            await _tripRepository.UpdateTripAsync(trip);
        }

        public async Task AddCustomerToTripAsync(Guid tripId, Guid customerId)
        {
            var trip = await _tripRepository.GetTripAsync(tripId);
            if (trip.TripStatus == TripStatus.Cancelled)
            {
                throw new DomainException(DomainErrorCodes.AddingCustomerToCancelledTrip, null,
                    "Cannot add customer to cancelled trip");
            }

            var customer = await _customerRepository.GetCustomerAsync(customerId);

            CheckTripNullOrFail(trip);
            if (customer == null)
            {
                throw new DomainException(DomainErrorCodes.CustomerDoesNotExist, null, "Customer does not exist!");
            }

            await _tripRepository.AddCustomerToTripAsync(trip, customer);
        }

        private void CheckTripNullOrFail(Trip trip)
        {
            if (trip == null)
            {
                throw new DomainException(DomainErrorCodes.TripDoesNotExists, null, "Trip does not exist!");
            }
        }
    }
}