using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Travelo.Core.Domain
{

    [JsonConverter(typeof(StringEnumConverter))]//on my mac there is problem with System.Text.Json !?
    public enum TripStatus
    {
        Active,
        Cancelled
    }
}