using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace CoolParkingConsole.ClientModels
{
    public class VehicleClient
    {
        private int vehicleType;

        public VehicleClient(string id, int vehicleType, decimal balance)
        {
            Id = id;
            VehicleType = vehicleType;
            Balance = balance;
        }

        [JsonPropertyName("id")]
        public string Id { get; set; }


        [JsonPropertyName("vehicleType")]
        public int VehicleType
        {
            get => vehicleType; set
            {
                if (value > 4 || value <= 0)
                    throw new ArgumentException("Vehicle type value can be only between 1 and 4");
                vehicleType = value;
            }
        }

        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
    }
}