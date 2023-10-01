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
using System.Security.Cryptography;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class LoginActivityApiController : ApiController
    {
        private ApplicationDbContext _db;

        IlsContext Db = new IlsContext();

        public LoginActivityApiController()
        {
            _db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        [HttpGet]
        [Route("api/drafts/getdraft")]
        public IHttpActionResult GetDrafts()
        {
            var newLIst = Db.Drafts.ToList();
            return Ok(newLIst.Take(10));
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
            return Ok(activity.OrderByDescending(x => x.Id).Take(8));
        }

        [Route("api/RequestActivity/get")]
        public IHttpActionResult GetRequestAcitivty()
        {
            var ra = _db.RequestHistory
                .Include(x => x.Departments)
                .Include(x => x.Divisions)
                .ToList().Select(Mapper.Map<RequestHistory, RequestHistoryDto>);
            return Ok(ra.OrderByDescending(x => x.Id).Take(8));
        }

        // GET
        [Route("api/Dashacitivy/get")]
        public IHttpActionResult GetDashRecentActivity()
        {
            var activity = _db.LoginActivity.ToList().Select(Mapper.Map<LoginActivity, LoginActivityDto>);
            return Ok(activity.OrderByDescending(x => x.Id).Take(8));
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

        [Route("api/users/GetHardwareReqAdmin")]
        public IHttpActionResult GetlogHardtReq()
        {
            var hreq = _db.RequestHistory
            .Include(x => x.Departments)
            .Include(x => x.Divisions)
            .Include(x => x.HardwareUserRequest)
            .ToList().Select(Mapper.Map<RequestHistory, RequestHistoryDto>);

            if (User.IsInRole("HardwareAdmin"))
            {
                //var HardwareCategory = "1";
                hreq = new List<RequestHistoryDto>(hreq.Where(x => x.Category == false));

            }
            return Ok(hreq.OrderByDescending(x => x.Id).Take(8));
        }

        [Route("api/users/GetSoftwareReqAdmin")]
        public IHttpActionResult GetlogSoftReq()
        {
            var sreq = _db.SoftwareUserRequests
            .Include(x => x.Departments)
            .Include(x => x.Divisions)
            .ToList().Select(Mapper.Map<SoftwareUserRequest, SoftwareUserRequestDto>);
            return Ok(sreq.OrderByDescending(x => x.Id).Take(8));
        }



        //[HttpGet]
        //[Route("api/v2/sr/assign")]
        //public IHttpActionResult GetDashProgrammers()
        //{
        //    var progDto = _db.ProgrammerReport
        //        .Include(x => x.Software)
        //        .Include(x => x.SoftwareUserRequest)
        //        .ToList().Select(Mapper.Map<ProgrammerReport, ProgrammerReportDto>);
        //    if (User.IsInRole("Programmers") || User.IsInRole("Technicians/Programmer"))
        //    {
        //        var progEmail = User.Identity.GetUserEmail();
        //        progDto = new List<ProgrammerReportDto>(progDto.Where(x => x.ProgrammerEmail == progEmail));
        //    }
        //    return Ok(progDto.OrderByDescending(x => x.Id).Take(5));
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
            return Ok(user.FirstName);
        }









        //Count per technician Assigned
        [Route("api/hardware/assignbytech")]
        public IHttpActionResult GetCountProgrammer()
        {
            var techDto = _db.TechnicianReports.ToList();
            if (User.IsInRole("Technicians") || User.IsInRole("Technicians/Programmer"))
            {
                var techEmail = User.Identity.GetUserEmail();
                techDto = new List<TechnicianReport>(techDto.Where(x => x.TechEmail == techEmail));
            }
            return Ok(techDto.Count);
        }



        [Route("api/software/assignbytech")]
        public IHttpActionResult GetCountTechnician()
        {
            var progDto = _db.ProgrammerReport.ToList();
            if (User.IsInRole("Programmers") || User.IsInRole("Technicians/Programmer"))
            {
                var progEmail = User.Identity.GetUserEmail();
                progDto = new List<ProgrammerReport>(progDto.Where(x => x.ProgrammerEmail == progEmail));
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
        [Route("api/software/DivisionUser/count")]
        public IHttpActionResult GetDivisionUsers()
        {
            var divId = User.Identity.GetDivisionName();
            var userList = _db.Users.Include(x => x.Divisions).Include(x => x.Departments).ToList();
            return Ok(userList.Where(x => x.DivisionName == divId).Count());
        }
        [Route("api/software/allReq/count")]
        public IHttpActionResult GetAllReq()
        {
            var userDivision = User.Identity.GetDivisionName();
            var allReq = _db.SoftwareUserRequests.Include(x => x.Divisions).Include(x => x.Departments).ToList();
            return Ok(allReq.Where(x => x.DivisionName == userDivision).Count());
        }
        [Route("api/software/NewReq/count")]
        public IHttpActionResult GetDivApprover()
        {
            var accept = "Accept";
            var updateReq = "Update Request";
            var newReq = _db.SoftwareUserRequests.ToList();
            //var userList = _db.Users.Include(x => x.Divisions).Include(x => x.Departments).ToList();
            return Ok(newReq.Where(x => x.Status == accept || x.Status == updateReq).Count());
        }
        [Route("api/login/userActivity")]
        public IHttpActionResult GetUserActivity()
        {
            var loginDto = _db.LoginActivity.ToList();
            if (User.IsInRole("Users") || User.IsInRole("Programmers") || User.IsInRole("Technicians") || User.IsInRole("HardwareAdmin") || User.IsInRole("SoftwareAdmin") || User.IsInRole("SuperAdmin") || User.IsInRole("Developer") || User.IsInRole("DivisionApprover"))
            {
                var userEmail = User.Identity.GetUserEmail();
                loginDto = new List<LoginActivity>(loginDto.Where(x => x.Email == userEmail));
            }
            return Ok(loginDto.OrderByDescending(x => x.Id));
        }
        [Route("api/login/userActivity2")]
        public IHttpActionResult GetUserActivity2()
        {
            var loginDto = _db.LoginActivity.ToList();
            if (User.IsInRole("Users") || User.IsInRole("Programmers") || User.IsInRole("Technicians") || User.IsInRole("SoftwareAdmin") || User.IsInRole("HardwareAdmin") || User.IsInRole("SuperAdmin") || User.IsInRole("Developer") || User.IsInRole("DivisionApprover") || User.IsInRole("Technicians/Programmer") || User.IsInRole("Programmer/Admin"))
            {
                var userEmail = User.Identity.GetUserEmail();
                var logEmail = User.Identity.GetLogEmail();
                loginDto = new List<LoginActivity>(loginDto.Where(x => x.Email == userEmail || x.Email == logEmail).OrderByDescending(x => x.Id));
            }
            return Ok(loginDto.Take(10));
        }
        [Route("api/login/userActivity3")]
        public IHttpActionResult GetUserActivity3()
        {
            var loginDto = _db.LoginActivity.ToList();
            if (User.IsInRole("SoftwareAdmin"))
            {
                var userEmail = User.Identity.GetFullName();
                loginDto = new List<LoginActivity>(loginDto.Where(x => x.UserName == userEmail).OrderByDescending(x => x.Id));
            }
            return Ok(loginDto.Take(10));
        }
        [Route("api/UserRegistration/register")]
        public IHttpActionResult GetUserReg()
        {
            var user = _db.UserRegistrations.ToList().OrderByDescending(x => x.Id);
            return Ok(user);
        }


        //Post user Registration
        [HttpPost]
        [Route("api/userRegistration/save")]
        public IHttpActionResult SaveCred(UserRegistrationDto userRegistrationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = Mapper.Map<UserRegistrationDto, UserRegistration>(userRegistrationDto);
            var devNumber = "09165778160";
            //department.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            //if (_ctx.Ftp.Any(x => x.FtpHost == ftpcreds.FtpHost || x.FtpHost == null))
            //{
            //    return BadRequest("Host name already Exist");
            //}
            UserRegistration userReg = new UserRegistration();

            userRegistrationDto.Id = user.Id;
            userRegistrationDto.FirstName = userReg.FirstName;
            userRegistrationDto.LastName = userReg.LastName;
            userRegistrationDto.MiddleName = userReg.MiddleName;
            userRegistrationDto.DivisionName = userReg.DivisionName;
            userRegistrationDto.DepartmentName = userReg.DepartmentName;
            userRegistrationDto.Email = userReg.Email;
            userRegistrationDto.MobileNumber = userReg.MobileNumber;
            userRegistrationDto.DateCreated = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            user.DateCreated = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            _db.UserRegistrations.Add(user);

            Db.Drafts.Add(new Draft
            {
                Sendto = devNumber,
                msg = "New User Account Request",
                tag = 0
            });
            Db.SaveChanges();

            _db.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + userReg.Id), userRegistrationDto);
        }

        [HttpGet]
        [Route("api/newusers/get")]
        public IHttpActionResult GetUserRegistration()
        {
            var users = _db.UserRegistrations.ToList();
            return Ok(users.Where(x => x.IsNewAccount == false).OrderByDescending(u => u.Id));
        }

        [HttpGet]
        [Route("api/useraccount/get/{id}")]
        public IHttpActionResult GetuserAccount(int id)
        {
            var users = _db.UserRegistrations.SingleOrDefault(x => x.Id == id);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<UserRegistration, UserRegistrationDto>(users));
        }

        //Put
        [HttpPut]
        [Route("api/v2/useraccount/{id}")]
        public IHttpActionResult updateUser(int id, UserRegistrationDto userRegistrationDto)
        {
            var usersInDb = _db.UserRegistrations.SingleOrDefault(x => x.Id == id);
            usersInDb.Id = userRegistrationDto.Id;
            usersInDb.IsNewAccount = true;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Check User Request Account",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/userAccount/Delete/{id}")]
        public IHttpActionResult DeleteRequest(int id)
        {
            var userInDb = _db.UserRegistrations.SingleOrDefault(d => d.Id == id);
            if (userInDb == null)
            {
                return NotFound();
            }
            _db.UserRegistrations.Remove(userInDb);
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

        [Route("api/UserForgotPass/passemail")]
        public IHttpActionResult Getuserdetails()
        {
            var user = _db.UserForgotPasswords.ToList().Select(Mapper.Map<UserForgotPassword, UserForgotPasswordsDto>);
            return Ok(user.OrderByDescending(x => x.Id));
        }
        //[HttpGet]
        //[Route("api/AccountDept/get")]
        //public IHttpActionResult GetDept()
        //{
        //    var users = _db.Users.ToList();
        //    return Ok(users.Count());
        //}

        [HttpGet]
        [Route("api/AccountDept/get")]
        public IHttpActionResult GetDeptUsers()
        {
            var result = _db.Users.GroupBy(r => new { r.DepartmentName })
                            .Where(grp => grp.Count() > 0)
                            .Select(g => new { g.Key.DepartmentName, Count = g.Count() })
                            .ToList();
            return Ok(result);
        }
        //Notification
        [HttpGet]
        [Route("api/forgotPass/get")]
        public IHttpActionResult GetUserForgotPass()
        {
            var users = _db.UserForgotPasswords.ToList();
            return Ok(users.Where(x => x.IsRequest == false).OrderByDescending(u => u.Id));
        }

        [HttpGet]
        [Route("api/NewDivRequest/get")]
        public IHttpActionResult GetNewDivisionRequest()
        {
            var users = _db.SoftwareUserRequests.ToList();
            return Ok(users.Where(x => x.IsNew == true).OrderByDescending(u => u.Id));
        }

        ////Post user Registration
        //[HttpPost]
        //[Route("api/userForgotPassword/save")]
        //public IHttpActionResult SaveForgotPassword(UserForgotPasswordsDto userForgotPasswordsDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var userfp = Mapper.Map<UserForgotPasswordsDto, UserForgotPassword>(userForgotPasswordsDto);
        //    var devNumber = "09165778160";

        //    //department.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
        //    userfp.DateCreated = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
        //    UserForgotPassword userforgot = new UserForgotPassword();

        //    userForgotPasswordsDto.Id = userfp.Id;
        //    userForgotPasswordsDto.DivisionName = userforgot.DivisionName;
        //    userForgotPasswordsDto.DepartmentName = userforgot.DepartmentName;
        //    userForgotPasswordsDto.MobileNumber = userforgot.MobileNumber;
        //    userForgotPasswordsDto.DateCreated = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

        //    _db.UserForgotPasswords.Add(userfp);

        //    Db.Drafts.Add(new Draft
        //    {
        //        Sendto = devNumber,
        //        msg = "New Forgot Password Request",
        //        tag = 0
        //    });
        //    Db.SaveChanges();

        //    _db.SaveChanges();

        //    return Created(new Uri(Request.RequestUri + "/" + userforgot.Id), userForgotPasswordsDto);
        //}


        ////Post user Registration
        [HttpPost]
        [Route("api/userForgotPassword/save")]
        public IHttpActionResult SaveCred(UserForgotPasswordsDto userForgotPasswordsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = Mapper.Map<UserForgotPasswordsDto, UserForgotPassword>(userForgotPasswordsDto);
            var devNumber = "09165778160";
            //department.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            //if (_ctx.Ftp.Any(x => x.FtpHost == ftpcreds.FtpHost || x.FtpHost == null))
            //{
            //    return BadRequest("Host name already Exist");
            //}
            UserForgotPassword userReg = new UserForgotPassword();

            userForgotPasswordsDto.Id = user.Id;
            userForgotPasswordsDto.FirstName = userReg.FirstName;
            userForgotPasswordsDto.LastName = userReg.LastName;
            userForgotPasswordsDto.MiddleName = userReg.MiddleName;
            userForgotPasswordsDto.DivisionName = userReg.DivisionName;
            userForgotPasswordsDto.DepartmentName = userReg.DepartmentName;
            userForgotPasswordsDto.MobileNumber = userReg.MobileNumber;
            userForgotPasswordsDto.DateCreated = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            user.DateCreated = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            _db.UserForgotPasswords.Add(user);

            Db.Drafts.Add(new Draft
            {
                Sendto = devNumber,
                msg = "New User Account Request",
                tag = 0
            });
            Db.SaveChanges();

            _db.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + userReg.Id), userForgotPasswordsDto);
        }

        [HttpGet]
        [Route("api/userforgot/get/{id}")]
        public IHttpActionResult GetuserbyIdForgot(int id)
        {
            var users = _db.UserForgotPasswords.SingleOrDefault(x => x.Id == id);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<UserForgotPassword, UserForgotPasswordsDto>(users));
        }

        //Put
        [HttpPut]
        [Route("api/v2/userforgot/{id}")]
        public IHttpActionResult updateUserForgot(int id, UserForgotPasswordsDto userForgotPasswordsDto)
        {
            var usersInDb = _db.UserForgotPasswords.SingleOrDefault(x => x.Id == id);
            usersInDb.Id = userForgotPasswordsDto.Id;
            usersInDb.IsRequest = true;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Check User Request Account",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/forgot/Delete/{id}")]
        public IHttpActionResult DeleteForgot(int id)
        {
            var userInDb = _db.UserForgotPasswords.SingleOrDefault(d => d.Id == id);
            if (userInDb == null)
            {
                return NotFound();
            }
            _db.UserForgotPasswords.Remove(userInDb);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted A Forgot Password Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/addtech/save")]
        public IHttpActionResult SaveTechnician(HardwareTechnicianDto hardwareTechnicianDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var hardwareTech = Mapper.Map<HardwareTechnicianDto, HardwareTechnician>(hardwareTechnicianDto);

            hardwareTech.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            SoftwareTechnician stech = new SoftwareTechnician();
            hardwareTechnicianDto.Id = hardwareTech.Id;
            hardwareTechnicianDto.Name = hardwareTech.Name;
            hardwareTechnicianDto.Email = hardwareTech.Email;
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
    }
}
