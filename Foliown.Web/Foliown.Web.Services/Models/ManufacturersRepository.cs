using System.Collections.Generic;
using Foliown.GovtVehicleServices;

namespace Foliown.Web.Services.Models
{
    public class ManufacturersRepository : IManufacturersRepository {
        public IEnumerable<string> GetAll()
        {
            return new FormHelpers().GetSupportedManufacturers();
        }
    }
}