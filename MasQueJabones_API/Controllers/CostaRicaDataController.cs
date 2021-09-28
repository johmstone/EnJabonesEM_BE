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

namespace MasQueJabones_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CostaRicaDataController : ApiController
    {
        private CostaRicaDataBL CRBL = new CostaRicaDataBL();

        [HttpGet]
        [AllowAnonymous]
        [Route("api/CostaRica/Provinces")]
        [ResponseType(typeof(List<CostaRicaProvinces>))]
        public HttpResponseMessage Provinces()
        {
            var r = CRBL.Provinces();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/CostaRica/Cantons")]
        [ResponseType(typeof(List<CostaRicaCantons>))]
        public HttpResponseMessage Cantons()
        {
            var r = CRBL.Cantons();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/CostaRica/Districts")]
        [ResponseType(typeof(List<CostaRicaDistrics>))]
        public HttpResponseMessage Distrinc()
        {
            var r = CRBL.Districts();

            return this.Request.CreateResponse(HttpStatusCode.OK, r);
        }

    }
}