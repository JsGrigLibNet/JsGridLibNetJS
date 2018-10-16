namespace JsGridLib.Models
{
    using System.Collections.Generic;

    public class JsGridStorageStatistics<T>
    {
        public long Total { set; get; }

        public IEnumerable<T> Results { set; get; }
    }
}