using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;
using MasQueJabones_API.Filters;
using System.Web.Http.Description;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web.Http.Cors;

namespace MasQueJabones_API.Controllers
{
    [ApiKeyAuthentication]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        private UserBL UBL = new UserBL();
        private DeliveryAddressBL DBL = new DeliveryAddressBL();
        private FacturationInfoBL FBL = new FacturationInfoBL();

        [HttpPost]
        [ResponseType(typeof(List<User>))]
        public HttpResponseMessage List()
        {
            var r = UBL.List();

            if (r.Count > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ResponseType(typeof(User))]
        public HttpResponseMessage Details(int id)
        {
            var r = UBL.Details(id);

            if (r.UserID > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Users/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] User model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = UBL.Update(model, UserName);

            if (!r)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            else
            {

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Users/AdminResetPassword")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AdminResetPassword([FromBody] int id)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = UBL.AdminResetPassword(id, UserName);

            if (!r)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            else
            {
                var model = UBL.Details(id);

                #region Email
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/AdminResetPassword.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{FullName}", model.FullName);
                body = body.Replace("{Email}", model.Email);
                body = body.Replace("{Password}", "!234s6789");

                Emails Email = new Emails()
                {
                    FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                    ToEmail = model.Email,
                    SubjectEmail = "Su contraseña de EnJabonesEM.com ha sido restablecida",
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
                #endregion

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Users/DeliveryAddress/{UserID}")]
        [ResponseType(typeof(List<DeliveryAddress>))]
        public HttpResponseMessage Address(int UserID)
        {
            var r = DBL.List(UserID);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
            
        }

        [HttpPost]
        [Route("api/Users/DeliveryAddress/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage UpdateDeliveryAddress([FromBody] DeliveryAddress model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = DBL.Update(model, UserName);

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
        [Route("api/Users/DeliveryAddress/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNewDeliveryAddress([FromBody] DeliveryAddress model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = DBL.AddNew(model, UserName);

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
        [Route("api/Users/FacturationInfo/{UserID}")]
        [ResponseType(typeof(List<FacturatioInfo>))]
        public HttpResponseMessage FacturationInfo(int UserID)
        {
            var r = FBL.List(UserID);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);

        }

        [HttpPost]
        [Route("api/Users/FacturationInfo/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage UpdateFacturationInfo([FromBody] FacturatioInfo model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = FBL.Update(model, UserName);

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
        [Route("api/Users/FacturationInfo/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNewFacturationInfo([FromBody] FacturatioInfo model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = FBL.AddNew(model, UserName);

            if (r)
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