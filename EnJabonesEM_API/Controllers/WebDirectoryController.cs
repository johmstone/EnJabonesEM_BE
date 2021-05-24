using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using ET;
using BL;
using EnJabonesEM_API.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace EnJabonesEM_API.Controllers
{
    [EnableCors(origins:"*",headers:"",methods:"*")]
    public class WebDirectoryController : ApiController
    {
        private WebDirectoryBL WBL = new WebDirectoryBL();

        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(List<>))]
        [Route("api/WebDirectory/Menu")]
        public HttpResponseMessage Menu([FromBody] WebDirectoryRequest Model)
        {
            List<WebDirectory> r = WBL.Menu(Model);

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/WebDirectory/RightsValidation")]
        [ResponseType(typeof(AccessRights))]
        public HttpResponseMessage RightsValidation([FromBody] AccessRightsRequest model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            model.UserID = Convert.ToInt32(tokenS.Claims.First(claim => claim.Type == "UserID").Value);

            AccessRights Result = WBL.ValidationRights(model);

            return this.Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpPost]
        [ApiKeyAuthentication]
        [ResponseType(typeof(List<WebDirectory>))]
        public HttpResponseMessage List(int id)
        {
            var r = WBL.List(id);
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
        [Route("api/WebDirectory/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] WebDirectory model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = WBL.AddNew(model, UserName);

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
        [Route("api/WebDirectory/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] WebDirectory model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = WBL.Update(model, UserName);

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
