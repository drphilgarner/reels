using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Foliown.SearchServices;
using Microsoft.AspNetCore.WebUtilities;


namespace Foliown.Web.Services.Models
{
    public class ImageSearchRepository : IImageSearchRepository
    {
        public string GetImage(string searchQuery)
        {
            //TODO: exception handling
            var bingApi = new BingSearchApi();
            
            return bingApi.GetImageThumbnail(searchQuery).Result;
            
        }
    }
}