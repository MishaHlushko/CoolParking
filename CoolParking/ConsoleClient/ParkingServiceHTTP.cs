using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using CoolParkingConsole.ClientModels;
using System.Net;

namespace CoolParkingConsole
{
    public static class ParkingServiceHTTP
    {
        private static HttpClient Client { get; set; } = new HttpClient();

        private static string BaseUrl = new string(@"https://localhost:5001/api/");

        public static async Task<string> GetParkingHttp(string LastEndpoint)
        {
            var resp = await Client.GetAsync(BaseUrl + $"parking/{LastEndpoint}");
            return await resp.Content.ReadAsStringAsync();
        }

        public static async Task<IList<VehicleClient>> GetVehicles()
        {
            var resp = await Client.GetAsync(BaseUrl + $"vehicles");
            return JsonSerializer.Deserialize<IList<VehicleClient>>(resp.Content.ReadAsStringAsync().Result);
        }

        public static async Task<VehicleClient> GetVehicleById(string id)//404 and 400 code 
        {
            var resp = await Client.GetAsync(BaseUrl + $"vehicles/{id}");
            if (resp.StatusCode == HttpStatusCode.NotFound || resp.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException("Id have incorrect format or could not found this id");
            string vehicle = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<VehicleClient>(vehicle);
        }

        public static async Task<string> AddVehicle(VehicleClient vehicle)//400
        {
            var resp = await Client.PostAsync(BaseUrl + "vehicles", new StringContent(JsonSerializer.Serialize(vehicle), Encoding.UTF8, "application/json"));
            if (resp.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException("This vehicle is already on parking or invalid body");
            return "This vehicle was successfully added:" + await resp.Content.ReadAsStringAsync();
        }

        public static async Task<string> RemoveVehicle(string id)//404 and 400
        {
            var resp = await Client.DeleteAsync(BaseUrl + $"vehicles/{id}");
            if (resp.StatusCode == HttpStatusCode.NotFound || resp.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException("Id have incorrect format or could not found this id");
            return "Successfully deleted:" + resp.StatusCode;
        }

        public static async Task<TransactionInfoClient[]> GetLastTransactions()
        {
            var resp = await Client.GetAsync(BaseUrl + "transactions/last");
            return JsonSerializer.Deserialize<TransactionInfoClient[]>(await resp.Content.ReadAsStringAsync());
        }

        public static async Task<string> GetAllTransactions()//404
        {
            var resp = await Client.GetAsync(BaseUrl + "transactions/all");
            if (resp.StatusCode == HttpStatusCode.NotFound)
                throw new ArgumentException("File not found");
            return await resp.Content.ReadAsStringAsync();
        }

        public static async Task<VehicleClient> TopUpVehicle(string id, decimal sum)// 400 and 404
        {
            var info = new
            {
                id = id,
                Sum = sum
            };
            var resp = await Client.PutAsync(BaseUrl + "transactions/topUpVehicle", new StringContent(JsonSerializer.Serialize(info), Encoding.UTF8, "application/json"));
            if (resp.StatusCode == HttpStatusCode.NotFound || resp.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException("Id not found or sum less than zero");
            return JsonSerializer.Deserialize<VehicleClient>(await resp.Content.ReadAsStringAsync());
        }

    }
}

