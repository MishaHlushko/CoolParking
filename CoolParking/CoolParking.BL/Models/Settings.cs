// TODO: implement class Settings.
//       Implementation details are up to you, they just have to meet the requirements of the home task.
namespace CoolParking.BL.Models
{
    public static class Settings
    {
        public static decimal Balance { get; } = 0;
        public static int AmountPlaces { get; } = 10;
        public static int TPay { get; } = 5000;
        public static int TLog { get; } = 60000;
        public static decimal Fine { get; } = 2.5m;
        static public string LogPath { get; set; } = @".\Transactions.log";
        public static decimal Bill(VehicleType vehicle)
        {
            return vehicle switch
            {
                VehicleType.Bus => 3.5m,
                VehicleType.Motorcycle => 1,
                VehicleType.PassengerCar => 2,
                VehicleType.Truck => 5,
                _ => -1,
            };
        }
    }
}