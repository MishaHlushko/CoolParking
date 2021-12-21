using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using CoolParking.BL.Models;
using CoolParking.BL.Interfaces;
using System.Text.RegularExpressions;
using CoolParking.WebAPI.ServerModels;

namespace CoolParking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private IParkingService Service { get; set; }
        public VehiclesController(IParkingService service)
        {
            Service = service;
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<Vehicle>> GetVehiclesCollection() => Ok(Service.GetVehicles());


        [HttpGet("{id}")]
        public ActionResult<Vehicle> GetVehiclesById(string id)
        {
            Regex regex = new Regex(@"[A-Z]{2}-\d{4}-[A-Z]{2}");
            if (!regex.IsMatch(id))
                return BadRequest("Id is incorrect");
            Vehicle vehicle = Vehicle.GetVehicleById(id);
            if (vehicle == null)
                return NotFound("Vehicle with this id is not found");
            return vehicle;
        }

        [HttpPost]
        public ActionResult<Vehicle> AddVehicle([FromBody] VenicleServer Newvehicle)
        {
            if (Newvehicle.Id == null || Newvehicle.VehicleType == 0 || Newvehicle.Balance == 0)
                return BadRequest("No one or several needed parameters");
            Vehicle vehicle;
            try
            {
                vehicle = new Vehicle(Newvehicle.Id, Newvehicle.VehicleType, Newvehicle.Balance);
                Service.AddVehicle(vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created("./CoolParking/Models/Parking.Vehicles", vehicle);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteVehicles(string id)
        {
            Regex regex = new Regex(@"[A-Z]{2}-\d{4}-[A-Z]{2}");
            if (!regex.IsMatch(id))
                return BadRequest("This id is incorrect");
            Vehicle vehicle = Vehicle.GetVehicleById(id);
            if (vehicle == null)
                return NotFound("Vehicle with this id is not found");
            Service.RemoveVehicle(id);
            return NoContent();
        }
    }
}

