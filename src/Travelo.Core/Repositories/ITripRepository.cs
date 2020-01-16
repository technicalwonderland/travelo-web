using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Travelo.Core.Domain;

namespace Travelo.Core.Repositories
{
    public interface ITripRepository
    {
        Task<Trip> GetTripAsync(Guid id);
        Task<IEnumerable<Trip>> GetTripsAsync();

        Task AddCustomerToTripAsync(Trip trip, Customer customer);
        Task AddTripAsync(Trip trip);
        Task UpdateTripAsync(Trip trip);
    }
}