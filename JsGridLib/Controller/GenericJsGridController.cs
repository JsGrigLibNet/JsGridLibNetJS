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
    public class GenericJsGridController<TEntity>
       : ApiController

    {
        readonly object validationDefinition = new { };

        protected IGridRequestOptions GridRequestOptions { set; get; }
        public GenericJsGridController(
            IGridRequestOptions gridRequestOptions,
            bool includeIdField
        
        )
        {
            if (gridRequestOptions != null)
                this.GridRequestOptions = gridRequestOptions;
            this.IncludeIdField = includeIdField;
            //this.CreatableToEntityConverter = creatableToEntityConverter;
            //this.EditableToEntityConverter = editableToEntityConverter;
           
            this.validationDefinition = validationDefinition;
            this.Service = new JsGridDataService<TEntity>(
                gridRequestOptions. storage,
                this.IncludeIdField
                );
        }

       
      
        bool IncludeIdField { get; }

       // Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> Filter { get; }

        public JsGridDataService<TEntity> Service { set; get; }

        [HttpGet]
        public virtual PageResult<dynamic> Get(ODataQueryOptions opts,[FromUri]string dataaccess)
        {

            if (!GridRequestOptions.IsAuthorized(Request))
            {
                return  new PageResult<dynamic>(new List<dynamic>(), null, 0);
            }

            #region MyRegion

            //IEnumerable<KeyValuePair<string, string>> queryString = this.Request.GetQueryNameValuePairs();
            //KeyValuePair<string, string> filter = queryString.FirstOrDefault(x => x.Key == "$filter");
            //FilterQueryOption cleanFilter = filter.Value == null || !filter.Value.Contains("(undefined(null,undefined))") ? opts.Filter : new FilterQueryOption(filter.Value.Replace("(undefined(null,undefined)) or ", "").Replace(" or (undefined(null,undefined))", "").Replace("(undefined(null,undefined)) and ", "").Replace(" and (undefined(null,undefined))", "").Trim(), opts.Context);

            JsGridStorageStatistics all = opts.Top==null?
                this.Service.GetAll(this.Request, dataaccess, opts) :
                this.Service.GetAllTop(this.Request, dataaccess, opts, opts.Top?.Value ?? 100, opts.Skip?.Value ?? 0);
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
            if (!GridRequestOptions.IsAuthorized(Request))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"You are not allowed to access {dataaccess}");
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, this.Service.GetById(this.Request, dataaccess,  id));
        }

        [HttpPost]
        public virtual HttpResponseMessage Put([FromBody] TEntity client, [FromUri]string dataaccess)
        {
            if (!GridRequestOptions.IsAuthorized(Request))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"You are not allowed to access {dataaccess}");
            }

            TEntity entity = client;
            this.Service.Put(dataaccess, entity);
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        //Used to modify and update a resource
        [HttpPut]
        public virtual HttpResponseMessage Post([FromBody] TEntity editedClient, [FromUri]string dataaccess)
        {
            if (!GridRequestOptions.IsAuthorized(Request))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"You are not allowed to access {dataaccess}");
            }

            TEntity creatable = editedClient;
            this.Service.Post(dataaccess, creatable);
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public virtual HttpResponseMessage Delete(string id, [FromUri]string dataaccess)
        {
            if (!GridRequestOptions.IsAuthorized(Request))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"You are not allowed to access {dataaccess}");
            }
            this.Service.Delete(dataaccess, id);
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }

    #region MyRegion

    //public class GenericJsGridController<TEntity> : GenericJsGridController<TEntity, TEntity, TEntity, TEntity, TEntity>

    //{
    //    public GenericJsGridController(IGridRequestOptions gridRequestOptions,bool includeIdField, Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> filter, object validationDefinition, IJsGridStorage storage)
    //        : base(gridRequestOptions, includeIdField, pe => pe, pe => pe, pe => pe, pe => pe)
    //    {
    //    }

    //    public GenericJsGridController(IGridRequestOptions gridRequestOptions, object validationDefinition, IJsGridStorage storage)
    //        : base(gridRequestOptions, false, pe => pe, pe => pe, pe => pe, pe => pe)
    //    {
    //    }
    //}

    //public class GenericJsGridController<TEntity, TEntity> : GenericJsGridController<TEntity, TEntity, TEntity, TEntity, TEntity>

    //{
    //    public GenericJsGridController(
    //        bool includeIdField,
    //        IGridRequestOptions gridRequestOptions,
    //        object validationDefinition,
    //        IJsGridStorage storage,
    //        Func<TEntity, TEntity> pocoToEntityConverter,
    //        Func<TEntity, TEntity> entityToPocoConverter)
    //        : base(gridRequestOptions, includeIdField, pocoToEntityConverter, entityToPocoConverter, pocoToEntityConverter, pocoToEntityConverter)
    //    {
    //    }

    //    public GenericJsGridController(
    //        IGridRequestOptions gridRequestOptions,
    //        object validationDefinition,
    //        IJsGridStorage storage,
    //        Func<TEntity, TEntity> pocoToEntityConverter,
    //        Func<TEntity, TEntity> entityToPocoConverter)
    //        : base(gridRequestOptions, false, pocoToEntityConverter, entityToPocoConverter, pocoToEntityConverter, pocoToEntityConverter)
    //    {
    //    }
    //}

    //public class GenericJsGridController<TEntity, TEntity, TEntity> : GenericJsGridController<TEntity, TEntity, TEntity, TEntity, TEntity>

    //{
    //    public GenericJsGridController(
    //        bool includeIdField,
    //        IGridRequestOptions gridRequestOptions,
    //        object validationDefinition,
    //        IJsGridStorage storage,
    //        Func<TEntity, TEntity> pocoToEntityConverter,
    //        Func<TEntity, TEntity> entityToPocoConverter,
    //        Func<TEntity, TEntity> editableToEntityConverter)
    //        : base(gridRequestOptions, includeIdField, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, pocoToEntityConverter)
    //    {
    //    }

    //    public GenericJsGridController(
    //        IGridRequestOptions gridRequestOptions,
    //        object validationDefinition,
    //        IJsGridStorage storage,
    //        Func<TEntity, TEntity> pocoToEntityConverter,
    //        Func<TEntity, TEntity> entityToPocoConverter,
    //        Func<TEntity, TEntity> editableToEntityConverter)
    //        : base(gridRequestOptions, false, pocoToEntityConverter, entityToPocoConverter, editableToEntityConverter, pocoToEntityConverter)
    //    {
    //    }
    //}
    
   
    public class GenericJsGridController : GenericJsGridController<dynamic>
    {
        public GenericJsGridController(IGridRequestOptions gridRequestOptions,bool includeIdField=false)
            : base(gridRequestOptions, includeIdField)
        {
        }

       
    }

    #endregion

}