using System.Web.Http;
using ET;
using BL;
using MasQueJabones_API.Filters;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace MasQueJabones_API.Controllers
{
    [ApiKeyAuthentication]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PropertiesController : ApiController
    {
        private PropertiesBL PBL = new PropertiesBL();

        [HttpPost]
        [ResponseType(typeof(List<Property>))]
        public HttpResponseMessage List()
        {
            var r = PBL.List();

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
        [Route("api/Properties/Update/{ProductPropertyID}")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Disable(int ProductPropertyID)
        {
            var authHeader = this.Request.Headers.GetValues("Authorization").FirstOrDefault();
            var token = authHeader.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var UserName = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            var r = PBL.Disable(ProductPropertyID, UserName);

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