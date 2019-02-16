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
    public class GenericJsGridController<TEntity, TReadable, TUpdateable, TCreatable, TDeletable>
       : ApiController

    {
        readonly object validationDefinition = new { };

        public GenericJsGridController(
            bool includeIdField,
            Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> filter,
            object validationDefinition,
            IJsGridStorage storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter,
            Func<TUpdateable, TEntity> editableToEntityConverter,
            Func<TCreatable, TEntity> creatableToEntityConverter
        
        )
        {
            this.IncludeIdField = includeIdField;
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

      
        bool IncludeIdField { get; }

        Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> Filter { get; }

        public JsGridDataService<TEntity, TReadable, TUpdateable, TCreatable, TDeletable> Service { set; get; }

        [HttpGet]
        public virtual PageResult<dynamic> Get(ODataQueryOptions opts,[FromUri]string dataaccess)
        {
            #region MyRegion

            IEnumerable<KeyValuePair<string, string>> queryString = this.Request.GetQueryNameValuePairs();
            KeyValuePair<string, string> filter = queryString.FirstOrDefault(x => x.Key == "$filter");
            FilterQueryOption cleanFilter = filter.Value == null || !filter.Value.Contains("(undefined(null,undefined))") ? opts.Filter : new FilterQueryOption(filter.Value.Replace("(undefined(null,undefined)) or ", "").Replace(" or (undefined(null,undefined))", "").Replace("(undefined(null,undefined)) and ", "").Replace(" and (undefined(null,undefined))", "").Trim(), opts.Context);

            JsGridStorageStatistics all = opts.Top==null?
                this.Service.GetAll(this.Request, dataaccess, this.Filter):
                this.Service.GetAllTop(this.Request, dataaccess, this.Filter, opts.Top?.Value ?? 100, opts.Skip?.Value ?? 0);
            //IQueryable<dynamic> emp = all.Results.ToList().AsQueryable();

            //long count = all.Total;

            //if (opts.OrderBy != null)
            //    emp = opts.OrderBy.ApplyTo(emp); //perform sort
            //if (opts.Filter != null)
            //    try
            //    {
            //        emp = cleanFilter.ApplyTo(emp, new ODataQuerySettings()).Cast<object>();
            //    }
            //    catch (Exception e)
            //    {
            //    }

            //if (opts.InlineCount != null)
            //    count = all.Total;
            //if (opts.Skip != null)
            //    emp = opts.Skip.ApplyTo(emp, new ODataQuerySettings()); //perform skip
            //if (opts.Top != null)
            //    emp = opts.Top.ApplyTo(emp, new ODataQuerySettings()); //perform take

            #endregion

            return new PageResult<dynamic>(all.Results, null, all.Total);
        }

        [HttpGet]
        public virtual HttpResponseMessage Get(string id, [FromUri]string dataaccess)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, this.Service.GetById(this.Request, dataaccess, this.Filter, id));
        }

        [HttpPost]
        public virtual HttpResponseMessage Put([FromBody] TUpdateable client, [FromUri]string dataaccess)
        {
            TEntity entity = this.EditableToEntityConverter(client);
            this.Service.Put(dataaccess, entity);
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        //Used to modify and update a resource
        [HttpPut]
        public virtual HttpResponseMessage Post([FromBody] TCreatable editedClient, [FromUri]string dataaccess)
        {
            TEntity creatable = this.CreatableToEntityConverter(editedClient);
            this.Service.Post(dataaccess, creatable);
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public virtual HttpResponseMessage Delete(string id, [FromUri]string dataaccess)
        {
            this.Service.Delete(dataaccess, id);
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }

    #region MyRegion

    public class GenericJsGridController<TEntity> : GenericJsGridController<TEntity, TEntity, TEntity, TEntity, TEntity>

    {
        public GenericJsGridController(bool includeIdField, Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> filter, object validationDefinition, IJsGridStorage storage)
            : base(includeIdField, filter, validationDefinition, storage, pe => pe, pe => pe, pe => pe, pe => pe)
        {
        }

        public GenericJsGridController(Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> filter, object validationDefinition, IJsGridStorage storage)
            : base(false, filter, validationDefinition, storage, pe => pe, pe => pe, pe => pe, pe => pe)
        {
        }
    }

    public class GenericJsGridController<TEntity, TReadable> : GenericJsGridController<TEntity, TReadable, TReadable, TReadable, TReadable>

    {
        public GenericJsGridController(
            bool includeIdField,
            Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> filter,
            object validationDefinition,
            IJsGridStorage storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter)
            : base(includeIdField, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, pocoToEntityConverter, pocoToEntityConverter)
        {
        }

        public GenericJsGridController(
            Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> filter,
            object validationDefinition,
            IJsGridStorage storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter)
            : base(false, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, pocoToEntityConverter, pocoToEntityConverter)
        {
        }
    }

    public class GenericJsGridController<TEntity, TReadable, TUpdateable> : GenericJsGridController<TEntity, TReadable, TUpdateable, TReadable, TReadable>

    {
        public GenericJsGridController(
            bool includeIdField,
            Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> filter,
            object validationDefinition,
            IJsGridStorage storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter,
            Func<TUpdateable, TEntity> editableToEntityConverter)
            : base(includeIdField, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, pocoToEntityConverter)
        {
        }

        public GenericJsGridController(
            Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> filter,
            object validationDefinition,
            IJsGridStorage storage,
            Func<TReadable, TEntity> pocoToEntityConverter,
            Func<TEntity, TReadable> entityToPocoConverter,
            Func<TUpdateable, TEntity> editableToEntityConverter)
            : base(false, filter, validationDefinition, storage, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, pocoToEntityConverter)
        {
        }
    }
    
   
    public class GenericJsGridController : GenericJsGridController<dynamic>
    {
        public GenericJsGridController(bool includeIdField, Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> filter, object validationDefinition, IJsGridStorage storage)
            : base(includeIdField, filter, validationDefinition, storage)
        {
        }

        public GenericJsGridController(Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> filter, object validationDefinition, IJsGridStorage storage)
            : base(filter, validationDefinition, storage)
        {
        }
    }

    #endregion

}