using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoolParkingConsole.ClientModels
{
    public class TransactionInfoClient
    {
        [JsonPropertyName("sum")]
        public decimal Sum { get; set; }
        [JsonPropertyName("vehicleId")]
        public string VehicleId { get; set; }
        [JsonPropertyName("timeTransaction")]
        public DateTime TimeTransaction { get; set; }
    }
}
