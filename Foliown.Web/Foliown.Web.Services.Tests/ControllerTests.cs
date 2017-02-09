using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foliown.SearchServices;
using Xunit;

namespace Foliown.Web.Services.Tests
{

    public class ControllerTests
    {
        [Fact]
        public void Can_Get_Image_From_Bing_Search()
        {
            var bingApi = new BingSearchApi();

            var response = bingApi.GetImageThumbnail("2008 Mercedes CLS 320 CDI");

            Uri uriResult;
            bool result = Uri.TryCreate(response.Result, UriKind.Absolute, out uriResult);
            
            Assert.True(result);


        }
    }
}
