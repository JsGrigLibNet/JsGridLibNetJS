namespace JsGridLib.Models
{
    using System.Collections.Generic;

    public class JsGridStorageStatistics
    {
        public long Total { set; get; }

        public IEnumerable<object> Results { set; get; }
    }
}