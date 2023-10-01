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
    public class HardwareApiController : ApiController
    {
        private ApplicationDbContext _db;

        public HardwareApiController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        [Route("api/hard/get")]
        public IHttpActionResult GetHardware()
        {
            var hardware = _db.Hardware.ToList().Select(Mapper.Map<Hardware, HardwareDto>);
            return Ok(hardware);
        }

        [HttpPost]
        [Route("api/hard/save")]
        public IHttpActionResult SaveHardware(HardwareDto hardwareDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var hardware = Mapper.Map<HardwareDto, Hardware>(hardwareDto);

            if (_db.Hardware.Any(d => d.Name == hardware.Name || d.Name == null))
            {
                return BadRequest("Department name already Exist");
            }
            hardware.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            Hardware dept = new Hardware();

            hardwareDto.Id = hardware.Id;
            _db.Hardware.Add(hardware);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added A Hardware Service Category",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + hardware.Id), hardwareDto);
        }

        //GET in form
        [Route("api/hard/gethardware/{id}")]
        [HttpGet]
        public IHttpActionResult GetHardware(int id)
        {
            var hardwares = _db.Hardware.SingleOrDefault(d => d.Id == id);
            if (hardwares == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Hardware, HardwareDto>(hardwares));
        }

        //Put
        [HttpPut]
        [Route("api/hard/updateHardware/{id}")]
        public IHttpActionResult UpdateHardware(int id, HardwareDto hardwareDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hardInDb = _db.Hardware.SingleOrDefault(d => d.Id == id);
            if (hardInDb == null)
            {
                return NotFound();
            }

            Mapper.Map(hardwareDto, hardInDb);
            hardInDb.Id = id;
            hardInDb.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            hardwareDto.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edited A Hardware Service Category",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/hard/Delete/{id}")]
        public IHttpActionResult DeleteHardware(int id)
        {
            var hardwareInDb = _db.Hardware.SingleOrDefault(d => d.Id == id);
            if (hardwareInDb == null)
            {
                return NotFound();
            }
            _db.Hardware.Remove(hardwareInDb);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Developr Delete A Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }
    }
}
