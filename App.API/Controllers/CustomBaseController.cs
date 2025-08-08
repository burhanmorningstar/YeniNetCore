using System.Net;
using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ServiceResult<T> result)
        {
            return result.Status switch
            {
                HttpStatusCode.NoContent => NoContent(),
                HttpStatusCode.Created => Created(result.UrlAsCreated, result),
                _ => new ObjectResult(result) { StatusCode = (int)result.Status }
            };
        }

        [NonAction]
        public IActionResult CreateActionResult(ServiceResult result)
        {
            return result.Status switch
            {
                HttpStatusCode.NoContent => new ObjectResult(null) { StatusCode = (int)result.Status },
                _ => new ObjectResult(result) { StatusCode = (int)result.Status }
            };
        }
    }
}
