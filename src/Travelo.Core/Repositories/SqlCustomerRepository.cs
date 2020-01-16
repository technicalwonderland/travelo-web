using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travelo.Core.Domain;
using Travelo.DataStore;

namespace Travelo.Core.Repositories
{
    public class SqlCustomerRepository : ICustomerRepository
    {
        private readonly TraveloDataContext _context;

        public SqlCustomerRepository(TraveloDataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<Customer> GetCustomerAsync(Guid customerId)
            => await _context.Customers.Include(x => x.CustomerTrips).ThenInclude(x => x.Trip)
                .FirstOrDefaultAsync(x => x.Id == customerId);

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
            => await _context.Customers.Include(x => x.CustomerTrips).ThenInclude(x => x.Trip).ToListAsync();

        public async Task<IEnumerable<Customer>> GetCustomersByFullNameAsync(string fullname)
        {
            if (string.IsNullOrWhiteSpace(fullname))
            {
                throw new DomainException(DomainErrorCodes.ArgumentNullOrEmpty, null, "Fullname cannot be empty");
            }
            
            return await _context.Customers.Where(x => x.FullName.Contains(fullname)).ToListAsync();
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            try
            {
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
            }
            catch (ArgumentException argumentException)
            {
                if (await CheckIfCustomerExists(customer.Id))
                {
                    throw new DomainException(DomainErrorCodes.CustomerAlreadyExists, argumentException,
                        $"Customer with id: {customer.Id} already exists.");
                }
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CheckIfCustomerExists(customer.Id))
                {
                    throw new DomainException(DomainErrorCodes.CustomerDoesNotExist, null,
                        $"Customer with id: {customer.Id} does not exists.");
                }
            }
        }

        private async Task<bool> CheckIfCustomerExists(Guid id)
        {
            return await _context.Customers.AnyAsync(x => x.Id == id);
        }
    }
}