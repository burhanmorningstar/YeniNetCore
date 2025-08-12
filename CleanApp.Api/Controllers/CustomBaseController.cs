using App.Application;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ApplicationResult<T> result)
        {
            return result.Status switch
            {
                HttpStatusCode.NoContent => NoContent(),
                HttpStatusCode.Created => Created(result.UrlAsCreated, result),
                _ => new ObjectResult(result) { StatusCode = (int)result.Status }
            };
        }

        [NonAction]
        public IActionResult CreateActionResult(ApplicationResult result)
        {
            return result.Status switch
            {
                HttpStatusCode.NoContent => new ObjectResult(null) { StatusCode = (int)result.Status },
                _ => new ObjectResult(result) { StatusCode = (int)result.Status }
            };
        }
    }
}
