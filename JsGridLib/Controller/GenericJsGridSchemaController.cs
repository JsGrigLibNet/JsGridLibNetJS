namespace JsGridLib.Controller
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using JsGridLib.Contracts;

    public class GenericJsGridSchemaController<TEntity> : GenericJsGridController<TEntity, TEntity, TEntity, TEntity, TEntity>

    {
        public GenericJsGridSchemaController()
            : base(false, null, null, null, null, null, null, null)
        {
        }

        [HttpGet]
        public override HttpResponseMessage Get(string id, [FromUri]string dataaccess)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, this.Service.GetSchemaAndSettings());
        }

        [HttpGet]
        public override PageResult<dynamic> Get(ODataQueryOptions opts, [FromUri]string dataaccess)
        {
            return new PageResult<dynamic>(null, null, 0);
        }

        [HttpPost]
        public override HttpResponseMessage Post([FromBody] TEntity client, [FromUri]string dataaccess)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }

        [HttpPut]
        public override HttpResponseMessage Put([FromBody] TEntity editedClient, [FromUri]string dataaccess)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }

        [HttpDelete]
        public override HttpResponseMessage Delete(string id, [FromUri]string dataaccess)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }
    }

    public class GenericJsGridSchemaController : GenericJsGridController<dynamic, dynamic, dynamic, dynamic, dynamic>

    {
        private readonly object Sample;

        public GenericJsGridSchemaController(object sample)
            : base(false, null, null, null, null, null, null, null)
        {
            this.Sample = sample; //.ToExpandoObject();
        }

        [HttpGet]
        public override HttpResponseMessage Get(string id, [FromUri]string dataaccess)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, (object)this.Service.GetSchemaAndSettings(this.Sample));
        }

        [HttpGet]
        public override PageResult<dynamic> Get(ODataQueryOptions opts, [FromUri]string dataaccess)
        {
            return new PageResult<dynamic>(null, null, 0);
        }

        [HttpPost]
        public override HttpResponseMessage Post([FromBody] dynamic client, [FromUri]string dataaccess)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }

        [HttpPut]
        public override HttpResponseMessage Put([FromBody] dynamic editedClient, [FromUri]string dataaccess)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }

        [HttpDelete]
        public override HttpResponseMessage Delete(string id, [FromUri]string dataaccess)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }
    }

    public class GenericJsGridSchemaFromDataSourceController : GenericJsGridController<dynamic, dynamic, dynamic, dynamic, dynamic>

    {
        public GenericJsGridSchemaFromDataSourceController(IJsGridStorage storage)
            : base(false, null, null, storage, null, null, null, null)
        {
        }

        [HttpGet]
        public override HttpResponseMessage Get(string id, [FromUri]string dataaccess)
        {
            var sample = this.Service.GetDataSampleForSchema(this.Request, dataaccess);
            var schema = (object)this.Service.GetSchemaAndSettings(sample);
            return this.Request.CreateResponse(HttpStatusCode.OK, schema);
        }

        [HttpGet]
        public override PageResult<dynamic> Get(ODataQueryOptions opts, [FromUri]string dataaccess)
        {
            return new PageResult<dynamic>(null, null, 0);
        }

        [HttpPost]
        public override HttpResponseMessage Post([FromBody] dynamic client, [FromUri]string dataaccess)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }

        [HttpPut]
        public override HttpResponseMessage Put([FromBody] dynamic editedClient, [FromUri]string dataaccess)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }

        [HttpDelete]
        public override HttpResponseMessage Delete(string id, [FromUri]string dataaccess)
        {
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
        }
    }
}