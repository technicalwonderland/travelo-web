using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Travelo.Core.Domain;
using Travelo.Core.Repositories;
using Travelo.DataStore;
using Travelo.UnitTests.Helpers;
using Xunit;

namespace Travelo.UnitTests.Repositories
{
    public class TestSqlCustomerRepositoryTests: BaseTestSqlRepository
    {
        private ICustomerRepository customerRepository;

        public TestSqlCustomerRepositoryTests()
        {
            customerRepository = GetCustomerRepository();
        }

        [Fact]
        public async Task add_async_with_proper_data_should_create_customer_expect_success()
        {
            var customerId = Guid.NewGuid();
            var customer = DomainTestsHelper.ValidCustomerWithId(customerId);
            await customerRepository.AddCustomerAsync(customer);
            var fetchedCustomer = await customerRepository.GetCustomerAsync(customerId);
            Assert.Same(customer, fetchedCustomer);
        }

        [Fact]
        public async Task add_async_multiple_customers_get_customers_return_all_expect_success()
        {
            var customerId = Guid.NewGuid();
            var customer = DomainTestsHelper.ValidCustomerWithId(customerId);
            var customer2 = new Customer(Guid.NewGuid(),"Adamu","ADamu");

            await customerRepository.AddCustomerAsync(customer);
            await customerRepository.AddCustomerAsync(customer2);
            var customers = await customerRepository.GetCustomersAsync();
            customers.Should().Contain(customer);
            customers.Should().Contain(customer2);
        }

        [Fact]
        public async Task add_async_same_customer_multiple_times_expect_exception()
        {
            var customerId = Guid.NewGuid();
            var customer = DomainTestsHelper.ValidCustomerWithId(customerId);
            await customerRepository.AddCustomerAsync(customer);

            var exception = await Assert.ThrowsAsync<DomainException>(() => customerRepository.AddCustomerAsync(customer));
            exception.ErrorCode.Should().Be(DomainErrorCodes.CustomerAlreadyExists);
        }

        private ICustomerRepository GetCustomerRepository()
        {
            return new SqlCustomerRepository(GetMockedTraveloDataContext());
        }
    }
}