using System;
using CoolParkingConsole.ClientModels;
using CoolParkingConsole;
using System.Collections.ObjectModel;

namespace CoolParking
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayMainMessage();
            MainMethod();
        }
        static void DisplayMainMessage()
        {
            Console.WriteLine("Enter a number what have to do:\n" +
               "1:Display current balance of parking\n" +
               "2:Display capacity of the parking\n" +
               "3:Display amount of free place on parking\n" +
               "4:Display history of transactions for current period\n" +
               "5:Display list of transports which on the parking now\n" +
               "6:Add transpory on parking\n" +
               "7:Remove transport from the parking\n" +
               "8:Top up balance of a specific transport\n" +
               "9:Get a transport by id\n" +
               "10:Read information from log file(can be a lot of text)\n" +
               "11:Clear console and come back)\n" +
               "12:Exit program\n");
        }

        static string GetNormalStringOfTransactions()
        {
            string rez = "";
            TransactionInfoClient[] transactionInfos = ParkingServiceHTTP.GetLastTransactions().Result;
            for (int i = 0; i < transactionInfos.Length; i++)
            {
                rez += $"{transactionInfos[i].TimeTransaction:T} -- {transactionInfos[i].VehicleId}  {transactionInfos[i].Sum}\n";
            }
            return rez;
        }

        static void MainMethod()
        {

            string number = Console.ReadLine();
            try
            {
                switch (number)
                {
                    case "1":
                        Console.WriteLine($"Current balance is {ParkingServiceHTTP.GetParkingHttp("balance").Result}\n");
                        break;
                    case "2":
                        Console.WriteLine($"The capacity of the parking is {ParkingServiceHTTP.GetParkingHttp("capacity").Result}\n");
                        break;
                    case "3":
                        Console.WriteLine($"Amount free place of parking is {ParkingServiceHTTP.GetParkingHttp("freePlaces").Result}");
                        break;
                    case "4":
                        Console.WriteLine($"The history of transactions for current period is {GetNormalStringOfTransactions()}");
                        break;
                    case "5":
                        ReadOnlyCollection<VehicleClient> clients = new ReadOnlyCollection<VehicleClient>(ParkingServiceHTTP.GetVehicles().Result);
                        for (int i = 0; i < clients.Count; i++)
                            Console.Write(clients[i].Id + " with balance " + clients[i].Balance + "\n");
                        break;

                    case "6":
                        Console.WriteLine("Enter transport id, type" +
                            "(1 - Bus, 2 - PassengerCar, 3 - Truck and 4 - Motorcycle) and how much money you give");
                        string id = Console.ReadLine();
                        int type = Convert.ToInt32(Console.ReadLine());
                        decimal money = Convert.ToDecimal(Console.ReadLine());
                        VehicleClient vehicle = new VehicleClient(id, type, money);
                        string rez = ParkingServiceHTTP.AddVehicle(vehicle).Result;
                        Console.WriteLine($"{rez}\n");
                        break;

                    case "7":
                        Console.WriteLine("Enter the id of transport you want to remove:");
                        string idRemove = Console.ReadLine();
                        string rezRev = ParkingServiceHTTP.RemoveVehicle(idRemove).Result;
                        Console.WriteLine(rezRev + "\n");
                        break;

                    case "8":
                        Console.WriteLine("Enter sum and id of transport:");
                        decimal sum = Convert.ToDecimal(Console.ReadLine());
                        string ids = Console.ReadLine();
                        VehicleClient rezTopUp = ParkingServiceHTTP.TopUpVehicle(ids, sum).Result;
                        Console.WriteLine($"Now {rezTopUp.Id} is staying on parking with balance {rezTopUp.Balance}\n");
                        break;

                    case "9":
                        Console.WriteLine("Enter the id of transport:");
                        string IdTr = Console.ReadLine();
                        VehicleClient rezGet = ParkingServiceHTTP.GetVehicleById(IdTr).Result;
                        Console.WriteLine($"Vehicle with id {rezGet.Id} with balance {rezGet.Balance} is staying now on the parking\n");
                        break;

                    case "10":
                        Console.WriteLine(ParkingServiceHTTP.GetAllTransactions().Result);
                        break;

                    case "11":
                        Console.Clear();
                        break;

                    case "12":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Entered incorrect number, try again");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Mistake:{ex.Message}");
            }

            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Mistake:{ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Mistake:{ex.Message}");
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mistake:{ex.Message}");
            }
            finally
            {
                DisplayMainMessage();
                MainMethod();
            }
        }
    }
}
