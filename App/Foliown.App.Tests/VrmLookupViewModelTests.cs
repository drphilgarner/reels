using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.ViewModels;
using Xunit;
using Shouldly;

namespace Foliown.App.Tests
{
    public class VrmLookupViewModelTests
    {
        [Fact]
        public void Can_Estimate_Current_Odometer_From_Mot_History()
        {
            var vrmLookupVm = new VrmLookupViewModel();

            var motHistory = new List<OdometerHistory>
            {
                new OdometerHistory {OdometerReading = "30,000 miles", MotDate = DateTimeOffset.Parse("2015-01-01")},
                new OdometerHistory {OdometerReading = "20,000 miles", MotDate = DateTimeOffset.Parse("2014-01-01")},
                new OdometerHistory {OdometerReading = "10,000 miles", MotDate = DateTimeOffset.Parse("2013-01-01")},

            };

            vrmLookupVm.EstimateCurrentOdometer(motHistory, DateTimeOffset.Parse("2016-01-01")).ShouldBe(40000);

        }
        
    }
}
