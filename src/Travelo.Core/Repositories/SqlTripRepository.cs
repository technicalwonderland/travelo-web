using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travelo.Core.Domain;
using Travelo.DataStore;

namespace Travelo.Core.Repositories
{
    public class SqlTripRepository : ITripRepository
    {
        private readonly TraveloDataContext _context;

        public SqlTripRepository(TraveloDataContext context)
        {
            _context = context;
        }

        public async Task<Trip> GetTripAsync(Guid id)
            => await _context.Trips.Include(x => x.CustomerTrips).ThenInclude(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Trip>> GetTripsAsync()
            => await _context.Trips.Include(x => x.CustomerTrips).ThenInclude(x => x.Customer).ToListAsync();

        public async Task AddTripAsync(Trip trip)
        {
            try
            {
                await _context.Trips.AddAsync(trip);
                await _context.SaveChangesAsync();
            }
            catch (ArgumentException argumentException)
            {
                await CheckIfTripExistsOrThrowAsync(trip.Id, argumentException);
            }
        }
        
        public async Task AddCustomerToTripAsync(Trip trip, Customer customer)
        {
            trip.AddCustomer(customer);
            await UpdateTripAsync(trip);
        }

        public async Task UpdateTripAsync(Trip trip)
        {
            try
            {
                _context.Trips.Update(trip);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CheckIfTripExists(trip.Id))
                {
                    throw new DomainException(DomainErrorCodes.TripDoesNotExists, null,
                        $"Trip with id: {trip.Id} does not exists.");
                }
            }
        }
        
        private async Task CheckIfTripExistsOrThrowAsync(Guid id, ArgumentException argumentException = null)
        {
            if (await CheckIfTripExists(id))
            {
                throw new DomainException(DomainErrorCodes.TripAlreadyExists, argumentException,
                    $"Trip with id: {id} already exists.");
            }
        }

        private async Task<bool> CheckIfTripExists(Guid id)
        {
            return await _context.Trips.AnyAsync(x => x.Id == id);
        }
    }
}