namespace JsGridLib.Contracts
{
    using System;
    using System.Collections.Generic;
    using JsGridLib.Models;

    public interface IJsGridStorage

    {
        JsGridStorageStatistics LoadAll(
            int take = 100,
            dynamic sampleForFilter = null,
            Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> clientSideFiltering = null,
            int skip = 0);

        dynamic LoadById(string id);

        void Update(IDictionary<string, object> client);

        void Save(IDictionary<string, object> client);

        void Delete(string id);
    }
}