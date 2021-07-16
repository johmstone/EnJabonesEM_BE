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
using System.Configuration;
using System.IO;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

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
                try
                {
                    SendOrdenConfirmation(newOrder.OrderID, newOrder.EmailNotification);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Orders/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] Order Model)
        {
            var authHeader = Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = OBL.UpdateOrder(Model, UserName);

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
        [ApiKeyAuthentication]
        [Route("api/Orders/Statuses/{StatusType}")]
        [ResponseType(typeof(List<OrderStatus>))]
        public HttpResponseMessage Statuses(string StatusType)
        {
            var r = OBL.Statuses(StatusType);

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

        public void SendOrdenConfirmation(string OrderID, string EmailNotification)
        {
            string LinkURL = ConfigurationManager.AppSettings["FrontEnd_URL"] + "/Order/" + OrderID;
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/OrdenConfirmation.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{OrderID}", OrderID);
            body = body.Replace("{LinkURL}", LinkURL);
            //body = body.Replace("{GUID}", Code.GUID);

            Emails Email = new Emails()
            {
                FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                ToEmail = EmailNotification,
                SubjectEmail = "MasQueJabones - Confirmación de Orden",
                BodyEmail = body
            };

            MailMessage mm = new MailMessage(Email.FromEmail, Email.ToEmail)
            {
                Subject = Email.SubjectEmail,
                Body = Email.BodyEmail,
                IsBodyHtml = true,
                BodyEncoding = Encoding.GetEncoding("utf-8")
            };

            SmtpClient smtp = new SmtpClient();
            smtp.Send(mm);

        }

    }
}