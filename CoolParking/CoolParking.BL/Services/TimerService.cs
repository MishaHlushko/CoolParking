// TODO: implement class TimerService from the ITimerService interface.
//       Service have to be just wrapper on System Timers.
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Generic;
namespace CoolParking.BL.Services
{

    public class TimeService : ITimerService
    {
        private readonly Parking parking = Parking.GetParking();
        ILogService LogService { get; set; }
        public double Interval { get; set; }

        public event ElapsedEventHandler Elapsed;
        private Timer TimerPay { get; set; }
        public Timer TimerLog { get; set; }
        public TimeService()
        {
            LogService = new LogService(Settings.LogPath);
        }
        public void Dispose()
        {
            TimerPay?.Dispose();
            TimerLog?.Dispose();
            ((LogService)LogService).Dispose();
        }

        public void Start()
        {
            Interval = Settings.TPay;
            TimerPay = new Timer(Interval);
            TimerPay.Elapsed += async (sender, e) => await Task.Run(() => GetMoney(sender, e));
            TimerPay.Start();


            Interval = Settings.TLog;
            TimerLog = new Timer(Interval);
            TimerLog.Elapsed += async (sender, e) => await Task.Run(() => LogTransactions(sender, e));
            TimerLog.Start();

        }

        public void Stop()
        {
            TimerPay?.Stop();
            TimerLog?.Stop();
        }
        public void FireElapsedEvent()
        {
            Elapsed?.Invoke(this, null);
        }
        public void GetMoney(object sender, ElapsedEventArgs e)
        {
            var Vehicles = parking.Vehicles;
            for (int i = 0; i < parking.Vehicles.Count; i++)
            {
                if (Vehicles[i].Balance >= Settings.Bill(Vehicles[i].VehicleType))//баланс транспорту більше за баланс оплати
                {
                    parking.TransactionInfos.Add(new TransactionInfo(Settings.Bill(Vehicles[i].VehicleType), Vehicles[i].Id, System.DateTime.Now));
                    parking.Balance += Settings.Bill(Vehicles[i].VehicleType);//додаємо гроші до паркінгу
                    parking.CurentBalance += Settings.Bill(Vehicles[i].VehicleType);
                    Vehicles[i].Balance -= Settings.Bill(Vehicles[i].VehicleType);//забираємо гроші з балансу транспорту
                    continue;
                }
                if (Vehicles[i].Balance > 0)//баланс транспорту просто більше нуля
                {
                    parking.TransactionInfos.Add(new TransactionInfo(Vehicles[i].Balance, Vehicles[i].Id, System.DateTime.Now));
                    decimal LessZero = Settings.Bill(Vehicles[i].VehicleType) - Vehicles[i].Balance;//вираховуємо гроші по яким будет платитися штраф
                    parking.Balance += Vehicles[i].Balance + LessZero * 2.5m; ;//весь баланс боржника переходить до балансу паркінгу
                    parking.CurentBalance += Vehicles[i].Balance + LessZero * 2.5m;
                    Vehicles[i].Balance -= LessZero * Settings.Fine + Vehicles[i].Balance;//вираховуємо баланс боржника
                    continue;
                }
                parking.TransactionInfos.Add(new TransactionInfo(Settings.Bill(Vehicles[i].VehicleType) * Settings.Fine, Vehicles[i].Id, System.DateTime.Now));
                Vehicles[i].Balance -= Settings.Bill(Vehicles[i].VehicleType) * Settings.Fine;
                parking.Balance += Settings.Bill(Vehicles[i].VehicleType) * Settings.Fine;
                parking.CurentBalance += Settings.Bill(Vehicles[i].VehicleType) * Settings.Fine;
            }

        }
        private void LogTransactions(object sender, ElapsedEventArgs e)
        {
            var transaction = parking.TransactionInfos;
            for (int i = 0; i < transaction.Count; i++)
            {
                LogOneTransaction(transaction[i]);
            }
            parking.TransactionInfos.Clear();
            parking.CurentBalance = 0;

        }
        private void LogOneTransaction(TransactionInfo transaction) => LogService.Write($"{transaction.TimeTransaction:T}\n{transaction.VehicleId}\n{transaction.Sum}\n\n");
    }
}