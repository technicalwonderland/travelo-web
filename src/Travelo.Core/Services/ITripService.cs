using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Travelo.Core.Domain.DTO;

namespace Travelo.Core.Services
{
    public interface ITripService
    {
        Task<TripDTO> GetTripAsync(Guid id);
        Task<IEnumerable<TripDTO>> GetTripsAsync();

        Task<Guid> AddTripAsync(TripDTO tripDto);
        Task EditTripAsync(Guid tripId, TripDTO tripDto);
        Task AddCustomerToTripAsync(Guid tripId, Guid customerId);
    }
}