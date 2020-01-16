using System;
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
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Travelo.EndToEnd.Controllers
{
    public class TripControllerTests : BaseControllerTests
    {
        private readonly string _tripsApi = "api/trips";
        private readonly TripDTO _validTripDto;
        private readonly TestServer _testServer;
        private readonly HttpClient _client;

        public TripControllerTests() : base()
        {
            _validTripDto = new TripDTO()
            {
                StartDate = DateTimeOffset.Now + TimeSpan.FromDays(1),
                EndDate = DateTimeOffset.Now + TimeSpan.FromDays(7),
                Name = "TripName",
                Destination = "Trip destination",
                Description = "Long description"
            };
            
            _testServer = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                }).UseStartup<Startup>());
            _client = _testServer.CreateClient();
        }

        [Fact]
        public async Task fetching_trips_should_return_empty_collection()
        {
            var response = await _client.GetAsync(_tripsApi);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var bucketNames = JsonSerializer.Deserialize<IEnumerable<string>>(content);
            bucketNames.Should().BeEmpty();
        }

        [Fact]
        public async Task creating_new_unique_customer_succeed()
        {
            var response = await _client.PostAsync(_tripsApi,
                GetPayload(_validTripDto));
            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
        }

        [Fact]
        public async Task create_customer_and_retrieve_it()
        {
            var response = await _client.PostAsync(_tripsApi,
                GetPayload(_validTripDto));
            response.EnsureSuccessStatusCode();
            var locationString = response.Headers.Location.ToString();

            response = await _client.GetAsync($"api/{locationString}");
            var payload = await response.Content.ReadAsStringAsync();
            var tripDto = JsonConvert.DeserializeObject<TripDTO>(payload);
            tripDto.StartDate.Should().Be(_validTripDto.StartDate);
            tripDto.EndDate.Should().Be(_validTripDto.EndDate);
            Assert.Equal(locationString.Split("/").Last(), tripDto.Id.ToString());
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        }
    }
}