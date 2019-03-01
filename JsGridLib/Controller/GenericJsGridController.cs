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

         
            JsGridStorageStatistics all = opts.Top==null?
                this.Service.GetAll(this.Request, dataaccess, opts) :
                this.Service.GetAllTop(this.Request, dataaccess, opts, opts.Top?.Value ?? 100, opts.Skip?.Value ?? 0);
           
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

    public class GenericJsGridController : GenericJsGridController<dynamic>
    {
        public GenericJsGridController(IGridRequestOptions gridRequestOptions,bool includeIdField=false)
            : base(gridRequestOptions, includeIdField)
        {
        }

       
    }

    #endregion

}