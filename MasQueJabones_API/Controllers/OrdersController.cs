using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace MasQueJabones_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrdersController : ApiController
    {
        private OrdersBL OBL = new OrdersBL();

        [HttpPost]
        [Route("api/Orders/StagingOrders/AddNew")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage AddNew([FromBody] StagingOrder model)
        {
            var r = OBL.AddStagingOrder(model);

            if (r.Length > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("api/Orders/StagingOrders/{StagingOrderID}")]
        [ResponseType(typeof(StagingOrder))]
        public HttpResponseMessage AddStagingOrder(string StagingOrderID)
        {
            var r = OBL.SearchStaginOrder(StagingOrderID);

            if (r.StagingOrderID.Length > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}