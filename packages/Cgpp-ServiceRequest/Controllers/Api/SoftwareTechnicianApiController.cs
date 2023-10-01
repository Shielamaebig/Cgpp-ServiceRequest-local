using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cgpp_ServiceRequest.Models.Extensions;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Web;
using System.Data.Entity;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class SoftwareTechnicianApiController : ApiController
    {
        private ApplicationDbContext _db;
        public SoftwareTechnicianApiController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        //GET
        [HttpGet]
        [Route("api/stech/Gettech")]
        public IHttpActionResult GetTechnicians()
        {
            var softwareTech = _db.SoftwareTechnician.ToList().Select(Mapper.Map<SoftwareTechnician, SoftwareTechnicianDto>);
            return Ok(softwareTech);
        }

        //Save
        [HttpPost]
        [Route("api/stech/save")]
        public IHttpActionResult SaveTechnician(SoftwareTechnicianDto softwareTechnicianDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var softwareTech = Mapper.Map<SoftwareTechnicianDto, SoftwareTechnician>(softwareTechnicianDto);

            if (_db.SoftwareTechnician.Any(d => d.Name == softwareTech.Name || d.Name == null))
            {
                return BadRequest("Department name already Exist");
            }
            softwareTech.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            SoftwareTechnician stech = new SoftwareTechnician();

            softwareTechnicianDto.Id = softwareTech.Id;
            _db.SoftwareTechnician.Add(softwareTech);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added A Software Technician",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + softwareTech.Id), softwareTechnicianDto);
        }

        //Get edit
        //GET in form
        [Route("api/stech/getTechbyId/{id}")]
        [HttpGet]
        public IHttpActionResult GetSoftwareTech(int id)
        {
            var sftTechnician = _db.SoftwareTechnician.SingleOrDefault(d => d.Id == id);
            if (sftTechnician == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<SoftwareTechnician, SoftwareTechnicianDto>(sftTechnician));
        }

        //update
        [HttpPut]
        [Route("api/stech/updateSoftwareTech/{id}")]
        public IHttpActionResult UpdateSoftwareTechnician(int id, SoftwareTechnicianDto softwareTechnicianDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sftTechInDb = _db.SoftwareTechnician.SingleOrDefault(d => d.Id == id);
            if (sftTechInDb == null)
            {
                return NotFound();
            }
            Mapper.Map(softwareTechnicianDto, sftTechInDb);
            sftTechInDb.Id = id;
            sftTechInDb.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            softwareTechnicianDto.DateAdded = DateTime.Now.ToString("MMMM dd yyyy  hh:mm tt");

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edited A Software Technician",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/stech/Delete/{id}")]
        public IHttpActionResult DeleteSoftwareTech(int id)
        {
            var softwareTech = _db.SoftwareTechnician.SingleOrDefault(d => d.Id == id);
            if (softwareTech == null)
            {
                return NotFound();
            }
            _db.SoftwareTechnician.Remove(softwareTech);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted A Software Technician",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }
    }
}
