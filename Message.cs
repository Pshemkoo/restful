using RESTful.Controllers;
using System;
using System.Collections.Generic;

namespace RESTful
{
    public class Message
    {
        public long Id { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; }
        public IList<LinkDTO> Links { get; set; }
    }
}