using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_ReturnTypes_StatusCodes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        // This action only supports GET method
        [HttpGet]
        public IActionResult GetAction()
        {
            return Ok("GetAction Data processed");
        }
        // This action only supports POST method
        [HttpPost]
        public IActionResult PostAction()
        {
            return Ok("PostAction Data Processed");
        }
        // This action only supports PUT method
        [HttpPut]
        public IActionResult PutAction()
        {
            return Ok("PutAction Data processed");
        }
        // This action only supports PUT method
        [HttpPatch]
        public IActionResult PatchAction()
        {
            return Ok("PatchAction Data processed");
        }
        // This action only supports DELETE method
        [HttpDelete]
        public IActionResult DeleteAction()
        {
            return Ok("DeleteAction Data processed");
        }
    }
}
