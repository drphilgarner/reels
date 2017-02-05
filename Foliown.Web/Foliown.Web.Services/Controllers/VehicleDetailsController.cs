using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Foliown.Web.Services.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Foliown.Web.Services.Controllers
{
    [Route("api/[controller]")]
    public class VehicleDetailsController : Controller
    {
        public IVesVehicleDetailsRepository VesVehicleDetailsRepo { get; set; }
        public IManufacturersRepository ManufacturerRepo { get; set; }
        // GET: api/values
        public VehicleDetailsController(IVesVehicleDetailsRepository vesVehicleDetailsRepo, IManufacturersRepository manufacturerRepo)
        {
            VesVehicleDetailsRepo = vesVehicleDetailsRepo;
            ManufacturerRepo = manufacturerRepo;
        }

        [HttpPost]
        public IActionResult VehicleDetails([FromBody] dynamic data)
        {

            string manufacturer = data.manufacturer;
            string vrm = data.vrm;

            if (manufacturer == null || vrm == null) return BadRequest();

            var supportedManufacturers = ManufacturerRepo.GetAll();

            if (!supportedManufacturers.Contains(manufacturer.ToUpper()))
            {
                return BadRequest();
            }

            const string pattern = "(?<Current>^[A-Z]{2}[0-9]{2}[A-Z]{3}$)|(?<Prefix>^[A-Z][0-9]{1,3}[A-Z]{3}$)|(?<Suffix>^[A-Z]{3}[0-9]{1,3}[A-Z]$)|(?<DatelessLongNumberPrefix>^[0-9]{1,4}[A-Z]{1,2}$)|(?<DatelessShortNumberPrefix>^[0-9]{1,3}[A-Z]{1,3}$)|(?<DatelessLongNumberSuffix>^[A-Z]{1,2}[0-9]{1,4}$)|(?<DatelessShortNumberSufix>^[A-Z]{1,3}[0-9]{1,3}$)";

            var rgx = new Regex(pattern);

            var vrmNoSpace = vrm.Replace(" ", "");

            if (rgx.IsMatch(vrmNoSpace.ToUpper()))
            {
                var details = VesVehicleDetailsRepo.Find(manufacturer, vrmNoSpace);

                return new ObjectResult(details);
            }

            return BadRequest();
        }
    }
}
