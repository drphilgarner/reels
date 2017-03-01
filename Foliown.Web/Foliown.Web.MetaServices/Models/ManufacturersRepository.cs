using System.Collections.Generic;
using Foliown.GovtVehicleServices;

namespace Foliown.Web.MetaServices.Models
{
    public class ManufacturersRepository : IManufacturersRepository {
        public IEnumerable<string> GetAll()
        {
            return new FormHelpers().GetSupportedManufacturers();
        }

        public IEnumerable<string> GetMajorManufacturers()
        {
            return new FormHelpers().GetMajorManufacturers();
        }
    }
}