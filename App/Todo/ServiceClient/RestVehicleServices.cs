using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Todo.ServiceClient
{
    public class RestVehicleServices
    {
        private string _baseUrl;
        private string _thumbnailPath;
        private string _vehicleDetailsPath;
        private string _manufacturersPath;

        public RestVehicleServices()
        {
            _baseUrl = Variables.VehicleServices_LookupVRM_API;
            _thumbnailPath = Variables.VehicleServices_LookupVRM_ImageSearch_Thumb;
            _vehicleDetailsPath = Variables.VehicleServices_LookupVRM_VehicleDetails;
            _manufacturersPath = "Manufacturers";

        }

        public async void LookupVRM(string vrm)
        {
           
            using (var client = new HttpClient())
            {
                //var content = new StringContent()
                //client.PostAsync($"{_baseUrl}/{_vehicleDetailsPath}")
            }
        }

        public async Task<List<string>> GetManufacturers()
        {


            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{_baseUrl}/{_vehicleDetailsPath}/{_manufacturersPath}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    Manufacturers = JsonConvert.DeserializeObject<List<string>>(content);
                }
            }

            return Manufacturers;
        }

        public List<string> Manufacturers { get; set; }
    }
}
