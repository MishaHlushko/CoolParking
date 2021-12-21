using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.WebAPI.ServerModels;

namespace CoolParking.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private IParkingService Service;
        public TransactionsController(IParkingService service)
        {
            Service = service;
        }

        [HttpGet]
        [Route("last")]
        public ActionResult<IReadOnlyCollection<TransactionInfo>> GetLatsTransactions()
        {
            return Ok(Service.GetLastParkingTransactions());
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<string> AllTransactions()
        {
            if (!System.IO.File.Exists(Settings.LogPath))
                return NotFound("Log file not found");
            return Ok(Service.ReadFromLog());
        }

        [HttpPut]
        [Route("topUpVehicle")]
        public ActionResult<Vehicle> TopUpVehicle([FromBody] TopUp topUp)
        {
            Vehicle vehicle = Vehicle.GetVehicleById(topUp.Id);
            if (vehicle == null)
                return NotFound("Vehicle with this id did not found");
            if (topUp.Sum <= 0)
                return BadRequest("The sum can not be less than zero or equals zero");
            Service.TopUpVehicle(topUp.Id, topUp.Sum);
            return Ok(vehicle);
        }
    }
}