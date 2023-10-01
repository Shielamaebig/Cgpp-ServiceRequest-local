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
using Cgpp_ServiceRequest.Models.Extensions;
using Newtonsoft.Json;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class DivisionApiController : ApiController
    {

        private ApplicationDbContext _db;

        public DivisionApiController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

     

        [Route("api/test/divisions/getdivisions")]
        [HttpGet]
        public IHttpActionResult fetchdiv()
        {
            var divsionList = _db.Divisions
                .Include(m => m.Departments)
                .ToList()
                .Select(Mapper.Map<Divisions, DivisionDto>);
            return Ok(divsionList);
        }

        
        [Route("api/div/department")]
        [HttpGet]
        public IHttpActionResult GetDepartment()
        {
            var department = _db.Departments.ToList();
            return Ok(department);
        }

        //[Route("api/div/fetchbyid/{id}")]
        //[HttpGet]
        //public IHttpActionResult FetchDivision(int id)
        //{
        //    var divlist = _db.Divisions.Include(m => m.Departments).Where(d => d.DepartmentsId == id).ToList().Select(Mapper.Map<Divisions, DivisionDto>);
        //    if (divlist == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(divlist);
        //}
        [Route("api/divisions/getdivisions")]
        [HttpGet]
        public IHttpActionResult GetDivisions()
        {
            var divsionList = _db.Divisions
                .Include(m => m.Departments)
                .ToList()
                .Select(Mapper.Map<Divisions, DivisionSpecDto>);
            return Ok(divsionList);
        }
        
        //get id
        [Route("api/divisions/getdivisions/{id}")]
        [HttpGet]
        public IHttpActionResult GetDivisionbyid(int id)
        {
            var division = _db.Divisions.Include(m=>m.Departments).SingleOrDefault(d => d.Id == id);
            if (division == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Divisions, DivisionSpecDto>(division));
        }

        [Route("api/div/fetchbyid/{id}")]
        [HttpGet]
        public IHttpActionResult FetchDivision(int id)
        {
            var divlist = _db.Divisions.Include(m => m.Departments).Where(d => d.DepartmentsId == id).ToList().Select(Mapper.Map<Divisions, DivisionDto>);
            if (divlist == null)
            {
                return NotFound();
            }
            return Ok(divlist);
        }

        [Route("api/div/getdivisions/{id}")]
        [HttpGet]
        public IHttpActionResult GetDivisions(int id)
        {
            var division = _db.Divisions.SingleOrDefault(d => d.Id == id);
            if (division == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Divisions, DivisionDto>(division));
        }

        // Post
        [HttpPost]
        [Route("api/division/save")]
        public IHttpActionResult CreateDivision(DivisionDto divisionDto)
        {


            var division = Mapper.Map<DivisionDto, Divisions>(divisionDto);
            //doesnt save with same name

            division.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            Divisions div = new Divisions();
            div.DepartmentsId = divisionDto.DepartmentsId;
            divisionDto.Id = division.Id;

            _db.Divisions.Add(division);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added A Division",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + division.Id), divisionDto);
        }


        [HttpPut]
        [Route("api/division/updateDiv/{id}")]
        public IHttpActionResult UpdateDivision(int id, DivisionDto divisionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var divInDb = _db.Divisions.SingleOrDefault(d => d.Id == id);
            if (divInDb == null)
            {
                return NotFound();
            }
            Mapper.Map(divisionDto, divInDb);
            divInDb.Id = id;
            divisionDto.Id = id;
            divInDb.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            divisionDto.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edited A Division",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/division/Delete/{id}")]
        public IHttpActionResult DeleteDivision(int id)
        {
            var divisionInDb = _db.Divisions.SingleOrDefault(d => d.Id == id);
            if (divisionInDb == null)
            {
                return NotFound();
            }
            _db.Divisions.Remove(divisionInDb);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted A Division",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }
    }
}
