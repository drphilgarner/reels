using System;
using System.Collections.Generic;
using System.Linq;

namespace Foliown.Core
{
    public class MotTestResult
    {
        public DateTimeOffset TestDate { get; set; }

        public DateTimeOffset ExpiryDate { get; set; }

        public bool TestResult { get; set; }

        public string Odometer { get; set; }

        public string TestNumber { get; set; }

        public List<string> Advisories { get; set; }

        public bool HasAdvisories => Advisories?.Any() ?? false;

        public string OdometerUnits
        {
            get
            {
                if (Odometer == null)
                    return null;

                return string.Join("",Odometer.Where(char.IsLetter));
            } 
        }

        public int OdometerReading
        {
            get
            {
                if (Odometer == null)
                    return 0;

                var numeric = string.Join("", Odometer.Where(char.IsDigit));

                int outVal = 0;
                int.TryParse(numeric, out outVal);

                return outVal;

            }
        }

        public List<string> Failures { get; set; }

        public bool HasFailures => Failures?.Any() ?? false;

    }
}