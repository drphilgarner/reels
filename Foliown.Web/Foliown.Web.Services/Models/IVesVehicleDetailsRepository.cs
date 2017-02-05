using Foliown.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foliown.GovtVehicleServices;

namespace Foliown.Web.Services.Models
{
    public interface IVesVehicleDetailsRepository
    {
        VesVehicleDetails Find(string manufacturer, string vrm);
    }

    public interface IManufacturersRepository
    {
        IEnumerable<string> GetAll();
    }

    public class ManufacturersRepository : IManufacturersRepository {
        public IEnumerable<string> GetAll()
        {
            return new FormHelpers().GetSupportedManufacturers();
        }
    }

    public class VehicleDetailsRepository : IVesVehicleDetailsRepository
    {

        public VesVehicleDetails Find(string manufacturer, string vrm)
        {
            //TODO exception handling
            var vehicleLookupService = new GovtVehicleLookupService(manufacturer, vrm);

            return vehicleLookupService.GetVesAndMotDetails().Result;
        }
    }

    
}
