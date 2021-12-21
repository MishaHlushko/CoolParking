// TODO: implement class Vehicle.
//       Properties: Id (string), VehicleType (VehicleType), Balance (decimal).
//       The format of the identifier is explained in the description of the home task.
//       Id and VehicleType should not be able for changing.
//       The Balance should be able to change only in the CoolParking.BL project.
//       The type of constructor is shown in the tests and the constructor should have a validation, which also is clear from the tests.
//       Static method GenerateRandomRegistrationPlateNumber should return a randomly generated unique identifier.
using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CoolParking.BL.Models
{
    public class Vehicle
    {
        public Vehicle() { }
        [JsonIgnore]
        public int ParkingPlaceNumber { get; set; }

        public string Id { get; private set; }

        public VehicleType VehicleType { get; private set; }

        public decimal Balance { get; set; }
        public Vehicle(string id, VehicleType vehicle, decimal money)
        {
            Regex regex = new Regex(@"[A-Z]{2}-\d{4}-[A-Z]{2}");
            if (!(regex.IsMatch(id)) || money < 0)
                throw new ArgumentException("money less than zero or id have incorrect format");
            Id = id;
            VehicleType = vehicle;
            Balance = money;
        }
        public static string GenerateRandomRegistrationPlateNumber()
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return letters[new Random().Next(0, 26)].ToString() + letters[new Random().Next(0, 26)].ToString()
                + "-"
                + new Random().Next(0, 10) + new Random().Next(0, 10)
                + new Random().Next(0, 10) + new Random().Next(0, 10)
                + "-"
                + letters[new Random().Next(0, 26)].ToString() + letters[new Random().Next(0, 26)].ToString();
        }
        public static Vehicle GetVehicleById(string Id)
        {
            Parking parking = Parking.GetParking();
            for (int i = 0; i < parking.Vehicles.Count; i++)
            {
                if (parking.Vehicles[i].Id == Id)
                    return parking.Vehicles[i];
            }
            return null;
        }
    }
}