using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolParking.BL.Services;
using CoolParking.BL.Interfaces;
using System.Text.Json;

namespace CoolParking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        public ParkingController(IParkingService parking)
        {
            Service = parking;
        }
        private IParkingService Service { get; set; }


        [Route("capacity")]
        [HttpGet]
        public ActionResult<int> GetCapacity() => Ok(Service.GetCapacity());


        [Route("balance")]
        [HttpGet]
        public ActionResult<decimal> GetBalance() => Ok(Service.GetBalance());


        [Route("freePlaces")]
        [HttpGet]
        public ActionResult<int> GetFreePlaces() => Ok(Service.GetFreePlaces());

    }
}
