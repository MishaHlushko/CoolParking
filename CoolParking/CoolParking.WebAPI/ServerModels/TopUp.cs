using System.Text.Json.Serialization;

namespace CoolParking.WebAPI.ServerModels
{
    public class TopUp
    {
        public decimal Sum { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
