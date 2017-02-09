using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Foliown.SearchServices
{
    public class BingSearchApi
    {
        public async Task<string> GetImageThumbnail(string searchQuery)
        {

            var client = new HttpClient();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "a4a4434d77cd414cb6119fea5c8e3b4d");

            //var encodedSearchTerm = WebUtility.HtmlEncode(searchQuery);

            // Request parameters
            var queryDict = new Dictionary<string, string>
            {
                {"q", searchQuery},
                {"count", "1"},
                {"mkt", "en-gb"}
            };

            
            var uri = "https://api.cognitive.microsoft.com/bing/v5.0/images/search?";

            var goQuery = QueryHelpers.AddQueryString(uri, queryDict);

            var response = await client.GetAsync(goQuery);

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JObject.Parse(responseString);


            return (string)result["value"][0]["thumbnailUrl"];


        }
    }
}
