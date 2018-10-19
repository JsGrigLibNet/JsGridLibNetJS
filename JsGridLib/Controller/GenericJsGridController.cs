namespace JsGridLib.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using JsGridLib.Contracts;
    using JsGridLib.Models;
    using JsGridLib.Services;
    

    public class GenericJsGridController<TEntity> : GenericJsGridController<TEntity, TEntity, TEntity, TEntity, TEntity>
        where TEntity : class, IJsGridEntity, new()
    {
        public GenericJsGridController(bool includeIdField, Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter, object validationDefinition, IJsGridStorage<TEntity> storage)
            : base(includeIdField, filter, validationDefinition, storage, pe => pe, pe => pe, pe => pe, pe => pe, pe => pe)
        {
        }

        public GenericJsGridController(Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter, object validationDefinition, IJsGridStorage<TEntity> storage)
            : base(false, filter, validationDefinition, storage, pe => pe, pe => pe, pe => pe, pe => pe, pe => pe)
        {
        }
    }
    public class GenericJsGridController<TEntity, TReadable> : GenericJsGridController<TEntity, TReadable, TReadable, TReadable, TReadable>
        where TReadable : class, IJsGridEntity, new()
        where TEntity : class, IJsGridEntity, new()
    {
        public GenericJsGridController(bool includeIdField,
            Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
            object validationDefinition,
            IJsGridStorage<TEntity> storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter)
            : base(includeIdField, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, pocoToEntityConverter, pocoToEntityConverter, pocoToEntityConverter)
        {
        }

        public GenericJsGridController(
            Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
            object validationDefinition,
            IJsGridStorage<TEntity> storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter)
            : base(false, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, pocoToEntityConverter, pocoToEntityConverter, pocoToEntityConverter)
        {
        }
    }


    public class GenericJsGridController<TEntity, TReadable, TUpdateable> : GenericJsGridController<TEntity, TReadable, TUpdateable, TReadable, TReadable>
        where TReadable : class, IJsGridEntity, new()
        where TEntity : class, IJsGridEntity, new()
        where TUpdateable : class, IJsGridEntity, new()
    {
        public GenericJsGridController(bool includeIdField,
            Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
            object validationDefinition,
            IJsGridStorage<TEntity> storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter,
            Func<TUpdateable, TEntity> editableToEntityConverter)
            : base(includeIdField, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, pocoToEntityConverter, pocoToEntityConverter)
        {
        }

        public GenericJsGridController(
            Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
            object validationDefinition,
            IJsGridStorage<TEntity> storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter,
            Func<TUpdateable, TEntity> editableToEntityConverter)
            : base(false, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, pocoToEntityConverter, pocoToEntityConverter)
        {
        }
    }


    public class GenericJsGridController<TEntity, TReadable, TUpdateable, TCreatable> : GenericJsGridController<TEntity, TReadable, TUpdateable, TCreatable, TReadable>
        where TReadable : class, IJsGridEntity, new()
        where TEntity : class, IJsGridEntity, new()
        where TUpdateable : class, IJsGridEntity, new()
        where TCreatable : class, IJsGridEntity, new()
    {
        public GenericJsGridController(bool includeIdField,
            Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
            object validationDefinition,
            IJsGridStorage<TEntity> storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter,
            Func<TUpdateable, TEntity> editableToEntityConverter,
            Func<TCreatable, TEntity> creatableToEntityConverter)
            : base(includeIdField, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, creatableToEntityConverter, pocoToEntityConverter)
        {
        }

        public GenericJsGridController(
            Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
            object validationDefinition,
            IJsGridStorage<TEntity> storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter,
            Func<TUpdateable, TEntity> editableToEntityConverter,
            Func<TCreatable, TEntity> creatableToEntityConverter)
            : base(false, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, creatableToEntityConverter, pocoToEntityConverter)
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