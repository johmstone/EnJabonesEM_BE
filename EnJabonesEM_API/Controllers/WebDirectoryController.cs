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
        
    }
}
