namespace JsGridLib.Models
{
    using System;
    using System.Collections.Generic;

    public class SchemaSetUp
    {
        public List<dynamic> Fields { set; get; }

        public Dictionary<string, object> ClientColumnSetUp { get; set; }

        public Exception SchemaCreationException { get; set; }
    }
}