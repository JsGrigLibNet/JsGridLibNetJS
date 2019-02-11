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
        where TReadable : IJsGridEntity, new()
        where TEntity : IJsGridEntity, new()
        where TUpdateable : new()
        where TDeletable : new()
        where TCreatable : new()
    {
        readonly Func<TEntity, TReadable> entityToReadableConverter;
        readonly IJsGridStorage<TEntity> jsGridDataStorage;
        readonly Func<TReadable, TEntity> pocoToEntityConverter;

        public JsGridDataService(
            IJsGridStorage<TEntity> jsGridDataStorage,
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

        public JsGridStorageStatistics<TReadable> GetAll(HttpRequestMessage request, Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> clientFiltering, int take, int skip)
        {
            var sample = this.GetFilterInstance<TReadable>(HttpUtility.ParseQueryString(request.RequestUri.Query));

            JsGridStorageStatistics<TEntity> result = this.jsGridDataStorage.LoadAll(this.pocoToEntityConverter(sample), clientFiltering, take, skip);
            return new JsGridStorageStatistics<TReadable>
            {
                Total = result.Total,
                Results = result.Results.Select(x => this.entityToReadableConverter(x))
            };
        }

        public T GetFilterInstance<T>(NameValueCollection fil)
            where T : new()
        {
            var filterObj = new T();
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

        public JsGridObject<TReadable> GetSchemaAndSettings()
        {
            var res = new JsGridObject<TReadable>
            {
                FieldsReadable = new SchemaFactory().SchemaFromType<TReadable>(this.IncludeIdField).Fields,
                FieldsUpdateable = new SchemaFactory().SchemaFromType<TUpdateable>(this.IncludeIdField).Fields,
                FieldsDeletable = new SchemaFactory().SchemaFromType<TDeletable>(this.IncludeIdField).Fields,
                FieldsCreatable = new SchemaFactory().SchemaFromType<TCreatable>(this.IncludeIdField).Fields,
                Settings = new JsGridTableSettings()
            };
            return res;
        }

        public void Post(TEntity client)
        {
            this.jsGridDataStorage.Update(client.Id, client);
        }

        public void Put(TEntity editedClient)
        {
            this.jsGridDataStorage.Save(editedClient);
        }

        public void Delete(string id)
        {
            this.jsGridDataStorage.Delete(id);
        }
    }
}