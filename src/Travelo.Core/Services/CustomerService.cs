using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelo.Core.Domain;
using Travelo.Core.Domain.DTO;
using Travelo.Core.Mappers;
using Travelo.Core.Repositories;

namespace Travelo.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITraveloMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, ITraveloMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDTO> GetCustomerAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetCustomerAsync(customerId);
            CheckCustomerNullOrFail(customer);
            return _mapper.MapToCustomerDto(customer);
        }

        public async Task<IEnumerable<CustomerDTO>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetCustomersAsync();
            return customers.Select(x => _mapper.MapToCustomerDto(x));
        }

        public async Task<IEnumerable<CustomerDTO>> GetCustomersByFullNameAsync(string fullname)
        {
            var customers = await _customerRepository.GetCustomersByFullNameAsync(fullname);
            return customers.Select(x => _mapper.MapToCustomerDto(x));
        }

        public async Task<Guid> AddCustomerAsync(string firstName, string lastName)
        {
            var id = Guid.NewGuid();
            await _customerRepository.AddCustomerAsync(
                new Customer(id, firstName, lastName));
            return id;
        }

        public async Task EditCustomerAsync(Guid id, string firstName, string lastName)
        {
            var customer = await _customerRepository.GetCustomerAsync(id);
            CheckCustomerNullOrFail(customer);
            customer.EditCustomer(firstName, lastName);
            await _customerRepository.UpdateCustomerAsync(customer);
        }

        private void CheckCustomerNullOrFail(Customer customer)
        {
            if (customer == null)
            {
                throw new DomainException(DomainErrorCodes.CustomerDoesNotExist, null, "Customer does not exists!");
            }
        }
    }
}