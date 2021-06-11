using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ET;
using BL;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Http.Cors;
using MasQueJabones_API.Filters;

namespace MasQueJabones_API.Controllers
{
    
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductsController : ApiController
    {
        private ProductsBL PBL = new ProductsBL();

        [HttpGet]
        [AllowAnonymous]
        [ResponseType(typeof(IEnumerable<PrimaryProduct>))]
        public HttpResponseMessage List()
        {
            var r = PBL.List();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);

        }

        [HttpPost]
        [ApiKeyAuthentication]
        [Route("api/Products/AddNew")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage AddNew([FromBody] Product model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = PBL.AddNew(model, UserName);

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
        [Route("api/Products/Update")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Update([FromBody] Product model)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = PBL.Update(model, UserName);

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