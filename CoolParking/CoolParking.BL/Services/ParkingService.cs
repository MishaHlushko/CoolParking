// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace CoolParking.BL.Services
{
    public class ParkingService : IParkingService
    {
        private Parking Park { get; set; } = Parking.GetParking();
        public ILogService LogFileService { get; set; }
        ITimerService WithdrawTimer { get; set; }
        public ParkingService() { }
        public ParkingService(bool IsStartTimer)
        {
            if (IsStartTimer)
            {
                WithdrawTimer = new TimeService();
                WithdrawTimer.Start();
            }
            LogFileService = new LogService(Settings.LogPath);
        }

        public ParkingService(ITimerService withdrawTimer, ITimerService logTimer, ILogService logService)
        {
            LogFileService = logService;
            withdrawTimer.Elapsed += (sender, e) => new TimeService().GetMoney(sender, e);
            WithdrawTimer = withdrawTimer;
        }
        public void AddVehicle(Vehicle vehicle)
        {

            if (Park.Vehicles.Count >= Settings.AmountPlaces)
                throw new System.InvalidOperationException("Try to add auto on full parking");
            if (IsNotEqualId(vehicle.Id))
            {
                Park.Vehicles.Add(vehicle);
                vehicle.ParkingPlaceNumber = Park.GetFreeParkingPlace();
                Park.ParkingPlaces[Park.GetFreeParkingPlace()] = false;//місце зайнято
                return;
            }
            throw new System.ArgumentException("Auto with this id is already stay on cool parking");

        }
        private bool IsNotEqualId(string vehicleId) => !Park.Vehicles.Any(Vehicle => Vehicle.Id == vehicleId);
        public void Dispose()
        {
            Park.Balance = Settings.Balance;
            Park.ParkingPlaces.Clear();
            for (int i = 0; i < Settings.AmountPlaces; i++)
                Park.ParkingPlaces.Add(i, true);
            Park.Vehicles.Clear();
            WithdrawTimer?.Dispose();
            Park.TransactionInfos.Clear();
            if (System.IO.File.Exists(Settings.LogPath))
                System.IO.File.Delete(Settings.LogPath);
            System.GC.Collect();
        }

        public decimal GetBalance()
        {
            return Park.Balance;
        }

        public int GetCapacity()
        {
            return Settings.AmountPlaces;
        }
        public string GetFreePlaces(bool IsNormalize = false)
        {
            string rez = "";
            for (int i = 0; i < Park.ParkingPlaces.Count; i++)
            {
                if (Park.ParkingPlaces[i])
                    rez += i;
                if (IsNormalize && Park.ParkingPlaces[i])
                    rez += ", ";
            }
            return rez;//номери від 0 до 10 вільних місць в один рядок...
        }
        public int GetFreePlaces() => Park.ParkingPlaces.Where(Park => Park.Value).Count();

        public TransactionInfo[] GetLastParkingTransactions()
        {
            return Park.TransactionInfos.ToArray();
        }

        public ReadOnlyCollection<Vehicle> GetVehicles()
        {
            ReadOnlyCollection<Vehicle> collection = new ReadOnlyCollection<Vehicle>(Park.Vehicles);
            return collection;
        }

        public string ReadFromLog()
        {
            LogService log = new(Settings.LogPath);
            return log.Read();
        }

        public void RemoveVehicle(string vehicleId)
        {
            if (!IsNotEqualId(vehicleId))
            {
                if (Vehicle.GetVehicleById(vehicleId).Balance < 0)
                    throw new System.InvalidOperationException("Try to remove transport with negative balance");
                Park.ParkingPlaces[Vehicle.GetVehicleById(vehicleId).ParkingPlaceNumber] = true;
                Park.Vehicles.Remove(Vehicle.GetVehicleById(vehicleId));
                return;
            }
            throw new System.ArgumentException("Id is not right");
        }

        public void TopUpVehicle(string vehicleId, decimal sum)
        {
            if (sum <= 0)
                throw new System.ArgumentException("The sum less zero or equals zero");
            Vehicle vehicle = Vehicle.GetVehicleById(vehicleId);
            if (vehicle != null)
            {
                vehicle.Balance += sum;
                return;
            }
            throw new System.ArgumentException("Id is not right");
        }
    }
}