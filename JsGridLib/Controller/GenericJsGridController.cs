namespace JsGridLib.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using JsGridLib.Contracts;
    using JsGridLib.Models;
    using JsGridLib.Services;

    public class GenericJsGridController<TEntity> : GenericJsGridController<TEntity, TEntity, TEntity, TEntity, TEntity>
        where TEntity : class, IJsGridEntity, new()
    {
        public GenericJsGridController(bool includeIdField, Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter, object validationDefinition, IJsGridStorage<TEntity> storage)
            : base(includeIdField, filter, validationDefinition, storage, pe => pe, pe => pe, pe => pe, pe => pe)
        {
        }

        public GenericJsGridController(Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter, object validationDefinition, IJsGridStorage<TEntity> storage)
            : base(false, filter, validationDefinition, storage, pe => pe, pe => pe, pe => pe, pe => pe)
        {
        }
    }

    public class GenericJsGridController<TEntity, TReadable> : GenericJsGridController<TEntity, TReadable, TReadable, TReadable, TReadable>
        where TReadable : class, IJsGridEntity, new()
        where TEntity : class, IJsGridEntity, new()
    {
        public GenericJsGridController(
            bool includeIdField,
            Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
            object validationDefinition,
            IJsGridStorage<TEntity> storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter)
            : base(includeIdField, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, pocoToEntityConverter, pocoToEntityConverter)
        {
        }

        public GenericJsGridController(
            Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
            object validationDefinition,
            IJsGridStorage<TEntity> storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter)
            : base(false, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, pocoToEntityConverter, pocoToEntityConverter)
        {
        }
    }

    public class GenericJsGridController<TEntity, TReadable, TUpdateable> : GenericJsGridController<TEntity, TReadable, TUpdateable, TReadable, TReadable>
        where TReadable : class, IJsGridEntity, new()
        where TEntity : class, IJsGridEntity, new()
        where TUpdateable : class, IJsGridEntity, new()
    {
        public GenericJsGridController(
            bool includeIdField,
            Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
            object validationDefinition,
            IJsGridStorage<TEntity> storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter,
            Func<TUpdateable, TEntity> editableToEntityConverter)
            : base(includeIdField, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, pocoToEntityConverter)
        {
        }

        public GenericJsGridController(
            Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
            object validationDefinition,
            IJsGridStorage<TEntity> storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter,
            Func<TUpdateable, TEntity> editableToEntityConverter)
            : base(false, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, pocoToEntityConverter)
        {
        }
    }

    //public class GenericJsGridController<TEntity, TReadable, TUpdateable, TCreatable> : GenericJsGridController<TEntity, TReadable, TUpdateable, TCreatable, TReadable>
    //    where TReadable : class, IJsGridEntity, new()
    //    where TEntity : class, IJsGridEntity, new()
    //    where TUpdateable : class, IJsGridEntity, new()
    //    where TCreatable : class, IJsGridEntity, new()
    //{
    //    public GenericJsGridController(bool includeIdField,
    //        Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
    //        object validationDefinition,
    //        IJsGridStorage<TEntity> storage,
    //        Func<TReadable, TEntity> pocoToEntityConverter,
    //        Func<TEntity, TReadable> entityToPocoConverter,
    //        Func<TUpdateable, TEntity> editableToEntityConverter,
    //        Func<TCreatable, TEntity> creatableToEntityConverter)
    //        : base(includeIdField, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, creatableToEntityConverter, pocoToEntityConverter)
    //    {
    //    }

    //    public GenericJsGridController(
    //        Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> filter,
    //        object validationDefinition,
    //        IJsGridStorage<TEntity> storage,
    //        Func<TReadable, TEntity> pocoToEntityConverter,
    //        Func<TEntity, TReadable> entityToPocoConverter,
    //        Func<TUpdateable, TEntity> editableToEntityConverter,
    //        Func<TCreatable, TEntity> creatableToEntityConverter)
    //        : base(false, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, creatableToEntityConverter, pocoToEntityConverter)
    //    {
    //    }
    //}

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
            Func<TCreatable, TEntity> creatableToEntityConverter
            //,
            //Func<TDeletable, TEntity> deletableToEntityConverter
        )
        {
            this.IncludeIdField = includeIdField;
            // this.DeletableToEntityConverter = deletableToEntityConverter;
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

        //  Func<TDeletable, TEntity> DeletableToEntityConverter { get; }

        bool IncludeIdField { get; }

        Func<IEnumerable<TEntity>, TEntity, IEnumerable<TEntity>> Filter { get; }

        public JsGridDataService<TEntity, TReadable, TUpdateable, TCreatable, TDeletable> Service { set; get; }

        //[HttpGet]
        //public object GetValidation()
        //{
        //    return this.validationDefinition;
        //}

        [HttpGet]
        public virtual PageResult<TReadable> Get(ODataQueryOptions opts)
        {
            IEnumerable<KeyValuePair<string, string>> queryString = this.Request.GetQueryNameValuePairs();
            KeyValuePair<string, string> filter = queryString.FirstOrDefault(x => x.Key == "$filter");
            FilterQueryOption cleanFilter = filter.Value == null || !filter.Value.Contains("(undefined(null,undefined))") ? opts.Filter : new FilterQueryOption(filter.Value.Replace("(undefined(null,undefined)) or ", "").Replace(" or (undefined(null,undefined))", "").Replace("(undefined(null,undefined)) and ", "").Replace(" and (undefined(null,undefined))", "").Trim(), opts.Context);

            JsGridStorageStatistics<TReadable> all = this.Service.GetAll(this.Request, this.Filter, 100000, 0);
            IQueryable<TReadable> emp = all.Results.ToList().AsQueryable();

            long count = all.Total;

            if (opts.OrderBy != null)
                emp = opts.OrderBy.ApplyTo(emp); //perform sort
            if (opts.Filter != null)
                try
                {
                    emp = cleanFilter.ApplyTo(emp, new ODataQuerySettings()).Cast<TReadable>();
                }
                catch (Exception e)
                {
                }

            if (opts.InlineCount != null)
                count = all.Total;
            if (opts.Skip != null)
                emp = opts.Skip.ApplyTo(emp, new ODataQuerySettings()); //perform skip
            if (opts.Top != null)
                emp = opts.Top.ApplyTo(emp, new ODataQuerySettings()); //perform take

            return new PageResult<TReadable>(emp, null, all.Total);
        }

        [HttpGet]
        public virtual HttpResponseMessage Get(int id)
        {
            JsGridStorageStatistics<TReadable> all = this.Service.GetAll(this.Request, this.Filter, 1000, 0);
            IQueryable<TReadable> emp = all.Results.ToList().AsQueryable();
            long count = all.Total;
            return this.Request.CreateResponse(HttpStatusCode.OK, emp.FirstOrDefault(x => x.Id == id.ToString()));
        }

        //[HttpGet]
        //public JsGridObject<TReadable> GetSchemaAndSettings()
        //{
        //    return this.Service.GetSchemaAndSettings();
        //}
        //Used to create a resource, or overwrite it. While you specify the resources new URL.
        [HttpPost]
        public virtual HttpResponseMessage Put([FromBody] TUpdateable client)
        {
            this.Service.Put(this.EditableToEntityConverter(client));
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        //Used to modify and update a resource
        [HttpPut]
        public virtual HttpResponseMessage Post([FromBody] TCreatable editedClient)
        {
            this.Service.Post(this.CreatableToEntityConverter(editedClient));
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public virtual HttpResponseMessage Delete(string id)
        {
            this.Service.Delete(id);
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }

    public class GenericJsGridSchemaController<TEntity> : GenericJsGridController<TEntity, TEntity, TEntity, TEntity, TEntity>
        where TEntity : class, IJsGridEntity, new()
    {
        public GenericJsGridSchemaController()
            : base(
                false,
                null,
                null,
                null,
                null,
                null,
                null,
                null)
        {
        }

        [HttpGet]
        public override HttpResponseMessage Get(int id)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, this.Service.GetSchemaAndSettings());
        }

        [HttpGet]
        public override PageResult<TEntity> Get(ODataQueryOptions opts)
        {
            return new PageResult<TEntity>(null, null, 0);
        }

        [HttpPost]
        public override HttpResponseMessage Post([FromBody] TEntity client)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }

        [HttpPut]
        public override HttpResponseMessage Put([FromBody] TEntity editedClient)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }

        [HttpDelete]
        public override HttpResponseMessage Delete(string id)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }
    }
}