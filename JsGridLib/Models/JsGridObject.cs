namespace SampleHttpsServer
{
    using System.Collections.Generic;

    public class JsGridObject<T>
    {
        public List<T> Data { get; set; }

        public List<dynamic> Fields { get; set; }

        public JsGridTableSettings Settings { get; set; }
    }
}