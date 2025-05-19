using Microsoft.AspNetCore.Mvc;

namespace DotNest.Controllers.Shared
{
    public class StatusCodeController : Controller
    {
        
        [HttpGet("/StatusCode/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            switch (statusCode)
            {
                case 401:
                    return View("404");
                case 500:
                    return View("500");
                default:
                    return View("Default");
            }
        }
        
    }
}
