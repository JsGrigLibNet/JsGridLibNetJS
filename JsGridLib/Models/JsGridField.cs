namespace SampleHttpsServer
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using SampleHttpsServer.JsGridModule;

    public class JsGridField
    {
       
        public string Name { get; set; }

        public string Type { get; set; }

        public int? Width { get; set; }

        public string ValueField { get; set; }

        public string TextField { get; set; }

        public string Title { get; set; }

        public bool? Sorting { get; set; }
        
        [JsonIgnore]
        public JsGridObjectTypes ClientType { get; set; }
        [JsonIgnore]
        public string PropertyName { get; set; }
        [JsonIgnore]
        public Type OriginalType { get; set; }
        [JsonIgnore]
        public object PropertyValue { get; set; }
        [JsonIgnore]
        public List<string> DefaultValues { get; set; }
        [JsonIgnore]
        public string UIRoute { get; set; }

        public List<DropDownItem> Items { get; set; }
    }
}