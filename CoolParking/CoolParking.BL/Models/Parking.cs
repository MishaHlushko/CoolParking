// TODO: implement class Parking.
//       Implementation details are up to you, they just have to meet the requirements 
//       of the home task and be consistent with other classes and tests.
using System.Collections.Generic;
namespace CoolParking.BL.Models
{
    public class Parking
    {
        public decimal CurentBalance { get; set; } = Settings.Balance;
        private static Parking parking;//singlton
        public IList<TransactionInfo> TransactionInfos { get; set; }
        public decimal Balance { get; set; } = Settings.Balance;//баланс
        public IList<Vehicle> Vehicles { get; set; }//колекція транспортних засобів      
        public Dictionary<int, bool> ParkingPlaces { get; set; }//паркувальні місце(ключ - номер місця, значення - чи є це місце вільним)

        private Parking()
        {
            Initialization();
        }

        public static Parking GetParking()
        {
            if (parking == null)
                parking = new Parking();
            return parking;
        }

        private void Initialization()
        {
            ParkingPlaces = new Dictionary<int, bool>();
            for (int i = 0; i < Settings.AmountPlaces; i++)
                ParkingPlaces.Add(i, true);
            Vehicles = new List<Vehicle>();
            TransactionInfos = new List<TransactionInfo>();
        }

        public int GetFreeParkingPlace()//метод який повертає перше вільне паркувальне місце
        {
            for (int i = 0; i < ParkingPlaces.Count; i++)
            {
                if (ParkingPlaces[i])
                    return i;
            }
            throw new System.InvalidOperationException("Free parking place is not found");
        }
    }
}