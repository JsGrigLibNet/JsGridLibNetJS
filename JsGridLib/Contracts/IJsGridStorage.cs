namespace JsGridLib.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http.OData.Query;
    using JsGridLib.Models;


    public interface IGridRequestOptions
    {
        bool IsAuthorized(HttpRequestMessage request);
        IJsGridStorage storage { set; get; }
    }

    public interface IJsGridStorage
    {
     JsGridStorageStatistics LoadAllTop(
         string storageAccess,
         int take = 100,
         dynamic sampleForFilter = null,
         ODataQueryOptions clientSideFiltering = null,
         int skip = 0);
        JsGridStorageStatistics LoadAll(
            string storageAccess,
            dynamic sampleForFilter = null,
            ODataQueryOptions clientSideFiltering = null);
         dynamic LoadById(string storageAccess, string id);

         void Update(string storageAccess, IDictionary<string, object> client);

        void Save(string storageAccess, IDictionary<string, object> client);

        void Delete(string storageAccess, string id);

        object GetDataSampleForSchemaByDataAccess(string dataaccess);

        object GetReplacementDataSampleForSchemaByDataAccess(string dataaccess);
    }
}