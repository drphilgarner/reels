using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo
{
    public static class Helpers
    {
        public static string EngineSizeCcToDecimalString(string engineSizeCc)
        {
            var justNumericEngine = new string(engineSizeCc.Cast<char>().Where(char.IsDigit).ToArray());

            double simpleEngineSize = 0;
            
            if (double.TryParse(justNumericEngine, out simpleEngineSize))
            {
                var round = Math.Round(simpleEngineSize / 1000, 2);
                return round.ToString("#.0");
            }

            return null;
        }

        public static string OdoStringOnlyDigits(string odoReading)
        {
            return new string(odoReading.Cast<char>().Where(char.IsDigit).ToArray());
        }
    }
}
