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
    public class UnitTypesApiController : ApiController
    {
        private ApplicationDbContext _db;

        public UnitTypesApiController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        // GET
        [Route("api/unit/get")]
        [HttpGet]
        public IHttpActionResult GetUnitType()
        {
            var unitList = _db.UnitType.ToList().Select(Mapper.Map<UnitType, UnitTypeDto>);
            return Ok(unitList);
        }

        //Get edit
        //GET in form
        [Route("api/unit/getTechbyId/{id}")]
        [HttpGet]
        public IHttpActionResult GetUnitTypes(int id)
        {
            var unit = _db.UnitType.SingleOrDefault(d => d.Id == id);
            if (unit == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<UnitType, UnitTypeDto>(unit));
        }

        //Save
        [HttpPost]
        [Route("api/unit/save")]
        public IHttpActionResult SaveUnit(UnitTypeDto unitTypeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var unitTypes = Mapper.Map<UnitTypeDto, UnitType>(unitTypeDto);

            if (_db.UnitType.Any(d => d.Name == unitTypes.Name || d.Name == null))
            {
                return BadRequest("unit name already Exist");
            }

            unitTypes.DateAdded = DateTime.Now.ToString("MMMM dd yyyyy hh:mm tt");
            UnitType unit = new UnitType();

            unitTypeDto.Id = unitTypes.Id;
            _db.UnitType.Add(unitTypes);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added A Unit type",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + unitTypes.Id), unitTypeDto);
        }


        [Route("api/unit/get/{id}")]
        public IHttpActionResult GetUnitType(int id)
        {
            var units = _db.UnitType.Where(m => m.Id == id).FirstOrDefault();
            return Ok(units);
        }

        //update
        [HttpPut]
        [Route("api/unit/updateUnit/{id}")]
        public IHttpActionResult UpdateUnitType(int id, UnitTypeDto unitTypeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unitTypes = _db.UnitType.SingleOrDefault(d => d.Id == id);
            if (unitTypes == null)
            {
                return NotFound();
            }
            Mapper.Map(unitTypeDto, unitTypes);
            unitTypes.Id = id;
            unitTypes.DateAdded = DateTime.Now.ToString("MMMM dd yyyyy hh:mm tt");
            unitTypeDto.DateAdded = DateTime.Now.ToString("MMMM dd yyyyy hh:mm tt");
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Updated A Unit type",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }


        [HttpDelete]
        [Route("api/unit/Delete/{id}")]
        public IHttpActionResult DeleteUnit(int id)
        {
            var unitTypes = _db.UnitType.SingleOrDefault(d => d.Id == id);
            if (unitTypes == null)
            {
                return NotFound();
            }
            _db.UnitType.Remove(unitTypes);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted A Unit type",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        //[HttpGet]
        //[Route("api/get/requestv2")]
        //public IHttpActionResult GetUserRequest()
        //{
        //    var req = _db.HardwareUserRequests.Include(x => x.TechnicianReport).ToList()
        //        .Select(Mapper.Map<HardwareUserRequest, HardwareUserRequestDto>);

        //    return Ok(req);
        //}
    }
}
