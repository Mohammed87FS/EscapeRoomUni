using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace libs
{
    public class Dialogue
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<Response> Responses { get; set; }
    }

    public class Response
    {
        public string Text { get; set; }
        public int NextId { get; set; }
    }

    
}
