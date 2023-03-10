using Foliown.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foliown.Web.MetaServices.Models
{
    public interface IVesVehicleDetailsRepository
    {
        VesVehicleDetails Find(string manufacturer, string vrm);
    }

    public interface IManufacturersRepository
    {
        IEnumerable<string> GetAll();

        IEnumerable<string> GetMajorManufacturers();
    }

    public interface IImageSearchRepository
    {
        string GetImage(string searchQuery);
    }
}
