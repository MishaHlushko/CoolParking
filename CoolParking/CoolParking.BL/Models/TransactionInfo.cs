// TODO: implement struct TransactionInfo.
//       Necessarily implement the Sum property (decimal) - is used in tests.
//       Other implementation details are up to you, they just have to meet the requirements of the homework.
namespace CoolParking.BL.Models
{
    public struct TransactionInfo
    {
        public decimal Sum { get; set; }
        public string VehicleId { get; set; }
        public System.DateTime TimeTransaction { get; set; }
        public TransactionInfo(decimal sum, string vehicleId, System.DateTime timeTransaction)
        {
            Sum = sum;
            VehicleId = vehicleId;
            TimeTransaction = timeTransaction;
        }


    }
}