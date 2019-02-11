namespace JsGridLib.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class JsGridField
    {
        public bool displayAsCheckBox;
        public object format;
        public bool visible;

        public bool isPrimaryKey { set; get; }

        public string field { get; set; }

        public string editType { get; set; }

        public int? width { get; set; }

        [JsonIgnore]
        public string ValueField { get; set; }

        [JsonIgnore]
        public string TextField { get; set; }

        [JsonIgnore]
        public string Title { get; set; }

        [JsonIgnore]
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