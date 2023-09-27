using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Web;
using System.Data.Entity;
using Cgpp_ServiceRequest.Models.Extensions;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class InformationApiController : ApiController
    {
        private ApplicationDbContext _db;

        public InformationApiController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        [HttpGet]
        [Route("api/information/getSystem")]
        public IHttpActionResult GetInformationSystem()
        {
            var information = _db.InformationSystem.ToList().Select(Mapper.Map<InformationSystem, InformationSystemDto>);
            return Ok(information);
        }

        //GET in form
        [Route("api/information/getSystembyId/{id}")]
        [HttpGet]
        public IHttpActionResult GetUnitTypes(int id)
        {
            var infosystem = _db.InformationSystem.SingleOrDefault(d => d.Id == id);
            if (infosystem == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<InformationSystem, InformationSystemDto>(infosystem));
        }

        [HttpPost]
        [Route("api/information/saveSystem")]
        public IHttpActionResult SaveInformationSystem (InformationSystemDto informationSystemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var information = Mapper.Map<InformationSystemDto, InformationSystem>(informationSystemDto);

            if(_db.InformationSystem.Any(i => i.Name == information.Name || i.Name == null ))
            {
                return BadRequest("System name already Exist");
            }
            information.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            InformationSystem info = new InformationSystem();

            informationSystemDto.Id = information.Id;
            _db.InformationSystem.Add(information);


            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added A Existing System",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + information.Id), informationSystemDto);
        }

        [HttpPut]
        [Route("api/information/updateSystem/{id}")]
        public IHttpActionResult updateInformation(int id, InformationSystemDto informationSystemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var informationInDb = _db.InformationSystem.SingleOrDefault(i => i.Id == id);
            if (informationInDb == null)
            {
                return NotFound();
            }
            Mapper.Map(informationSystemDto, informationInDb);
            informationInDb.Id = id;
            informationInDb.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            informationSystemDto.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Updated A Existing System",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/information/Delete/{id}")]
        public IHttpActionResult deleteInformationSystem(int id)
        {
            var informationInDb = _db.InformationSystem.SingleOrDefault( i => i.Id == id);
            if(informationInDb == null)
            {
                return NotFound();
            }
            _db.InformationSystem.Remove(informationInDb);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted A Existing System",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }
    }
}
