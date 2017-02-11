using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public async Task<JObject> LookupVrmAsync(string vrm, string manufacturer)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(new {vrm, manufacturer});

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response =
                    await client.PostAsync(
                        $"{Variables.VehicleServices_LookupVRM_API}/{Variables.VehicleServices_LookupVRM_VehicleDetails}",
                        content);

                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    var responseJson = JObject.Parse(responseStr);

                    return responseJson;
                }
                return null;
            }
        }

        public async Task<JObject> LookupVRM(string vrm, string manufacturer)
        {

            //using (
            //    var client =
            //        new RestClient(
            //            $"{Variables.VehicleServices_LookupVRM_API}/{Variables.VehicleServices_LookupVRM_VehicleDetails}")
            //)
            //{
            //    var request = new RestRequest(Method.POST);
            //    request.AddHeader("cache-control", "no-cache");
            //    request.AddHeader("content-type", "application/json");
            //    request.AddParameter("application/json", $"{{\"{nameof(vrm)}\":\"{vrm}\",\"{nameof(manufacturer)}\":{manufacturer}}}", ParameterType.RequestBody);
            //    IRestResponse response = await client.Execute(request);

            //    if (response.IsSuccess)
            //    {
            //        var responseString = response.Content;

            //        var responseJson = JObject.Parse(responseString);

            //        return responseJson;

            //        //        var responseJson = JObject.Parse(responseString);

            //        //        return responseJson;
            //    }
            //    return null;
            //}

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var formDataDict = new Dictionary<string, string>
                {
                    {"Vrm", vrm},
                    {"Make", manufacturer}
                };

                var formData = new FormUrlEncodedContent(formDataDict);

                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"{Variables.VehicleServices_LookupVRM_API}/{Variables.VehicleServices_LookupVRM_VehicleDetails}")
                {
                    Content =
                        new StringContent($"{{\"{nameof(vrm)}\":\"{vrm}\",\"{nameof(manufacturer)}\":{manufacturer}}}",
                            Encoding.UTF8,
                            "application/json")
                };

                //var response =
                //    await client.PostAsync(
                //        $"{Variables.VehicleServices_LookupVRM_API}/{Variables.VehicleServices_LookupVRM_VehicleDetails}",
                //        formData);

                //request.Content = formData;

                //CONTENT-TYPE header

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {

                    var responseString = await response.Content.ReadAsStringAsync();

                    var responseJson = JObject.Parse(responseString);

                    return responseJson;
                }

                return null;
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

        public async Task<string> GetVehicleThumbnail(string selectedManufacturer, string model, string colour, string year)
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync(
                        $"{_baseUrl}/{Variables.VehicleServices_LookupVRM_ImageSearch_Thumb}/{selectedManufacturer}%20{model}%20{colour}%20{year}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                     return content;
                }
            }
            return null;
        }
    }



    
}
