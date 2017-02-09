using Foliown.Web.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foliown.Web.Services.Controllers
{
    [Route("api/[controller]")]
    public class ImageSearchController : Controller
    {
        public IImageSearchRepository ImageSearchRepo { get; set; }

        public ImageSearchController(IImageSearchRepository imageSearchRepo)
        {
            ImageSearchRepo = imageSearchRepo;
        }

        [HttpGet("Thumb/{searchQuery}", Name ="GetThumbnailFromQuery")]
        public IActionResult GetImageThumbnail(string searchQuery)
        {
            var thumb = ImageSearchRepo.GetImage(searchQuery);

            return new ObjectResult(thumb);
        }

    }
}