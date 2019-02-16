namespace JsGridLib.Models
{
    using System.Collections.Generic;
    using JsGridLib.Services;

    public class JsGridObject<T>
    {
        public List<T> Data { get; set; }

        public List<dynamic> FieldsReadable { get; set; }

        //public List<dynamic> FieldsCreatable { get; set; }

        //public List<dynamic> FieldsUpdateable { get; set; }

        //public List<dynamic> FieldsDeletable { get; set; }

       // public JsGridTableSettings Settings { get; set; }
    }
}