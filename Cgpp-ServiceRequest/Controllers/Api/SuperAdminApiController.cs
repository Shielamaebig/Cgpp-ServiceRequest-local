using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Web;
using System.Data.Entity;
using Cgpp_ServiceRequest.Models.Extensions;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class SuperAdminApiController : ApiController
    {
        public ApplicationDbContext _db;

        public SuperAdminApiController()
        {
            _db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        [HttpGet]
        [Route("api/get/SuperAdmin")]
        public IHttpActionResult GetSuperAdmin()
        {
            var superAdmin= _db.SuperAdmins.ToList().Select(Mapper.Map<SuperAdmin, SuperAdminDto>);
            return Ok(superAdmin);
        }
        [HttpPost]
        [Route("api/superAdmin/save")]
        public IHttpActionResult saveSuperAdmin( SuperAdminDto superAdminDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var sAdmin = Mapper.Map<SuperAdminDto, SuperAdmin>(superAdminDto);

            if (_db.SuperAdmins.Any(d => d.Name == sAdmin.Name || d.Name == null))
            {
                return BadRequest("Super Admin name already Exist");
            }

            SuperAdmin suadmin = new SuperAdmin();
            superAdminDto.Id = sAdmin.Id;
            superAdminDto.Name = sAdmin.Name;
            superAdminDto.Email = sAdmin.Email;
            _db.SuperAdmins.Add(sAdmin);
            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + sAdmin.Id),superAdminDto);
        }

        [HttpGet]
        [Route("api/get/superAdminbyId/{id}")]
        public IHttpActionResult GetSuperAdmins(int id)
        {
            var superAdmin = _db.SuperAdmins.SingleOrDefault(d => d.Id == id);
            if (superAdmin == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<SuperAdmin, SuperAdminDto>(superAdmin));
        }

        [HttpPut]
        [Route("api/update/superAdmin/{id}")]
        public IHttpActionResult UpdateSAdmin(int id, SuperAdminDto superAdminDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var superAdminInDb = _db.SuperAdmins.SingleOrDefault( s=>s.Id == id);
            if (superAdminInDb == null)
            {
                return NotFound();
            }
            Mapper.Map(superAdminDto, superAdminInDb);
            superAdminInDb.Id = id;
            _db.SaveChanges();
            return Ok();
        }
    }
}
