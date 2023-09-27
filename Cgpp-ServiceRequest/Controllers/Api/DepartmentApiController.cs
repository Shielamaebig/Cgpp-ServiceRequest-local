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
    public class DepartmentApiController : ApiController
    {
        private ApplicationDbContext _db;

        public DepartmentApiController()
        { 
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET
        [Route("api/dept/get")]
        public IHttpActionResult GetDepartments()
        {
            var departments = _db.Departments.Include(x=>x.Ftp).ToList();
            return Ok(departments.OrderByDescending(x=>x.DateAdded));
        }

        [HttpPost]
        [Route("api/dept/save")]
        public IHttpActionResult SaveDept (DepartmentDto departmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var department = Mapper.Map<DepartmentDto, Departments>(departmentDto);

            if (_db.Departments.Any(d => d.Name == department.Name || d.Name == null))
            {
                return BadRequest("Department name already Exist");
            }
            department.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            Departments dept = new Departments();

            departmentDto.Id = department.Id;
            _db.Departments.Add(department);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added A Department",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),

            });
            _db.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + department.Id), departmentDto);
        }

        //GET in form
        [Route("api/dept/getdepartment/{id}")]
        [HttpGet]
        public IHttpActionResult GetDept(int id)
        {
            var departments = _db.Departments.Include(x=>x.Ftp).SingleOrDefault(d => d.Id == id);
            if (departments == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Departments, DepartmentDto>(departments));
        }


        //Put
        [HttpPut]
        [Route("api/dept/updatedept/{id}")]
        public IHttpActionResult UpdateDept(int id, DepartmentDto departmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deptInDb = _db.Departments.SingleOrDefault(d => d.Id == id);
            if (deptInDb == null)
            {
                return NotFound();
            }
            Mapper.Map(departmentDto, deptInDb);
            deptInDb.Id = id;
            deptInDb.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            departmentDto.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            departmentDto.IsDepartmentApprover = departmentDto.IsDepartmentApprover;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edited A Department",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/department/Delete/{id}")]
        public IHttpActionResult DeleteDepartment(int id)
        {
            var departmentInDb = _db.Departments.SingleOrDefault(d => d.Id == id);
            if(departmentInDb == null)
            {
                return NotFound();
            }
            _db.Departments.Remove(departmentInDb);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted A Department",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/department/countDept")]
        public IHttpActionResult GetCountDept()
        {
            var departmentDto = _db.Departments.ToList();
            return Ok(departmentDto.Count);
        }
    }
}
