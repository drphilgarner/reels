using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;

namespace FoliownServices
{
    public class UkVehicleEnquiryService
    {
        private const string VehicleEnquiryServiceUrl = "https://vehicleenquiry.service.gov.uk/ViewVehicle";
        const string VehicleMake = "Vehicle make";
        private const string DateOfFirstReg = "Date of first registration";
        private const string YearOfManufacture = "Year of manufacture";
        private const string CylinderCapacity = "Cylinder capacity (cc)";
        private const string FuelType = "Fuel type";
        private const string ExportMarker = "Export marker";
        private const string VehicleStatus = "Vehicle status";
        private const string VehicleColour = "Vehicle colour";
        private const string TypeApproval = "Vehicle type approval";
        private const string Wheelplan = "Wheelplan";
        private const string RevWeight = "Revenue weight";
        private const string TaxDue = "Tax due:";
        private const string Expired = "Expired";
        private const string Expires = "Expires";

        public async Task<string> GetVrmDetails(string vrm, string manufacturer)
        {
            using (var client = new HttpClient())
            {
                var formDataDict = new Dictionary<string, string>
                {
                    {"Vrm", vrm},
                    {"Make", manufacturer}
                };

                var formData = new FormUrlEncodedContent(formDataDict);

                var response = await client.PostAsync(VehicleEnquiryServiceUrl, formData);

                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
        }

        public async Task<VehicleDetails> ParseResponse(string response)
        {
            var parser = new HtmlParser();

            var document = await parser.ParseAsync(response);

            var vehicle = new VehicleDetails();

            vehicle.VRM = document.All.FirstOrDefault(t => t.ClassName == "registrationNumber").TextContent;

            var spans = document.GetElementsByTagName("span");


            vehicle.Manufacturer =
                spans.FirstOrDefault(t => t.TextContent.Trim() == VehicleMake).NextElementSibling.TextContent;

            vehicle.FirstRegisrationDate =
                spans.FirstOrDefault(t => t.TextContent.Trim() == DateOfFirstReg)
                    .NextElementSibling.TextContent;

            vehicle.YearOfManufactureDate =
                spans.FirstOrDefault(t => t.TextContent.Trim() == YearOfManufacture).NextElementSibling.TextContent;

            vehicle.CylinderCapacity =
                spans.FirstOrDefault(t => t.TextContent.Trim() == CylinderCapacity).NextElementSibling.TextContent;

            vehicle.FuelType =
                spans.FirstOrDefault(t => t.TextContent.Trim() == FuelType).NextElementSibling.TextContent;
            
            vehicle.ExportMarker =
                spans.FirstOrDefault(t => t.TextContent.Trim() == ExportMarker).NextElementSibling.TextContent;

            vehicle.VehicleStatus =
                spans.FirstOrDefault(t => t.TextContent.Trim() == VehicleStatus).NextElementSibling.TextContent;
            
            vehicle.VehicleColour =
                spans.FirstOrDefault(t => t.TextContent.Trim() == VehicleColour).NextElementSibling.TextContent;

            vehicle.TypeApproval =
                spans.FirstOrDefault(t => t.TextContent.Trim() == TypeApproval).NextElementSibling.TextContent;

            vehicle.WheelPlan = 
                spans.FirstOrDefault(t => t.TextContent.Trim() == Wheelplan).NextElementSibling.TextContent;

            vehicle.RevenueWeight =
                spans.FirstOrDefault(t => t.TextContent.Trim() == RevWeight).NextElementSibling.TextContent;

            vehicle.Co2Emmisions = document.GetElementById("CO2EmissionShown")?.TextContent;

            //due dates

            var paras = document.GetElementsByTagName("p");
            
            var taxDueContent = paras.FirstOrDefault(t => t.TextContent.Contains(TaxDue))?.TextContent;

            if (taxDueContent != null)
            {
                var taxDue = DateTimeOffset.MinValue;

                if (DateTimeOffset.TryParse(
                    taxDueContent.Replace(TaxDue, ""),
                    out taxDue))
                {
                    vehicle.TaxDueDate = taxDue;
                }
            }

            var motDueContent = paras.FirstOrDefault(t => t.TextContent.Contains(Expired))?.TextContent;

            motDueContent = motDueContent ?? paras.FirstOrDefault(t => t.TextContent.Contains(Expires))?.TextContent;
            

            if (motDueContent != null)
            {
                var motDue = DateTimeOffset.MinValue;
                var motStr = motDueContent.Replace(Expired, "");

                motStr = motStr.Replace(Expires, "");

                motStr = motStr.Replace("\"E\"", "");
                motStr = motStr.Replace("\n", "");
                motStr = motStr.Replace(":", "");

                if (DateTimeOffset.TryParse(motStr.Trim(), out motDue))
                {
                    vehicle.MotExpiryDate = motDue;
                }
            }


            return vehicle;
        }

        
    }
}
