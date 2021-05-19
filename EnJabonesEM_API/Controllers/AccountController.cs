using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EnJabonesEM_API.Filters;
using System.Web;
using System.Net.Mail;

namespace EnJabonesEM_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountController : ApiController
    {
        private UserBL UBL = new UserBL();
        private TokensBL TBL = new TokensBL();
        private static string API_KEY = ConfigurationManager.AppSettings["APIStack_KEY"].ToString();
        private static string API_URL = ConfigurationManager.AppSettings["APIStack_URL"].ToString();
        private static string SecretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"].ToString();
        private static int expireTime = Convert.ToInt32(ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"]);


        [HttpGet]
        [AllowAnonymous]
        [Route("api/Account/CheckEmailAvailability")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage CheckEmailAvailability(string Email)
        {
            var r = UBL.CheckUserEmailAvailability(Email);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Account/Register")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Register([FromBody] User NewUser)
        {
            var existEmail = UBL.CheckUserEmailAvailability(NewUser.Email);
            if (existEmail)
            {
                NewUser.RoleID = 1;
                var r = UBL.AddUser(NewUser, NewUser.FullName);

                GenerateEmailValidation(NewUser.Email);

                return this.Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, false);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Account/ConfirmEmail")]
        [ResponseType(typeof(int))]
        public HttpResponseMessage ConfirmEmail(string id)
        {
            var r = UBL.ValidateEmail(id);

            if (r == -1)
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                if (r == 0)
                {
                    return this.Request.CreateResponse(HttpStatusCode.Forbidden);
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK);
                }
            }
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("api/Account/ConfirmEmailRequest")]
        public HttpResponseMessage ConfirmEmailRequest(string id)
        {
            var r = UBL.DetailsbyEmail(id);

            if (r.UserID > 0)
            {
                GenerateEmailValidation(id);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }



        [HttpGet]
        [BasicAuthentication]
        [Route("api/Account/Login")]
        [ResponseType(typeof(User))]
        public HttpResponseMessage Login()
        {
            var authenticationToken = Request.Headers.Authorization.Parameter;
            var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
            var usernamePasswordArray = decodedAuthenticationToken.Split(':');
            var userName = usernamePasswordArray[0];
            var password = usernamePasswordArray[1];

            var IP = Request.Headers.GetValues("IP").First();

            UserLogin login = new UserLogin()
            {
                Email = userName,
                Password = password
            };

            User LoginUser = UBL.Login(login);

            User Details = UBL.Details(LoginUser.UserID);

            if (LoginUser.EmailValidated && !LoginUser.NeedResetPwd)
            {
                GeolocationStack location = GetGeolocation(IP);
                LoginRecord loginRecord = new LoginRecord()
                {
                    UserID = LoginUser.UserID,
                    IP = location.Ip,
                    Country = location.CountryName,
                    Region = location.RegionName,
                    City = location.City
                };

                UBL.AddLogin(loginRecord);

                //Details.RolesData = RBL.List().Where(x => x.RoleID == Details.RoleID).FirstOrDefault();
                //Details.GroupList = GBL.ListbyUser(Details.UserID);

                var token = GenerateToken(LoginUser.UserID);

                if (token.TokenID.Length > 0)
                {
                    Details.NeedResetPwd = LoginUser.NeedResetPwd;
                    Details.Token = token.TokenID;
                    Details.TokenExpires = token.ExpiresDate;
                    Details.TokenExpiresMin = expireTime;
                    return this.Request.CreateResponse(HttpStatusCode.OK, Details);

                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }

            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, Details);
            }
        }

        static GeolocationStack GetGeolocation(string IP)
        {

            string url = API_URL + IP + $"?access_key={API_KEY}";
            string resultData = string.Empty;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                resultData = reader.ReadToEnd();
            }

            GeolocationStack location = JsonConvert.DeserializeObject<GeolocationStack>(resultData);

            return location;
        }

        public Token GenerateToken(int UserID)
        {
            User Details = UBL.Details(UserID);
            var SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
            var tokenExpires = DateTime.UtcNow.AddMinutes(expireTime);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserID",Details.UserID.ToString()),
                    new Claim("Email",Details.Email),
                    new Claim("NeedResetPwd",Details.NeedResetPwd.ToString()),
                    new Claim("Role",Details.RoleName)
                }),
                Expires = tokenExpires,
                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenid = tokenHandler.WriteToken(token);

            Token NewToken = new Token()
            {
                TokenID = tokenid,
                UserID = UserID,
                ExpiresDate = tokenExpires
            };

            var r = TBL.AddNew(NewToken);

            if (!r)
            {
                return new Token();
            }
            else
            {
                return NewToken;
            }
        }

        public void GenerateEmailValidation(string email)
        {
            InfoEmailValidation Details = UBL.ValidationEmailRequest(email);
            string LinkURL = ConfigurationManager.AppSettings["FrontEnd_URL"] + "/ConfirmEmail/" + Details.EVToken;
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/EmailTemplates/ValidateEmail.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{FullName}", Details.FullName);
            body = body.Replace("{LinkURL}", LinkURL);
            //body = body.Replace("{GUID}", Code.GUID);

            Emails Email = new Emails()
            {
                FromEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                ToEmail = email,
                SubjectEmail = "EnJabonesEM - Verificar cuenta",
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