namespace JsGridLib.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Web;
    using JsGridLib.Contracts;
    using JsGridLib.Models;
    using Newtonsoft.Json.Linq;

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

        public JsGridStorageStatistics GetAll(HttpRequestMessage request, Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> clientFiltering, int take, int skip)
        {
            var sample = this.GetFilterInstance<TReadable>(HttpUtility.ParseQueryString(request.RequestUri.Query));

            JsGridStorageStatistics result = this.jsGridDataStorage.LoadAll(take, this.pocoToEntityConverter(sample), clientFiltering, skip);
            return new JsGridStorageStatistics
            {
                Total = result.Total,
                Results = result.Results.Select(x => (object)this.entityToReadableConverter((TEntity)x))
            };
        }

        public TReadable GetById(HttpRequestMessage request, Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> clientFiltering, string id)
        {
            return this.jsGridDataStorage.LoadById(id);
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
                FieldsUpdateable = new SchemaFactory().SchemaFromType<TUpdateable>(this.IncludeIdField, sample).Fields,
                FieldsDeletable = new SchemaFactory().SchemaFromType<TDeletable>(this.IncludeIdField, sample).Fields,
                FieldsCreatable = new SchemaFactory().SchemaFromType<TCreatable>(this.IncludeIdField, sample).Fields,
                Settings = new JsGridTableSettings()
            };
            return res;
        }

        public void Post(TEntity client)
        {
            this.jsGridDataStorage.Update(client.ToExpandoObject());
        }

        public void Put(TEntity editedClient)
        {
            this.jsGridDataStorage.Save(editedClient.ToExpandoObject());
        }

        public void Delete(string id)
        {
            this.jsGridDataStorage.Delete(id);
        }
    }

    public static class ExpandoExtension
    {
        public static ExpandoObject ToExpandoObject(this object obj)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            var tt = new List<PropertyDescriptor>();
            if (obj is JObject)
            {
                var obj1 = (JObject)obj;
                expando = obj1.ToObject<ExpandoObject>();
                //foreach (var x in obj1)
                //{
                //    string name = x.Key;
                //    JToken value = x.Value;
                //    expando.Add(name, value.ToObject());
                //}
            }
            else
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
                    expando.Add(property.Name, property.GetValue(obj));
            }

            return (ExpandoObject)expando;
        }
    }
}