using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;
using ET;
using BL;
using System.IdentityModel.Tokens.Jwt;
using MasQueJabones_API.Filters;

namespace MasQueJabones_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PrimaryProductsController : ApiController
    {
        private PrimaryProductsBL PPBL = new PrimaryProductsBL();

        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(IEnumerable<PrimaryProduct>))]
        public HttpResponseMessage List()
        {
            var r = PPBL.List();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);

        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/PrimaryProducts/{PrimaryProductID}")]
        [ResponseType(typeof(PrimaryProduct))]
        public HttpResponseMessage Details(int PrimaryProductID)
        {
            var r = PPBL.Details(PrimaryProductID);

            if (r.PrimaryProductID > 0)
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
        [Route("api/PrimaryProducts/Formula/{PrimaryProductID}")]
        [ResponseType(typeof(List<Formula>))]
        public HttpResponseMessage Formula(int PrimaryProductID)
        {
            var r = PPBL.Formula(PrimaryProductID);

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
        [Route("api/PrimaryProducts/Formula/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddFormula([FromBody] PrimaryProduct Details)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = PPBL.AddFormula(Details, UserName);

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
        [Route("api/PrimaryProducts/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] PrimaryProduct model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = PPBL.Update(model, UserName);

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
        [Route("api/PrimaryProducts/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] PrimaryProduct model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = PPBL.AddNew(model, UserName);

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