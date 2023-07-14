using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class DraftsController : ApiController
    {
        private IlsContext _ctx;

        public DraftsController()
        {
            _ctx = new IlsContext();
        }
        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
        }

        [Route("api/drafts2")]
        public IHttpActionResult GetBilat()
        {
            var entity = _ctx.Drafts.Take(5).ToList().Select(Mapper.Map<Draft, DraftsDto>);
            return Ok(entity);
        }
    }
}
