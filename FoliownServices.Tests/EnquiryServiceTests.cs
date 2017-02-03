using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace FoliownServices.Tests
{
    public class EnquiryServiceTests
    {
        private const string TestVrm = "sg08 bbs";
        private const string TestManufacturer = "MERCEDES";

        [Fact(Skip="Don't want to hit API all the time")]
        public void Can_Get_Enquiry_Details()
        {
            var service = new UkVehicleEnquiryService();

            var mercDetails = service.GetVrmDetails(TestVrm, TestManufacturer);
            
            var logPath = Path.GetTempFileName();
            using (var writer = File.CreateText(logPath))
            {
                writer.WriteLine(mercDetails.Result); //or .Write(), if you wish

                Console.WriteLine($"Written to {logPath}");
            }

        }

        public void Can_Get_Mot_Details()
        {
            
        }

        [Fact]
        public void Can_Parse_Expired_Car()
        {
            var service = new UkVehicleEnquiryService();

            var assembly = typeof(EnquiryServiceTests).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream("FoliownServices.Tests.m40saxResult.html");

            var testResponse = new StreamReader(resourceStream).ReadToEnd();
            
            var vehicleDetails = service.ParseResponse(testResponse);

            var result = vehicleDetails.Result;

            Assert.True(result.VRM == "M40 SAX");
            Assert.True(result.Manufacturer == TestManufacturer);
            Assert.True(result.FirstRegisrationDate == "June 1995");
            Assert.True(result.YearOfManufactureDate == "1995");
            Assert.True(result.CylinderCapacity == "2799 cc");
            Assert.True(result.FuelType == "PETROL");
            Assert.True(result.ExportMarker == "Yes");
            Assert.True(result.VehicleStatus == "Not taxed");
            Assert.True(result.VehicleColour == "GREEN");
            Assert.True(result.TypeApproval == "Not available");
            Assert.True(result.WheelPlan == "2 AXLE RIGID BODY");
            Assert.True(result.RevenueWeight == "Not available");

            Assert.True(result.TaxDueDate == DateTimeOffset.Parse("01 October 2011"));
            Assert.True(result.MotExpiryDate == DateTimeOffset.Parse("20 August 2011"));

        }

        [Fact]
        public void Can_Parse_Mot_History()
        {
            var service = new CheckMotService();

            var assembly = typeof(EnquiryServiceTests).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream("FoliownServices.Tests.SG08BBS.Mot.Result.html");

            var testResponse = new StreamReader(resourceStream).ReadToEnd();

            var vehicleDetails = service.ParseResponse(testResponse);

            var result = vehicleDetails.Result;

            Assert.False(result.HasFailedMotLookup);
            Assert.True(result.VRM == "SG08BBS");
            Assert.True(result.Manufacturer == TestManufacturer);
            Assert.True(result.MotTestResults.First().Advisories.Count == 1);
            Assert.True(result.MotTestResults.Skip(4).First().Failures.Count == 3);
            Assert.True(result.MotTestResults.Last().ExpiryDate.Year == 2012);

        }

        [Fact]
        public void Can_Parse_Lookup_Failure()
        {
            var service = new CheckMotService();

            var assembly = typeof(EnquiryServiceTests).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream("FoliownServices.Tests.NoMotFound.html");

            var testResponse = new StreamReader(resourceStream).ReadToEnd();

            var vehicleDetails = service.ParseResponse(testResponse);

            var result = vehicleDetails.Result;

            Assert.True(result.HasFailedMotLookup);
        }

        [Fact]
        public void Can_Parse_Valid_Car()
        {
            var service = new UkVehicleEnquiryService();

            var assembly = typeof(EnquiryServiceTests).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream("FoliownServices.Tests.SG08BBSResult.html");

            var testResponse = new StreamReader(resourceStream).ReadToEnd();

            var vehicleDetails = service.ParseResponse(testResponse);

            var result = vehicleDetails.Result;


            Assert.False(result.HasFailedLookup);
            Assert.True(result.VRM == "SG08 BBS");
            Assert.True(result.Manufacturer == TestManufacturer);
            Assert.True(result.FirstRegisrationDate == "May 2008");
            Assert.True(result.YearOfManufactureDate == "2008");
            Assert.True(result.CylinderCapacity == "2987 cc");
            Assert.True(result.FuelType == "DIESEL");
            Assert.True(result.ExportMarker == "No");
            Assert.True(result.VehicleStatus == "Tax not due");
            Assert.True(result.VehicleColour == "BLUE");
            Assert.True(result.TypeApproval == "M1");
            Assert.True(result.WheelPlan == "2 AXLE RIGID BODY");
            Assert.True(result.RevenueWeight == "2280kg");
            Assert.True(result.Co2Emmisions == "200 g/km");

            Assert.True(result.TaxDueDate == DateTimeOffset.Parse("01 December 2017"));
            Assert.True(result.MotExpiryDate == DateTimeOffset.Parse("15 April 2017"));

        }

        public void Can_Parse_Failure_Message()
        {
            var service = new UkVehicleEnquiryService();

            var assembly = typeof(EnquiryServiceTests).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream("FoliownServices.Tests.SG08BBSResult.html");

            var testResponse = new StreamReader(resourceStream).ReadToEnd();

            var vehicleDetails = service.ParseResponse(testResponse);

            var result = vehicleDetails.Result;


            Assert.True(result.HasFailedLookup);
        }

        [Fact]
        public void Can_Merge_Lookup_From_Both_Services()
        {
            var govt = new GovtVehicleLookupService(TestManufacturer, TestVrm);

            var result = govt.GetVesAndMotDetails().Result;

            Assert.True(result.Model == "CLS 320");
            Assert.False(result.HasFailedMotLookup);
            Assert.True(result.VRM == "SG08BBS");
            Assert.True(result.Manufacturer == TestManufacturer);
            Assert.True(result.MotTestResults.First().Advisories.Count == 1);
            Assert.True(result.MotTestResults.Skip(4).First().Failures.Count == 3);
            Assert.True(result.MotTestResults.Last().ExpiryDate.Year == 2012);
        }


    }
}
