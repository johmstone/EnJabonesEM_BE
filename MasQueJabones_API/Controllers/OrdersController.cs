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
using Newtonsoft.Json;

namespace MasQueJabones_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrdersController : ApiController
    {
        private OrdersBL OBL = new OrdersBL();
        private UserBL UBL = new UserBL();

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
                try
                {
                    SendAdminConfirmation(newOrder);                    
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
        [ResponseType(typeof(OrderList))]
        public HttpResponseMessage OrderList([FromBody] SearchOrder Details)
        {
            if(Details.ExternalStatusID == 10000)
            {
                Details.ExternalStatusID = null;
            }
            OrderList r = new OrderList()
            {
                Orders = OBL.OrderList(Details),
                Summary = OBL.OrderSummary(Details)
            };

            if (r.Orders.Count() > 0)
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

        [HttpPost]
        [Route("api/Orders/PaidConfirmation")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage PaidConfirmation([FromBody] OrderConfirmationRequestEncrypted EncryptData)
        {
            var EncryptRequest = DecryptString(EncryptData.Data);

            var Request = JsonConvert.DeserializeObject<OrderConfirmationRequest>(EncryptRequest);

            if(Request.ApiKey == ConfigurationManager.AppSettings["AdminToken"])
            {
                Order NONot = new Order()
                {
                    OrderID = Request.OrderID,
                    ActionType = "VALPAID",
                    StatusID = 30100,
                    OrderVerified = true
                };
                var r = OBL.UpdateOrder(NONot, Request.Email);
                
                if (!r)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, r);
                }
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }            
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Orders/History")]
        [ResponseType(typeof(List<OrderHistory>))]
        public HttpResponseMessage History([FromBody] OrderHistoryRequest Model)
        {
            var r = OBL.OrderHistory(Model);

            if (r.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
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

            Order NONot = new Order()
            {
                OrderID = OrderID,
                ActionType = "CHGST",
                StatusID = 20300,
                OrderVerified = false
            };

            var rc = OBL.UpdateOrder(NONot, "SystemUser");

        }

        public void SendAdminConfirmation(NewOrder Details)
        {
            
            string body = string.Empty;
            bool result = false;

            try {
                List<string> Admins = UBL.AdminUserList();

                try
                {
                    Admins.ForEach(item =>
                    {
                        OrderConfirmationRequest request = new OrderConfirmationRequest()
                        {
                            ApiKey = ConfigurationManager.AppSettings["AdminToken"],
                            OrderID = Details.OrderID,
                            Email = item
                        };

                        string EncryptCode = EnryptString(JsonConvert.SerializeObject(request));

                        string LinkURL = ConfigurationManager.AppSettings["FrontEnd_URL"] + "/Order/PaidConfirmation/" + EncryptCode + "/" + Details.OrderID;

                        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/AdminOrdenNotification.html")))
                        {
                            body = reader.ReadToEnd();
                        }

                        body = body.Replace("{OrderID}", Details.OrderID);
                        body = body.Replace("{LinkURL}", LinkURL);
                        body = body.Replace("{PaymentMethod}", Details.PaymentMethod);
                        body = body.Replace("{ProofPayment}", Details.ProofPayment);

                        Emails Email = new Emails()
                        {
                            FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                            ToEmail = item,
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

                        if (!result)
                        {
                            result = true;
                        }
                    });                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }            

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            if(result)
            {
                try
                {
                    Order NONot = new Order()
                    {
                        OrderID = Details.OrderID,
                        ActionType = "CHGST",
                        StatusID = 20400,
                        OrderVerified = false
                    };
                    var rc = OBL.UpdateOrder(NONot, "SystemUser");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe);
                decrypted = "";
            }
            return decrypted;
        }

        public string EnryptString(string strEncrypted)
        {
            byte[] b = ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

    }
}