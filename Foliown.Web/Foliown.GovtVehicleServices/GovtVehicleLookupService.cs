using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Foliown.GovtVehicleServices
{
    public class GovtVehicleLookupService
    {
        private readonly string _manufacturer;
        private readonly string _vrm;

        public GovtVehicleLookupService(string manufacturer, string vrm)
        {
            _manufacturer = manufacturer;
            _vrm = vrm;
        }

        public async Task<VesVehicleDetails> GetVesAndMotDetails()
        {
            var vesService = new UkVehicleEnquiryService();
            var motService = new UkCheckMotService();

            var vrmHtmlResult = vesService.GetVrmDetails(_vrm, _manufacturer);
            var motHtmlResult = motService.GetVrmDetails(_vrm, _manufacturer);

            
            var res = await Task.WhenAll(vrmHtmlResult, motHtmlResult).ContinueWith(t =>
            {
                var vrmResult = vesService.ParseResponse(vrmHtmlResult.Result).Result;
                var motResult = motService.ParseResponse(motHtmlResult.Result).Result;

                var mergedResult = Helpers.Merge<VesVehicleDetails>(vrmResult, motResult);
               
                return mergedResult;

            });

            return res;
        }

        public List<string> GetSupportedManufacturers()
        {
            var assembly = typeof(GovtVehicleLookupService).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream("manufacturers.json");

            var manufacturersJson = new StreamReader(resourceStream).ReadToEnd();

            var formHints = JsonConvert.DeserializeObject<FormHints>(manufacturersJson);

            return formHints.Manufacturers;

        }
    }

    public class FormHints
    {
        public List<string> Manufacturers { get; set; }
    }
}
