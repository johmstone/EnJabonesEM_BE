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
using MasQueJabones_API.Filters;

namespace MasQueJabones_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrdersController : ApiController
    {
        private OrdersBL OBL = new OrdersBL();

        [HttpPost]
        [Route("api/Orders/StagingOrders/AddNew")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage AddNewStagingOrder([FromBody] StagingOrder model)
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


        [HttpPost]
        [Route("api/Orders/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] NewOrder newOrder)
        {
            var r = OBL.AddOrder(newOrder);

            if (r)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Orders/List")]
        [ResponseType(typeof(List<Order>))]
        public HttpResponseMessage OrderList([FromBody] SearchOrder Details)
        {
            var r = OBL.OrderList(Details);

            if (r.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("api/Orders/{OrderID}")]
        [ResponseType(typeof(Order))]
        public HttpResponseMessage OrderDetails(string OrderID)
        {
            var r = OBL.OrderDetails(OrderID);

            if (String.IsNullOrEmpty(r.OrderID))
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);                
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
        }
    }
}