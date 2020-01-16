using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Travelo.Core.Domain.DTO;

namespace Travelo.Core.Services
{
    public interface ICustomerService
    {
        Task<CustomerDTO> GetCustomerAsync(Guid customerId);
        Task<IEnumerable<CustomerDTO>> GetCustomersAsync();
        Task<IEnumerable<CustomerDTO>> GetCustomersByFullNameAsync(string fullname);

        Task<Guid> AddCustomerAsync(string firstName, string lastName);
        Task EditCustomerAsync(Guid id, string firstName, string lastName);
    }
}