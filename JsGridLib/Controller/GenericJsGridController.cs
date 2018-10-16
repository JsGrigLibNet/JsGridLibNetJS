namespace JsGridLib.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using JsGridLib.Contracts;
    using JsGridLib.Models;
    using JsGridLib.Services;
    

    public class GenericJsGridController<T> : GenericJsGridController<T, T, T, T, T>
        where T : class, IJsGridEntity, new()
    {
        public GenericJsGridController(bool includeIdField, Func<IEnumerable<T>, T, IEnumerable<T>> filter, object validationDefinition, IJsGridStorage<T> storage)
            : base(includeIdField, filter, validationDefinition, storage, pe => pe, pe => pe, pe => pe, pe => pe, pe => pe)
        {
        }

        public GenericJsGridController(Func<IEnumerable<T>, T, IEnumerable<T>> filter, object validationDefinition, IJsGridStorage<T> storage)
            : base(false, filter, validationDefinition, storage, pe => pe, pe => pe, pe => pe, pe => pe, pe => pe)
        {
        }
    }

    public class GenericJsGridController<TEntity, TReadable, TUpdateable, TCreatable, TDeletable>
        : ApiController
        where TReadable : class, IJsGridEntity, new()
        where TEntity : class, IJsGridEntity, new()
        where TUpdateable : new()
        where TCreatable : new()
        where TDeletable : new()
    {
        readonly object validationDefinition = new { };

        public GenericJsGridController(
            bool includeIdField,
            Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
            object validationDefinition,
            IJsGridStorage<TEntity> storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter,
            Func<TUpdateable, TEntity> editableToEntityConverter,
            Func<TCreatable, TEntity> creatableToEntityConverter,
            Func<TDeletable, TEntity> deletableToEntityConverter)
        {
            this.IncludeIdField = includeIdField;
            this.DeletableToEntityConverter = deletableToEntityConverter;
            this.CreatableToEntityConverter = creatableToEntityConverter;
            this.EditableToEntityConverter = editableToEntityConverter;
            this.Filter = filter;
            this.validationDefinition = validationDefinition;
            this.Service = new JsGridDataService<TEntity, TReadable, TUpdateable, TCreatable, TDeletable>(
                storage,
                pocoToEntityConverter,
                entityToPocoConverter,
                this.IncludeIdField);
        }

        Func<TUpdateable, TEntity> EditableToEntityConverter { get; }

        Func<TCreatable, TEntity> CreatableToEntityConverter { get; }

        Func<TDeletable, TEntity> DeletableToEntityConverter { get; }

        bool IncludeIdField { get; }

        Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> Filter { get; }

        public JsGridDataService<TEntity, TReadable, TUpdateable, TCreatable, TDeletable> Service { set; get; }

        [HttpGet]
        public object GetValidation()
        {
            return this.validationDefinition;
        }

        [HttpGet]
        public JsGridStorageStatistics<TReadable> GetAll(int take = 100, int skip = 0)
        {
            return this.Service.GetAll(this.Request, this.Filter, take, skip);
        }

        [HttpGet]
        public JsGridObject<TReadable> GetSchemaAndSettings()
        {
            return this.Service.GetSchemaAndSettings();
        }

        [HttpPost]
        public void Post([FromBody] TUpdateable client)
        {
            this.Service.Post(this.EditableToEntityConverter(client));
        }

        [HttpPost]
        public void Put([FromBody] TCreatable editedClient)
        {
            this.Service.Put(this.CreatableToEntityConverter(editedClient));
        }

        [HttpPost]
        public void Delete([FromBody] TDeletable editedClient)
        {
            this.Service.Delete(this.DeletableToEntityConverter(editedClient));
        }
    }
}