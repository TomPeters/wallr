using System;
using System.Collections.Generic;

namespace SerializationModel
{
    public class Source
    {
        public string Type { get; set; }
        public Guid Id { get; set; }
        public IDictionary<string, string> Settings { get; set; } 
    }
}
