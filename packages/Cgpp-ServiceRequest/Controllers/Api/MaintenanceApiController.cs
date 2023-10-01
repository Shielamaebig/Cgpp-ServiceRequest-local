using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class MaintenanceApiController : ApiController
    {
        private ApplicationDbContext db;

        public MaintenanceApiController()
        {
            db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        [Route("api/maintenance/GetMaintenance")]
        public IHttpActionResult GetMaintenance()
        {
            var mdto = db.MaintenanceMode.ToList().Select(Mapper.Map<MaintenanceMode, MaintenanceModeDto>);
            return Ok(mdto);
        }

        [HttpPut]
        [Route("api/maintenance/UpdateMaintenance/{id}")]
        public IHttpActionResult UpdateMaintenance(int id, MaintenanceModeDto maintenanceModeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var maintenanceInDb = db.MaintenanceMode.SingleOrDefault(x => x.Id == id);

            if (maintenanceInDb == null)
            {
                return NotFound();
            }
            else
            {
                Mapper.Map(maintenanceModeDto, maintenanceInDb);
                maintenanceInDb.Id = 1;
                maintenanceModeDto.Id = 1;
                db.SaveChanges();
                return Ok();
            }
        }

        [Route("api/sf/getnewRequest")]
        public IHttpActionResult GetSoftwareNewRequest()
        {
            var sfDto = db.SoftwareRequest.ToList().Select(Mapper.Map<SoftwareRequest, SoftwareRequestDto>);
            return Ok(sfDto.Where(x => x.IsNew == true).OrderByDescending(u => u.DateAdded));
        }

        [Route("api/hd/getnewRequesth")]
        public IHttpActionResult GetHardwareNewRequest()
        {
            var hrDto = db.HardwareRequest.ToList().Select(Mapper.Map<HardwareRequest, HardwareRequestDto>);
            return Ok(hrDto.Where(x => x.IsNew == true).OrderByDescending(x => x.DateAdded));
        }
    }
}
