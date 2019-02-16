namespace JsGridLib.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Web;
    using JsGridLib.Contracts;
    using JsGridLib.Models;

    public class JsGridDataService<TEntity, TReadable, TUpdateable, TCreatable, TDeletable>

    {
        readonly Func<TEntity, TReadable> entityToReadableConverter;
        readonly IJsGridStorage jsGridDataStorage;
        readonly Func<TReadable, TEntity> pocoToEntityConverter;

        public JsGridDataService(
            IJsGridStorage jsGridDataStorage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToReadableConverter,
            bool includeIdField)
        {
            this.IncludeIdField = includeIdField;
            this.jsGridDataStorage = jsGridDataStorage;
            this.entityToReadableConverter = entityToReadableConverter;
            this.pocoToEntityConverter = pocoToEntityConverter;
        }

        bool IncludeIdField { get; }

        public JsGridStorageStatistics GetAll(HttpRequestMessage request,string dataAccess, Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> clientFiltering)
        {
            var sample = this.GetFilterInstance<TReadable>(HttpUtility.ParseQueryString(request.RequestUri.Query));

            JsGridStorageStatistics result = this.jsGridDataStorage.LoadAll(dataAccess, this.pocoToEntityConverter(sample), clientFiltering);
            return new JsGridStorageStatistics
            {
                Total = result.Total,
                Results = result.Results.Select(x => (object)this.entityToReadableConverter((TEntity)x))
            };
        }

        public JsGridStorageStatistics GetAllTop(HttpRequestMessage request, string dataAccess, Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> clientFiltering, int take, int skip)
        {
            var sample = this.GetFilterInstance<TReadable>(HttpUtility.ParseQueryString(request.RequestUri.Query));

            JsGridStorageStatistics result = this.jsGridDataStorage.LoadAllTop(dataAccess, take: take, sampleForFilter: this.pocoToEntityConverter(sample), clientSideFiltering: clientFiltering, skip: skip);
            return new JsGridStorageStatistics
            {
                Total = result.Total,
                Results = result.Results.Select(x => (object)this.entityToReadableConverter((TEntity)x))
            };
        }

        public TReadable GetById(HttpRequestMessage request, string dataAccess, Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> clientFiltering, string id)
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

        public JsGridObject<TReadable> GetSchemaAndSettings(object sample = null)
        {
            var res = new JsGridObject<TReadable>
            {
                FieldsReadable = new SchemaFactory().SchemaFromType<TReadable>(this.IncludeIdField, sample).Fields,
                //FieldsUpdateable = new SchemaFactory().SchemaFromType<TUpdateable>(this.IncludeIdField, sample).Fields,
                //FieldsDeletable = new SchemaFactory().SchemaFromType<TDeletable>(this.IncludeIdField, sample).Fields,
                //FieldsCreatable = new SchemaFactory().SchemaFromType<TCreatable>(this.IncludeIdField, sample).Fields,
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
    }
}