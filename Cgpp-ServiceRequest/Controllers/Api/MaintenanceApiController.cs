using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;
using Cgpp_ServiceRequest.Models.Extensions;

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
            var divName = User.Identity.GetDivisionName();
            var pendingdivision = "Pending Division Approval";
            var pendingDepartment = "Pending Department Approval";
            var sfDto = db.SoftwareUserRequests.ToList().Select(Mapper.Map<SoftwareUserRequest, SoftwareUserRequestDto>);
            return Ok(sfDto.Where(x => x.Status == pendingdivision ||x.Status == pendingDepartment && x.DivisionName == divName).OrderByDescending(u => u.DateAdded));
        }

        [Route("api/hd/getnewRequesth")]
        public IHttpActionResult GetHardwareNewRequest()
        {
            var hrDto = db.HardwareUserRequests.ToList().Select(Mapper.Map<HardwareUserRequest, HardwareUserRequestDto>);
            return Ok(hrDto.Where(x => x.Status == "Open").OrderByDescending(x => x.DateAdded));
        }
    }
}
