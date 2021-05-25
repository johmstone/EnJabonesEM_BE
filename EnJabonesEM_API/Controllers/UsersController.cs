using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ET;
using BL;
using EnJabonesEM_API.Filters;
using System.Web.Http.Description;

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
    }
}