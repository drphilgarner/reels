using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FoliownServices
{
    public class VesVehicleDetails
    {
        public string Manufacturer { get; set; }

        public string  Model { get; set; }

        public string VRM { get; set; }

        public DateTimeOffset QueryDateTime { get; set; }
        

        public DateTimeOffset TaxDueDate { get; set; }

        public DateTimeOffset MotExpiryDate { get; set; }

        public DateTimeOffset FirstUsedDate { get; set; }

        public string FirstRegisrationDate { get; set; }

        public string YearOfManufactureDate { get; set; }

        public string CylinderCapacity { get; set; }

        public string Co2Emmisions { get; set; }

        public string ExportMarker { get; set; }

        public string VehicleStatus { get; set; }

        public string VehicleColour { get; set; }

        public string TypeApproval { get; set; }

        public string WheelPlan { get; set; }

        public string  RevenueWeight { get; set; }

        public string FuelType { get; set; }
        public bool HasFailedLookup { get; set; }

        public bool HasFailedMotLookup { get; set; }


        public List<MotTestResult> MotTestResults { get; set; }

        
    }
}