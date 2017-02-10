using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Foliown.GovtVehicleServices
{
    public class FormHelpers
    {
        public List<string> GetSupportedManufacturers()
        {
            var assembly = typeof(GovtVehicleLookupService).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream($"{typeof(FormHelpers).Namespace}.manufacturers.json");

            var manufacturersJson = new StreamReader(resourceStream).ReadToEnd();

            var formHints = JsonConvert.DeserializeObject<FormHints>(manufacturersJson);

            return formHints.Manufacturers;

        }

        public List<string> GetMajorManufacturers()
        {
            var assembly = typeof(GovtVehicleLookupService).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream($"{typeof(FormHelpers).Namespace}.major_manufacturers.json");

            var manufacturersJson = new StreamReader(resourceStream).ReadToEnd();

            var formHints = JsonConvert.DeserializeObject<FormHints>(manufacturersJson);

            return formHints.Manufacturers;
        }
    }
}