using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Cgpp_ServiceRequest.Models.Extensions;


namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class HardwareTechnicianApiController : ApiController
    {
        private ApplicationDbContext _db;

        public HardwareTechnicianApiController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        //GET
        [Route("api/htech/GetTech")]
        [HttpGet]
        public IHttpActionResult GetTechnicians()
        {
            var hardTech = _db.HardwareTechnician.ToList().Select(Mapper.Map<HardwareTechnician, HardwareTechnicianDto>);
            return Ok(hardTech);
        }

        //Save
        [HttpPost]
        [Route("api/htech/save")]
        public IHttpActionResult SaveTechnician(HardwareTechnicianDto hardwareTechnicianDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var hardwareTech = Mapper.Map<HardwareTechnicianDto, HardwareTechnician>(hardwareTechnicianDto);

            if (_db.HardwareTechnician.Any(d => d.Name == hardwareTech.Name || d.Name == null))
            {
                return BadRequest("Department name already Exist");
            }
            hardwareTech.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            hardwareTechnicianDto.Id = hardwareTech.Id;
            _db.HardwareTechnician.Add(hardwareTech);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added A Hardware Technician",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + hardwareTech.Id), hardwareTechnicianDto);
        }

        //Get edit
        //GET in form
        [Route("api/htech/getTechbyId/{id}")]
        [HttpGet]
        public IHttpActionResult GetHardwareTech(int id)
        {
            var hrdTech = _db.HardwareTechnician.SingleOrDefault(d => d.Id == id);
            if (hrdTech == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<HardwareTechnician, HardwareTechnicianDto>(hrdTech));
        }

        //update
        [HttpPut]
        [Route("api/htech/updateHardwareTech/{id}")]
        public IHttpActionResult UpdateHardwareTech(int id, HardwareTechnicianDto hardwareTechnicianDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hardwareTech = _db.HardwareTechnician.SingleOrDefault(d => d.Id == id);

            if (hardwareTech == null)
            {
                return NotFound();
            }
            Mapper.Map(hardwareTechnicianDto, hardwareTech);
            hardwareTech.Id = id;
            hardwareTech.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            hardwareTechnicianDto.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edited A Hardware Technician",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            _db.SaveChanges();
            return Ok();
        }


        [HttpDelete]
        [Route("api/htech/Delete/{id}")]
        public IHttpActionResult DeleteHardwareTech(int id)
        {
            var hardwareTech = _db.HardwareTechnician.SingleOrDefault(d => d.Id == id);
            if (hardwareTech == null)
            {
                return NotFound();
            }
            _db.HardwareTechnician.Remove(hardwareTech);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted A Hardware Technician",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

       
    }
}

