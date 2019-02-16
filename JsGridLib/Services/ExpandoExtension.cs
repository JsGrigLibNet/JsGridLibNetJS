namespace JsGridLib.Services
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using Newtonsoft.Json.Linq;

    public static class ExpandoExtension
    {
        public static ExpandoObject ToExpandoObject(this object obj)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            var tt = new List<PropertyDescriptor>();
            if (obj is JObject)
            {
                var obj1 = (JObject)obj;
                expando = obj1.ToObject<ExpandoObject>();
                //foreach (var x in obj1)
                //{
                //    string name = x.Key;
                //    JToken value = x.Value;
                //    expando.Add(name, value.ToObject());
                //}
            }
            else
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
                    expando.Add(property.Name, property.GetValue(obj));
            }

            return (ExpandoObject)expando;
        }
    }
}