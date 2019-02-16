namespace JsGridLib.Contracts
{
    using System;
    using System.Collections.Generic;
    using JsGridLib.Models;

    public interface IJsGridStorage
    {
     JsGridStorageStatistics LoadAllTop(string storageAccess,
            int take = 100,
            dynamic sampleForFilter = null,
            Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> clientSideFiltering = null,
            int skip = 0);
        JsGridStorageStatistics LoadAll(string storageAccess,
            dynamic sampleForFilter = null,
            Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> clientSideFiltering = null);
         dynamic LoadById(string storageAccess, string id);

         void Update(string storageAccess, IDictionary<string, object> client);

        void Save(string storageAccess, IDictionary<string, object> client);

        void Delete(string storageAccess, string id);

        object GetDataSampleForSchemaByDataAccess(string dataaccess);
    }
}