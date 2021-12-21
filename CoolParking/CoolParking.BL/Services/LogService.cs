// TODO: implement the LogService class from the ILogService interface.
//       One explicit requirement - for the read method, if the file is not found, an InvalidOperationException should be thrown
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in LogServiceTests you can find the necessary constructor format.
using CoolParking.BL.Interfaces;
using System;
using System.IO;
namespace CoolParking.BL.Services
{
    public class LogService : ILogService
    {
        private readonly string logFilePath;

        public LogService(ILogService s) { }

        public LogService(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        public string LogPath { get { return logFilePath; } }
        public void Dispose()
        {
            if (File.Exists(logFilePath))
                File.Delete(logFilePath);
        }
        public string Read()
        {
            if (!File.Exists(logFilePath))
                throw new InvalidOperationException();
            string log = "";
            using (StreamReader reader = new StreamReader(logFilePath))
            {
                log = reader.ReadToEnd();
            }
            return log;
        }

        public void Write(string logInfo)
        {
            using StreamWriter writer = new StreamWriter(logFilePath, true);
            writer.Write(logInfo + "\n");
        }
    }
}