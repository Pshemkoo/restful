using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace RESTful.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService messageService;

        public MessagesController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Ok(messageService.GetAll());
        }

        [HttpGet("{messageId}")]
        public IActionResult Get(long messageId)
        {
            return Ok(messageService.Get(messageId));
        }

        [HttpGet("Search/{startAt}")]
        public IActionResult Search(string startAt)
        {
            var messages = messageService.GetAll();

            return Ok(messages.Where(m => m.Content.Contains(startAt)).ToList());
        }

        [HttpGet("SearchByObject")]
        public IActionResult SearchByObject([FromBody]SearchParam searchParam)
        {
            var messages = messageService.GetAll();

            return Ok(messages.Where(m => m.Content.Contains(searchParam.Content) || m.Author == searchParam.Author).ToList());
        }
        
        [HttpPost("")]
        public IActionResult Post([FromBody]Message message)
        {
            return Ok(messageService.CreateMessage(message));
        }

        [HttpPut("{messageId}")]
        public IActionResult Put(long messageId, [FromBody]Message message)
        {
            return Ok(messageService.UpdateMessage(messageId, message));
        }

        [HttpDelete("{messageId}")]
        public IActionResult Delete(long messageId)
        {
            messageService.DeleteMessage(messageId);

            return Ok(NoContent());
        }
    }

    public class SearchParam
    {
        public string Author { get; set; }
        public string Content { get; set; }
    }
}