using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Travelo.API;

namespace Travelo.EndToEnd.Controllers
{
    public class BaseControllerTests
    {
        public BaseControllerTests()
        {

        }
        
        protected StringContent GetPayload(object data)
        {
            var json = JsonSerializer.Serialize(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}