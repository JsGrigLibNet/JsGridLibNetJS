namespace SampleHttpsServer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Web;
    using SampleHttpsServer.JsGridModule;

    public class JsGridDataService<TEntity,TPoco>
        where TPoco : IJsGridEntity, new()
        where TEntity : IJsGridEntity, new()
    {
        IJsGridStorage<TEntity> JsGridDataStorage;
             Func<TPoco,TEntity> PocoToEntityConverter;
        Func<TEntity,TPoco> EntityToPocoConverter;
        public JsGridDataService(IJsGridStorage<TEntity> jsGridDataStorage, Func<TPoco, TEntity> pocoToEntityConverter, Func<TEntity, TPoco> entityToPocoConverter)
        {
            this.JsGridDataStorage = jsGridDataStorage;
            this.EntityToPocoConverter = entityToPocoConverter;
            this.PocoToEntityConverter = pocoToEntityConverter;
        }
        
        public JsGridStorageStatistics<TPoco> GetAll(HttpRequestMessage request, Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> clientFiltering, int take, int skip)
        {
            var sample = GetFilterInstance<TPoco>(HttpUtility.ParseQueryString(request.RequestUri.Query));

            var result = JsGridDataStorage.LoadAll(this.PocoToEntityConverter(sample), clientFiltering,take,skip);
            return new JsGridStorageStatistics<TPoco>()
            {
                Total = result.Total,
                Results = result.Results.Select(x=> this.EntityToPocoConverter(x))
            };
        }

      public  T GetFilterInstance<T>(  NameValueCollection fil)
            where T : new()
        {
            var filterObj = new T();
            if (fil == null || fil.Count == 0)
            {
                var keys = fil.AllKeys;

                foreach (var o in keys)
                {
                    Type type = typeof(T);

                    PropertyInfo prop = type.GetProperty(o);

                    prop.SetValue(filterObj, fil[o], null);
                }
            }

            return filterObj;
        }


    
        public JsGridObject<TPoco> GetSchemaAndSettings()
        {
            var t = new SchemaFactory().SchemaFromType<TPoco>();
            var res = new JsGridObject<TPoco>()
            {
                Fields = t.Fields,
                Settings = new JsGridTableSettings()
            };
            return res;
        }
       
        public void Post(TPoco client)
        {
            JsGridDataStorage.Update(client.Id,this.PocoToEntityConverter(client));

        }


      
        public void Put(TPoco editedClient)
        {
           
            JsGridDataStorage.Save(this.PocoToEntityConverter(editedClient));
        }

       
        public void Delete(TPoco editedClient)
        {
            JsGridDataStorage.Delete(editedClient.Id, this.PocoToEntityConverter(editedClient));
        }
    }
}