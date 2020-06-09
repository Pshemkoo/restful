using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RESTful.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [TypeFilter(typeof(CustomFilter))]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService messageService;
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;

        public MessagesController(IMessageService messageService, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            this.messageService = messageService;
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccessor = actionContextAccessor;
        }

        [HttpGet(Name = nameof(GetAll))]
        public IActionResult GetAll()
        {
            return Ok(messageService.GetAll().Select(m => CreateLinks(m)));
        }

        [HttpGet("{messageId}", Name = nameof(Get))]
        public IActionResult Get(long messageId)
        {
            return Ok(CreateLinks(messageService.Get(messageId)));
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
        
        [HttpPost(Name = nameof(Post))]
        public IActionResult Post([FromBody]Message message)
        {
            var mess = messageService.CreateMessage(message);

            message = CreateLinks(message);

            Response.Headers.Add("Location", message.Links.FirstOrDefault(y => y.Rel == "get").Href);

            return Ok();
        }

        [HttpPut("{messageId}")]
        public IActionResult Put(long messageId, [FromBody]Message message)
        {
            return Ok(CreateLinks(messageService.UpdateMessage(messageId, message)));
        }

        [HttpDelete("{messageId}", Name = nameof(Delete))]
        public IActionResult Delete(long messageId)
        {
            messageService.DeleteMessage(messageId);

            return Ok(NoContent());
        }

        private Message CreateLinks(Message ob)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            var idObj = new { messageId = ob.Id };

            ob.Links = new List<LinkDTO>();

            ob.Links.Add(
                new LinkDTO(urlHelper.Link(nameof(this.GetAll), null),
                "all",
                "GET"));

            ob.Links.Add(
                new LinkDTO(urlHelper.Link(nameof(this.Get), idObj),
                "get",
                "GET"));

            ob.Links.Add(
                new LinkDTO(urlHelper.Link(nameof(this.Delete), idObj),
                "delete",
                "DELETE"));

            ob.Links.Add(
                new LinkDTO(urlHelper.Link(nameof(this.Post), null),
                "create",
                "POST"));

            return ob;
        }
    }

    public class SearchParam
    {
        public string Author { get; set; }
        public string Content { get; set; }
    }

    public class LinkDTO
    {
        public string Href { get; private set; }
        public string Rel { get; private set; }
        public string Method { get; private set; }
        public LinkDTO(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
    public class CustomFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Add("mojNaglowek", "rsi test");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                token = token.ToString().Substring("Basic ".Length).Trim();
                System.Console.WriteLine(token);
                var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var credentials = credentialstring.Split(':');
                if (credentials[0] == "admin" && credentials[1] == "admin")
                {
                    var claims = new[] { new Claim("name", credentials[0]), new Claim(ClaimTypes.Role, "Admin") };
                    var identity = new ClaimsIdentity(claims, "Basic");
                }
            }
        }
    }
}