namespace JsGridLib.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Web;
    using System.Web.Http.OData.Query;
    using JsGridLib.Contracts;
    using JsGridLib.Models;

    public class JsGridDataService<TEntity>

    {
        readonly IJsGridStorage jsGridDataStorage;

        public JsGridDataService(
            IJsGridStorage jsGridDataStorage,
            bool includeIdField)
        {
            this.IncludeIdField = includeIdField;
            this.jsGridDataStorage = jsGridDataStorage;
        }

        bool IncludeIdField { get; }

        public JsGridStorageStatistics GetAll(HttpRequestMessage request,string dataAccess, ODataQueryOptions opt)
        {
            var sample = this.GetFilterInstance<TEntity>(HttpUtility.ParseQueryString(request.RequestUri.Query));

            JsGridStorageStatistics result = this.jsGridDataStorage.LoadAll(dataAccess,sample, opt);
            return new JsGridStorageStatistics
            {
                Total = result.Total,
                Results = result.Results.Select(x => (object)((TEntity)x))
            };
        }

        public JsGridStorageStatistics GetAllTop(HttpRequestMessage request, string dataAccess, ODataQueryOptions opt, int take, int skip)
        {
            var sample = this.GetFilterInstance<TEntity>(HttpUtility.ParseQueryString(request.RequestUri.Query));

            JsGridStorageStatistics result = this.jsGridDataStorage.LoadAllTop(dataAccess, take,  (sample), opt, skip);
            return new JsGridStorageStatistics
            {
                Total = result.Total,
                Results = result.Results.Select(x => (object)((TEntity)x))
            };
        }

        public TEntity GetById(HttpRequestMessage request, string dataAccess,  string id)
        {
            return this.jsGridDataStorage.LoadById(dataAccess, id);
        }

        public T GetFilterInstance<T>(NameValueCollection fil)

        {
            var filterObj = Activator.CreateInstance<T>();
            if (fil == null || fil.Count == 0)
            {
                string[] keys = fil.AllKeys;

                foreach (string o in keys)
                {
                    Type type = typeof(T);

                    PropertyInfo prop = type.GetProperty(o);

                    prop.SetValue(filterObj, fil[o], null);
                }
            }

            return filterObj;
        }

        public JsGridObject<TEntity> GetSchemaAndSettings(object sample = null)
        {
            var res = new JsGridObject<TEntity>
            {
                FieldsReadable = new SchemaFactory().SchemaFromType<TEntity>(this.IncludeIdField, sample).Fields,
                //FieldsUpdateable = new SchemaFactory().SchemaFromType<TEntity>(this.IncludeIdField, sample).Fields,
                //FieldsDeletable = new SchemaFactory().SchemaFromType<TDeletable>(this.IncludeIdField, sample).Fields,
                //FieldsCreatable = new SchemaFactory().SchemaFromType<TEntity>(this.IncludeIdField, sample).Fields,
                //Settings = new JsGridTableSettings()
            };
            return res;
        }

        public void Post(string dataAccess, TEntity client)
        {
            this.jsGridDataStorage.Update(dataAccess, client.ToExpandoObject());
        }

        public void Put(string dataAccess, TEntity editedClient)
        {
            this.jsGridDataStorage.Save(dataAccess, editedClient.ToExpandoObject());
        }

        public void Delete(string dataAccess, string id)
        {
            this.jsGridDataStorage.Delete(dataAccess, id);
        }

        public object GetDataSampleForSchema(HttpRequestMessage request, string dataaccess)
        {
           return this.jsGridDataStorage.GetDataSampleForSchemaByDataAccess(dataaccess);
        }

        public object GetDataSampleForSchemaReplacement(HttpRequestMessage request, string dataaccess)
        {
            return this.jsGridDataStorage.GetReplacementDataSampleForSchemaByDataAccess(dataaccess);
        }
    }
}