namespace SampleHttpsServer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Http;



  

    public class GenericJsGridController< T> : GenericJsGridController<T,T>
        where T : class, IJsGridEntity, new()
    {
       
        public GenericJsGridController(Func<IEnumerable<T>, T, IEnumerable<T>> filter, object validationDefinition, IJsGridStorage<T> storage)
            : base(filter, validationDefinition, storage,(pe)=>pe, (pe) => pe)
        {
        }
    }

    public class GenericJsGridController<TEntity,TPoco> : ApiController where TPoco :class, IJsGridEntity, new()  where TEntity:class, IJsGridEntity, new()
    {
        Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> Filter { set; get; }
        public GenericJsGridController(Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter, object validationDefinition, IJsGridStorage<TEntity> storage,
            Func<TPoco, TEntity> pocoToEntityConverter, Func<TEntity, TPoco> entityToPocoConverter)
        {
            this.Filter = filter;
            this.validationDefinition = validationDefinition;
            ;
            this.Service = new JsGridDataService<TEntity,TPoco>(
                storage,pocoToEntityConverter,entityToPocoConverter);
        }

        object validationDefinition = new { };
        public  JsGridDataService<TEntity,TPoco> Service { set; get; }


        [HttpGet]
        public object GetValidation()
        {
            return this.validationDefinition;
        }
        [HttpGet]
        public JsGridStorageStatistics<TPoco> GetAll(int take=100, int skip=0)
        {
            return this.Service.GetAll(this.Request, this.Filter,take,skip);
        }

        [HttpGet]
        public JsGridObject<TPoco> GetSchemaAndSettings()
        {
            return this.Service.GetSchemaAndSettings();
        }

        
        [HttpPost]
        public void Post([FromBody]TPoco client)
        {
            this.Service.Post(client);
        }

        [HttpPost]
        public void Put([FromBody]TPoco editedClient)
        {
            this.Service.Put(editedClient);
        }

        [HttpPost]
        public void Delete([FromBody]TPoco editedClient)
        {
            this.Service.Delete(editedClient);
        }
    }
}