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
    public class SoftwareApiController : ApiController
    {
        private ApplicationDbContext _db;

        public SoftwareApiController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET
        [Route("api/soft/get")]
        public IHttpActionResult GetSoftware()
        {
            var software = _db.Software.ToList().Select(Mapper.Map<Software, SoftwareDto>);
            return Ok(software);
        }

        //POST
        [HttpPost]
        [Route("api/soft/save")]
        public IHttpActionResult SaveSoftware(SoftwareDto softwareDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var software = Mapper.Map<SoftwareDto, Software>(softwareDto);

            if (_db.Software.Any(d => d.Name == software.Name || d.Name == null))
            {
                return BadRequest("Department name already Exist");
            }
            software.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            Software dept = new Software();

            softwareDto.Id = software.Id;
            _db.Software.Add(software);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added A Software Services Category",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + software.Id), softwareDto);
        }

        //GET in form
        [Route("api/soft/getsoftware/{id}")]
        [HttpGet]
        public IHttpActionResult GetSoftware(int id)
        {
            var softwares = _db.Software.SingleOrDefault(d => d.Id == id);
            if (softwares == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Software, SoftwareDto>(softwares));
        }

        [HttpPut]
        [Route("api/soft/updateSoftware/{id}")]
        public IHttpActionResult UpdateDept(int id, SoftwareDto softwareDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var softInDb = _db.Software.SingleOrDefault(d => d.Id == id);
            if (softInDb == null)
            {
                return NotFound();
            }
            Mapper.Map(softwareDto, softInDb);
            softInDb.Id = id;
            softInDb.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            softwareDto.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Updated A Software Services Category",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/soft/Delete/{id}")]
        public IHttpActionResult DeleteSoftware(int id)
        {
            var softwareInDb = _db.Software.SingleOrDefault(d => d.Id == id);
            if (softwareInDb == null)
            {
                return NotFound();
            }
            _db.Software.Remove(softwareInDb);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted A Software Services Category",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }
    }
}
