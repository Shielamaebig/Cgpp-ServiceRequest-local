using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;
using Cgpp_ServiceRequest.Models.Extensions;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class LoginActivityApiController : ApiController
    {
        private ApplicationDbContext _db;
        

        public LoginActivityApiController()
        {
            _db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        //get current Logged user
        [HttpGet]
        [Route("api/login/current")]
        public IHttpActionResult GetCurrentUser()
        {
            var user = _db.Users.Find(User.Identity.GetUserId());

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET
        [Route("api/acitivi/get")]
        public IHttpActionResult GetRecentActivity()
        {
            var activity = _db.LoginActivity.ToList().Select(Mapper.Map<LoginActivity, LoginActivityDto>);
            return Ok(activity.OrderByDescending(x=>x.Id).Take(8));
        }

        [Route("api/RequestActivity/get")]
        public IHttpActionResult GetRequestAcitivty()
        {
            var ra = _db.RequestHistory
                .Include(x=>x.Departments)
                .Include(x=>x.Divisions)
                .ToList().Select(Mapper.Map<RequestHistory, RequestHistoryDto>);
            return Ok(ra.OrderByDescending(x => x.Id).Take(8));
        }

        // GET
        [Route("api/Dashacitivy/get")]
        public IHttpActionResult GetDashRecentActivity()
        {
            var activity = _db.LoginActivity.ToList().Select(Mapper.Map<LoginActivity, LoginActivityDto>);
            return Ok(activity.OrderByDescending(x=>x.Id).Take(8));
        }

        [Route("api/DashRequest/get")]
        public IHttpActionResult GetDashRequestAcitivty()
        {
            var ra = _db.RequestHistory
                .Include(x => x.Departments)
                .Include(x => x.Divisions)
                .ToList().Select(Mapper.Map<RequestHistory, RequestHistoryDto>);
            return Ok(ra.OrderByDescending(x => x.Id).Take(8));
        }

        //[Route("api/users/GetUserImage")]
        //public IHttpActionResult GetUserImage()
        //{
        //    var user = _db.Users.Find(User.Identity.GetUserId());
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user.ImagePath);
        //}

        [Route("api/users/GetUserImage")]
        public IHttpActionResult GetUserImage()
        {
            var user = _db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ImagePath);
        }

        //Get Department
        [Route("api/User/department")]
        [HttpGet]
        public IHttpActionResult GetDepartment()
        {
            var department = _db.Departments.ToList();
            return Ok(department);
        }

        //get Division
        [Route("api/User/division")]
        [HttpGet]
        public IHttpActionResult GetDivision()
        {
            var division = _db.Divisions.ToList();
            return Ok(division);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/users/getid/{id}")]
        public IHttpActionResult GetUserId(string id)
        {
            var user = _db.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Route("api/users/GetUserFullName")]
        public IHttpActionResult GetUserFullName()
        {
            var user = _db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.FullName);
        }

        [Route("api/SoftwareActivity/get")]
        public IHttpActionResult GetsoftwareActivty()
        {
            var Softwareactivity = _db.SoftwareRequestHistory
                .Include(x=>x.Departments)
                .Include(x=>x.Divisions)
                .ToList().Select(Mapper.Map<SoftwareRequestHistory, SoftwareRequestHistoryDto>);
            return Ok(Softwareactivity.OrderByDescending(x => x.Id).Take(8));
        }

        [Route("api/v2/SoftwareActivity/get")]
        public IHttpActionResult GetsoftwareActivtyList()
        {
            var Softwareactivity = _db.SoftwareRequestHistory.Include(x=>x.Departments).Include(x=>x.Divisions).ToList().Select(Mapper.Map<SoftwareRequestHistory, SoftwareRequestHistoryDto>);
            return Ok(Softwareactivity.OrderByDescending(x => x.Id));
        }

        [Route("api/HardwareActivity/get")]
        public IHttpActionResult GethardwareActivty()
        {
            var HardwareActivity = _db.HardwareRequestHistoty
                .Include(x=>x.Departments)
                .Include(x=>x.Divisions).ToList().Select(Mapper.Map<HardwareRequestHistory, HardwareRequestHistoryDto>);
            return Ok(HardwareActivity.OrderByDescending(x => x.Id).Take(8));
        }
        [Route("api/v2/HardwareActivity/get")]
        public IHttpActionResult GethardwareActivtyList()
        {
            var HardwareActivity = _db.HardwareRequestHistoty
                .Include(x=>x.Departments)
                .Include(x=>x.Divisions).ToList().Select(Mapper.Map<HardwareRequestHistory, HardwareRequestHistoryDto>);
            return Ok(HardwareActivity.OrderByDescending(x => x.Id));
        }


        //Count per technician Assigned
        [Route("api/hardware/assignbytech")]
        public IHttpActionResult GetCountProgrammer()
        {
            var techDto = _db.HardwareRequest.ToList();
            if (User.IsInRole("Technicians"))
            {
                var techEmail = User.Identity.GetUserEmail();
                techDto = new List<HardwareRequest>(techDto.Where(x => x.TechEmail == techEmail));
            }
            return Ok(techDto.Count);
        }

        [Route("api/software/assignbytech")]
        public IHttpActionResult GetCountTechnician()
        {
            var progDto = _db.SoftwareRequest.ToList();
            if (User.IsInRole("Programmers"))
            {
                var progEmail = User.Identity.GetUserEmail();
                progDto = new List<SoftwareRequest>(progDto.Where(x=>x.ProEmail == progEmail));
            }
            return Ok(progDto.Count);
        }

        [Route("api/hardwareTechnician/count")]
        public IHttpActionResult GetHardwareTechCount()
        {
            var hardwareTech = _db.HardwareTechnician.ToList();
            return Ok(hardwareTech.Count);
        }

        [Route("api/softwareTechnician/count")]
        public IHttpActionResult GetProgrammers()
        {
            var softwareTech = _db.SoftwareTechnician.ToList();
            return Ok(softwareTech.Count);
        }
        [Route("api/login/userActivity")]
        public IHttpActionResult GetUserActivity()
        {
            var loginDto = _db.LoginActivity.ToList();
            if (User.IsInRole("Users") || User.IsInRole("Programmers") || User.IsInRole("Technicians") || User.IsInRole("HardwareAdmin") || User.IsInRole("SoftwareAdmin") || User.IsInRole("SuperAdmin") || User.IsInRole("Developer"))
            {
                var userEmail = User.Identity.GetUserEmail();
                loginDto = new List<LoginActivity>(loginDto.Where(x => x.Email == userEmail));
            }
            return Ok(loginDto.OrderByDescending(x=>x.Id));
        }
        [Route("api/login/userActivity2")]
        public IHttpActionResult GetUserActivity2()
        {
            var loginDto = _db.LoginActivity.ToList();
            if (User.IsInRole("Users") || User.IsInRole("Programmers") || User.IsInRole("Technicians") || User.IsInRole("HardwareAdmin") || User.IsInRole("SoftwareAdmin") || User.IsInRole("SuperAdmin") || User.IsInRole("Developer"))
            {
                var userEmail = User.Identity.GetUserEmail();
                loginDto = new List<LoginActivity>(loginDto.Where(x => x.Email == userEmail).OrderByDescending(x=>x.Id));
            }
            return Ok(loginDto.Take(10));
        }
    }
}
