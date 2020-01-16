using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Travelo.Core.Domain;

namespace Travelo.Core.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerAsync(Guid customerId);
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<IEnumerable<Customer>> GetCustomersByFullNameAsync(string fullname);
        
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
    }
}