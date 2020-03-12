using AT.CustomExceptions;
using AT.Data.Models;
using AT.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AT.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[EnableCors("AllowAll")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService<Vehicle> _vehicleService;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(ILogger<VehicleController> logger, IVehicleService<Vehicle> vehicleService)
        {
            _vehicleService = vehicleService;
            _logger = logger;
        }

        // GET: vehicle
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var vehicles = _vehicleService.GetAll();
            
            if (vehicles == null)
            {
                var exception = new NotFoundCustomException("Not Found", "Records could not be retrieved from server", "Get");
                _logger.LogError($"{exception.Path},{exception.Description},{exception.Message}");
                return NotFound(exception);
            }
            
            return Ok(vehicles);
        }

        // GET: vehicle/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            Vehicle Vehicle = _vehicleService.Get(id);

            if (Vehicle == null)
            {
                return NotFound("The Vehicle record couldn't be found.");
            }

            return Ok(Vehicle);
        }

        // GET: vehicle/GetBySelectedInfo
        [HttpGet]
        [Route("GetBySelectedInfo")]
        public async Task<IActionResult> Get(string make, string model, string minprice, string maxprice, string bodyType, string insuranceGroup, string postcode, string distance)
        {
            var vehicle = new Vehicle()
            {
                Make = make,
                Model = model,
                MinPrice = minprice,
                MaxPrice = maxprice,
                BodyType = bodyType,
                InsuranceGroup = insuranceGroup,
                Distance = distance
            };

            var vehicles = await _vehicleService.GetVehiclesBySelectedInfo(vehicle, postcode);

            if (vehicles == null)
            {
                return NotFound("Vehicle records could not be found");
            }

            return Ok(vehicles);
        }

        //GET: vehicle/GetMakes
        [HttpGet]
        [Route("GetMakes")]
        public IActionResult GetMakes()
        {
            var vehicleMakes = _vehicleService.GetMakes();
            if (vehicleMakes == null)
            {
                return NotFound("Vehicle records could not be found");
            }

            return Ok(vehicleMakes);
        }

        //GET: vehicle/GetModelsByMake
        [HttpGet]
        [Route("GetModelsByMake")]
        public IActionResult GetModelsByMake(string make)
        {
            var vehicleModels = _vehicleService.GetModelsByMake(make);
            if (vehicleModels == null)
            {
                return NotFound("Vehicle records could not be found");
            }

            return Ok(vehicleModels);
        }

        [HttpGet]
        [Route("GetBodyTypes")]
        public IActionResult GetBodyTypes()
        {
            var bodyTypes = _vehicleService.GetBodyTypes();
            if (bodyTypes == null)
            {
                return NotFound("BodyType records could not be found");
            }

            return Ok(bodyTypes);
        }

        [HttpGet]
        [Route("GetInsuranceGroups")]
        public IActionResult GetInsuranceGroups()
        {
            var insuranceGroups = _vehicleService.GetInsuranceGroups();
            if (insuranceGroups == null)
            {
                return NotFound("Insurance Group records could not be found");
            }

            return Ok(insuranceGroups);
        }

        // POST: vehicle
        [HttpPost]
        public IActionResult Post([FromBody] Vehicle Vehicle)
        {
            if (Vehicle == null)
            {
                return BadRequest("Vehicle is null.");
            }

            _vehicleService.Add(Vehicle);
            return CreatedAtRoute(
                  "Get",
                  new { ID = Vehicle.VehicleID },
                  Vehicle);
        }

        // PUT: vehicle/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Vehicle Vehicle)
        {
            if (Vehicle == null)
            {
                return BadRequest("Vehicle is null.");
            }

            Vehicle VehicleToUpdate = _vehicleService.Get(id);
            if (VehicleToUpdate == null)
            {
                return NotFound("The Vehicle record couldn't be found.");
            }

            _vehicleService.Update(VehicleToUpdate, Vehicle);
            return NoContent();
        }

        // DELETE: vehicle/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Vehicle Vehicle = _vehicleService.Get(id);
            if (Vehicle == null)
            {
                return NotFound("The Vehicle record couldn't be found.");
            }

            _vehicleService.Delete(Vehicle);
            return NoContent();
        }
    }
}
