using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Travelo.Core.Domain;
using Travelo.Core.Mappers;
using Travelo.Core.Repositories;
using Travelo.Core.Services;
using Travelo.UnitTests.Helpers;
using Xunit;

namespace Travelo.UnitTests.Services
{
    public class CustomerServiceTests
    {
        [Fact]
        public async Task get_customer_async_should_invoke_repository_method()
        {
            var mockedRepository = new Mock<ICustomerRepository>();
            mockedRepository.Setup(x => x.GetCustomerAsync(It.IsAny<Guid>()))
                .ReturnsAsync(DomainTestsHelper.ValidCustomer);
            var mockedMapper = new Mock<ITraveloMapper>();
            var customerService =
                new CustomerService(mockedRepository.Object, mockedMapper.Object);

            await customerService.GetCustomerAsync(Guid.NewGuid());
            mockedRepository.Verify(x => x.GetCustomerAsync(It.IsAny<Guid>()), Times.Once());
        }

        [Fact]
        public async Task get_customers_async_should_invoke_repository_method()
        {
            var mockedRepository = new Mock<ICustomerRepository>();
            var mockedMapper = new Mock<ITraveloMapper>();
            var customerService =
                new CustomerService(mockedRepository.Object, mockedMapper.Object);

            await customerService.GetCustomersAsync();
            mockedRepository.Verify(x => x.GetCustomersAsync(), Times.Once());
        }

        [Fact]
        public async Task add_customer_async_should_invoke_repository_method()
        {
            var mockedRepository = new Mock<ICustomerRepository>();
            var mockedMapper = new Mock<ITraveloMapper>();
            var customerService =
                new CustomerService(mockedRepository.Object, mockedMapper.Object);

            await customerService.AddCustomerAsync("Frank", "Sinatra");
            mockedRepository.Verify(x => x.AddCustomerAsync(It.IsAny<Customer>()), Times.Once());
        }

        [Fact]
        public async Task edit_customer_async_should_invoke_repository_method()
        {
            var mockedRepository = new Mock<ICustomerRepository>();
            mockedRepository.Setup(x => x.GetCustomerAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => DomainTestsHelper.ValidCustomer);
            var mockedMapper = new Mock<ITraveloMapper>();
            var customerService =
                new CustomerService(mockedRepository.Object, mockedMapper.Object);

            await customerService.EditCustomerAsync(Guid.NewGuid(), "James", "Howlett");
            mockedRepository.Verify(x => x.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Once());
            mockedRepository.Verify(x => x.GetCustomerAsync(It.IsAny<Guid>()), Times.Once());
        }
    }
}