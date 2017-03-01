using Foliown.Core;
using Foliown.GovtVehicleServices;

namespace Foliown.Web.MetaServices.Models
{
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