using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Travelo.API;
using Travelo.Core.Domain.DTO;
using Xunit;

namespace Travelo.EndToEnd.Controllers
{
    public class CustomerControllerTests:BaseControllerTests
    {
        private readonly CustomerDTO _validCustomerDto;
        private readonly string _customersApi = "api/customers";
        private readonly TestServer _testServer;
        private readonly HttpClient _client;

        public CustomerControllerTests():base()
        {
            _validCustomerDto = new CustomerDTO {FirstName = "Adam", LastName = "Adamski"};
            _testServer = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                }).UseStartup<Startup>());
            _client = _testServer.CreateClient();
        }

        [Fact]
        public async Task creating_new_unique_customer_and_fetching_return_one_item()
        {
            var response = await _client.PostAsync(_customersApi,
                GetPayload(_validCustomerDto));
            response.EnsureSuccessStatusCode();

            response = await _client.GetAsync($"api/customers");
            response.EnsureSuccessStatusCode();
            var payload = await response.Content.ReadAsStringAsync();

            var customerDtos = JsonConvert.DeserializeObject<List<CustomerDTO>>(payload);
            customerDtos.Count.Should().BeGreaterThan(0);
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public async Task creating_new_unique_customer_succeed()
        {
            var response = await _client.PostAsync(_customersApi,
                GetPayload(_validCustomerDto));
            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
        }

        [Fact]
        public async Task update_customer_succeed()
        {
            var response = await _client.PostAsync(_customersApi,
                GetPayload(_validCustomerDto));
            response.EnsureSuccessStatusCode();

            response = await _client.PutAsync($"api/{response.Headers.Location.ToString()}",
                GetPayload(new CustomerDTO() {FirstName = "Aaaa", LastName = "bbbb"}));

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public async Task create_customer_and_retrieve_it()
        {
            var firstName = "Grumpy";
            var lastName = "Mom";
            var response = await _client.PostAsync(_customersApi,
                GetPayload(new CustomerDTO() {FirstName = firstName, LastName = lastName}));
            response.EnsureSuccessStatusCode();

            var locationString = response.Headers.Location.ToString();

            response = await _client.GetAsync($"api/{locationString}");
            var payload = await response.Content.ReadAsStringAsync();
            var customerDto = JsonConvert.DeserializeObject<CustomerDTO>(payload);
            customerDto.FirstName.Should().Be(firstName);
            customerDto.LastName.Should().Be(lastName);
            Assert.Equal(locationString.Split("/").Last(), customerDto.Id.ToString());
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        }
        
    }
}