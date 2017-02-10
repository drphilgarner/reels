using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Todo.Models;

namespace Todo.ServiceClient
{
    public class RestVehicleServices
    {
        private string _baseUrl;
        private string _thumbnailPath;
        private string _vehicleDetailsPath;
        private string _manufacturersPath;
        private string _majorManufacturersPath;
        private string _hostUrl;
        private string _imagesPath;

        public RestVehicleServices()
        {
            _hostUrl = Variables.VehicleServices_hostUrl;
            _baseUrl = Variables.VehicleServices_LookupVRM_API;
            _thumbnailPath = Variables.VehicleServices_LookupVRM_ImageSearch_Thumb;
            _vehicleDetailsPath = Variables.VehicleServices_LookupVRM_VehicleDetails;
            _manufacturersPath = "Manufacturers";
            _majorManufacturersPath = "MajorManufacturers";
            _imagesPath = Variables.RestVehicleServices_RestVehicleServices_Images;

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

        public async Task<List<Manufacturer>> GetMajorManufacturers()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{_baseUrl}/{_vehicleDetailsPath}/{_majorManufacturersPath}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    Manufacturers = JsonConvert.DeserializeObject<List<string>>(content);
                }
            }

            var manuList = Manufacturers.Select(m => new Manufacturer {Name = m, LogoUri = $"{_hostUrl}/{_imagesPath}/{m}.png"}).ToList();

            return manuList;
        }
    }

    
}
