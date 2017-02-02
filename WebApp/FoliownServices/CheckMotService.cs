using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;

namespace FoliownServices
{
    public class CheckMotService : IEnquiryService
    {
        private string _motServiceUrl = "https://www.check-mot.service.gov.uk";
        private const string VehicleMake = "Vehicle make";
        private const string VehicleModel = "Vehicle model";
        private const string DateFirstUsed  = "Date first used";
        private const string FuelType = "Fuel type";
        private const string VehicleColour = "Colour";
        private const string TestDate = "Test date";
        private const string ExpiryDate = "Expiry date";
        private const string TestResult = "Test Result";
        private const string Odometer = "Odometer reading";
        private const string MotNumber = "MOT test number";
        private const string AdvisoryNotice = "Advisory notice item(s)";
        private const string FailureReasons = "Reason(s) for failure";



        public async Task<string> GetVrmDetails(string vrm, string manufacturer)
        {
            using (var client = new HttpClient())
            {
                var formDataDict = new Dictionary<string, string>
                {
                    {"registration", vrm},
                    {"manufacturer", manufacturer}
                };

                var formData = new FormUrlEncodedContent(formDataDict);

                var response = await client.PostAsync(_motServiceUrl, formData);

                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
        }

        public async Task<VesVehicleDetails> ParseResponse(string response)
        {
            var parser = new HtmlParser();

            var document = await parser.ParseAsync(response);

            var vehicle = new VesVehicleDetails
            {
                QueryDateTime = DateTimeOffset.Now,
                MotTestResults = new List<MotTestResult>()
            };

            vehicle.HasFailedMotLookup = document.All.Any(t => t.TextContent.Contains("No vehicle that matched the data you entered could be found."));

            if (vehicle.HasFailedMotLookup)
                return vehicle;


            vehicle.VRM = document.All.FirstOrDefault(t => t.ClassName == "registrationNumber").TextContent.Trim();

            var spans = document.GetElementsByTagName("span");

            vehicle.Manufacturer = spans.FirstOrDefault(t => t.TextContent.Trim() == VehicleMake).NextElementSibling.TextContent;

            vehicle.Model = spans.FirstOrDefault(t => t.TextContent.Trim() == VehicleModel).NextElementSibling.TextContent;

            var tmpFirstUsed = DateTimeOffset.MaxValue;

            DateTimeOffset.TryParse(spans.FirstOrDefault(t => t.TextContent.Trim() == DateFirstUsed).NextElementSibling.TextContent, out tmpFirstUsed);

            vehicle.FirstUsedDate = tmpFirstUsed;

            vehicle.FuelType = spans.FirstOrDefault(t => t.TextContent.Trim() == FuelType).NextElementSibling.TextContent;

            vehicle.VehicleColour = spans.FirstOrDefault(t => t.TextContent.Trim() == VehicleColour).NextElementSibling.TextContent;

            var testResultsHtml = document.GetElementsByClassName("ul-data testresult");

            foreach (var testHtml in testResultsHtml)
            {

                var testResult = new MotTestResult();

                var motSpans = testHtml.GetElementsByTagName("span");

                var tmpTestDate = DateTimeOffset.MinValue;
                var tmpExpiryDate = DateTimeOffset.MinValue;

                DateTimeOffset.TryParse(motSpans.FirstOrDefault(t => t.TextContent.Trim() == TestDate)?.NextElementSibling.TextContent,
                        out tmpTestDate);

                testResult.TestDate = tmpTestDate;

                DateTimeOffset.TryParse(motSpans.FirstOrDefault(t => t.TextContent.Trim() == ExpiryDate)?.NextElementSibling.TextContent,
                        out tmpExpiryDate);

                testResult.ExpiryDate = tmpExpiryDate;

                var resultTest =
                    motSpans.FirstOrDefault(t => t.TextContent.Trim() == TestResult)?.NextElementSibling.TextContent;

                testResult.TestResult = resultTest == "Pass";

                testResult.Odometer = motSpans.FirstOrDefault(t => t.TextContent.Trim() == Odometer)?.NextElementSibling.TextContent;
                
                testResult.TestNumber = motSpans.FirstOrDefault(t => t.TextContent.Trim() == MotNumber)?.NextElementSibling.TextContent;

                //advisory notes
                testResult.Advisories = new List<string>();
                testHtml.GetElementsByClassName("group advice-comment")?.ToList().ForEach(t => testResult.Advisories.Add(t.TextContent));

                //failure notes
                testResult.Failures = new List<string>();
                testHtml.GetElementsByClassName("group failure-comment")?.ToList().ForEach(t => testResult.Failures.Add(t.TextContent));

                vehicle.MotTestResults.Add(testResult);



            }

            return vehicle;
        }
    }
}