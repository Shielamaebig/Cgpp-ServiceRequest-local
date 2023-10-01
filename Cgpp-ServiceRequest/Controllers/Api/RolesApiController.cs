using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;
using Cgpp_ServiceRequest.Models.Extensions;
using Cgpp_ServiceRequest.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;

namespace Cgpp_ServiceRequest.Controllers
{
    public class RolesApiController : ApiController
    {
        private ApplicationDbContext db;

        public RolesApiController()
        {
            db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
        [HttpGet]
        [Route("api/v1/getusers")]
        public IHttpActionResult GetUserAccount()
        {
            var users = db.Users.Include(x => x.Departments).Include(x => x.Divisions).ToList();
            return Ok(users.Where(x => x.IsUserApproved == false).OrderByDescending(u => u.DateCreated));
        }
        [HttpGet]
        [Route("api/roles/current")]
        public IHttpActionResult GetCurrentUser()
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        // get users
        [HttpGet]
        [Route("api/v1/roles/users")]
        public IHttpActionResult GetUsers()
        {
            var users = db.Users.ToList();
            if (users == null)
            {
                return NotFound();
            }

            // return only name
            var usersViewModel = users.Select(u => new UsersInRoleViewModel
            {
                UserId = u.Id,
                Username = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                FullName = u.FullName,
                MobileNumber = u.MobileNumber,
                DepartmentsId = u.DepartmentsId,
                Departments = u.Departments
            });
            return Ok(usersViewModel);
        }

        [HttpGet]
        [Route("api/user/countuser")]
        public IHttpActionResult GetUserCount()
        {
            var users = db.Users.ToList();
            return Ok(users.Count);
        }

        [HttpGet]
        [Route("api/users/list")]
        public IHttpActionResult ListUsers()
        {
            var users = (from user in db.Users
                         select new
                         {
                             UserId = user.Id,
                             UserName = user.UserName,
                             DateCreated = user.DateCreated,
                             DepartmensId = user.DepartmentsId,
                             DivsionsId = user.DivisionsId,
                             FirstName = user.FirstName,
                             MiddleName = user.MiddleName,
                             LastName = user.LastName,
                             MobileNumber = user.MobileNumber,
                             Email = user.Email,
                             RoleNames = (from userRole in user.Roles join role in db.Roles on userRole.RoleId equals role.Id select role.Name).ToList(),
                         }).ToList().Select(p => new UsersInRoleViewModel()
                         {
                             UserId = p.UserId,
                             Username = p.UserName,
                             DateCreated = DateTime.Now,
                             FirstName = p.FirstName,
                             MiddleName = p.MiddleName,
                             LastName = p.LastName,
                             DepartmentsId = p.DepartmensId,
                             DivisionsId = p.DivsionsId
                         });

            return Ok(users);
        }
       
        [HttpGet]
        [Route("api/v1/users/getid/{id}")]
        public IHttpActionResult GetUserById(string id)
        {
            var user = db.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // get roles with names
        [HttpGet]
        [Route("api/v1/roles/get")]
        public IHttpActionResult GetRoles()
        {
            var roles = db.Roles.ToList();
            return Ok(roles);
        }


        [Route("api/v1/users/GetDepartments")]
        public IHttpActionResult GetDepartments()
        {
            var deps = db.Departments.ToList();
            if (User.IsInRole("DepartmentHead") || User.IsInRole("DivisionHead") || User.IsInRole("DivisionPersonnel"))
            {
                var depId = Convert.ToInt32(User.Identity.GetUserDepartment());
                deps = new List<Departments>(deps.Where(u => u.Id == depId));
            }

            return Ok(deps);
        }

        [HttpGet]
        [Route("api/get/user/division")]
        public IHttpActionResult GetuserByDivision()
        {
            var divName = User.Identity.GetDivisionName();
            var userList = db.Users.Include(x=>x.Divisions).Include(x=>x.Departments).ToList();
            return Ok(userList.Where(x => x.DivisionName == divName));
        }
        [HttpGet]
        [Route("api/get/user/department")]
        public IHttpActionResult GetuserByDepartment()
        {
            var deptName = User.Identity.GetDepartmentName();
            var userList = db.Users.Include(x => x.Divisions).Include(x => x.Departments).ToList();
            return Ok(userList.Where(x => x.DepartmentName == deptName));
        }
    }
}
