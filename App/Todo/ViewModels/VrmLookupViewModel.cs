using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.ViewModels
{
    public class VrmLookupViewModel
    {
        public int EstimateCurrentOdometer(IEnumerable<OdometerHistory> odoHistory, DateTimeOffset projectionDate)
        {
            var odometerHistories = odoHistory as IList<OdometerHistory> ?? odoHistory.ToList();

            var orderedOdo = odometerHistories.OrderByDescending(t => t.OdoInt);

            var averageOdoIncrement = orderedOdo.Zip(orderedOdo.Skip(1), (y, x) => y.OdoInt - x.OdoInt).Average();
            
            var timeSpanSinceLastMot = TimeSpan.FromTicks(projectionDate.Ticks - odometerHistories.OrderByDescending(t => t.MotDate).First().MotDate.Ticks);
            
            var milesPerDay = averageOdoIncrement / 365;

            var expectedMiles = (TimeSpan.FromTicks(timeSpanSinceLastMot.Ticks).Days * milesPerDay) + orderedOdo.First().OdoInt;

            return (int) expectedMiles;
        }

        
        
    }
    public class OdometerHistory
    {
        public DateTimeOffset MotDate { get; set; }
        public string OdometerReading { get; set; }

        public int OdoInt => int.Parse(Helpers.OdoStringOnlyDigits(OdometerReading));
    }
}
