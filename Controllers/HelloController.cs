using Microsoft.AspNetCore.Mvc;

namespace RESTful.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        public HelloController()
        {
        }

        [Produces("text/plain")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Witaj JAX-RS");
        }

        [Produces("text/plain")]
        [HttpGet("Echo")]
        public IActionResult Echo()
        {
            return Ok("Witaj Echo");
        }

        [Produces("text/plain")]
        [HttpGet("Echo2/{parameter}")]
        public IActionResult Echo2(string parameter)
        {
            return Ok($"Witaj Echo: {parameter}");
        }
    }
}