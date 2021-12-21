using CoolParking.BL.Models;
using System.Text.Json.Serialization;

namespace CoolParking.WebAPI.ServerModels
{
    public class VenicleServer
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("vehicleType")]
        public VehicleType VehicleType { get; set; }
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
    }
}
