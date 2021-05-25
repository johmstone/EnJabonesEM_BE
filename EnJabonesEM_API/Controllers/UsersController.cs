using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;
using EnJabonesEM_API.Filters;
using System.Web.Http.Description;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace EnJabonesEM_API.Controllers
{
    [ApiKeyAuthentication]
    public class UsersController : ApiController
    {
        private UserBL UBL = new UserBL();

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
    }
}