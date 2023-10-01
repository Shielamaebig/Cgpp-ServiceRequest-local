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
using System.Threading.Tasks;
using System.Web;
using System.IO;
using FluentFTP;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class HardwareRequestApiController : ApiController
    {
        private ApplicationDbContext _db;

        public HardwareRequestApiController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        [Route("api/v2/getuseremail")]
        public IHttpActionResult GetUserEmail()
        {
            var progEmail = User.Identity.GetUserEmail();
            return Ok(progEmail);
        }
        [Route("api/v2/hr/get")]
        public IHttpActionResult GetRequestByEmail()
        {
            var hrDto = _db.HardwareRequest
                .Include(x => x.Hardware)
                .ToList().Select(Mapper.Map<HardwareRequest, HardwareRequestSpecDto>);

            if (User.IsInRole("Users"))
            {
                var uEmail = User.Identity.GetUserEmail();
                hrDto = new List<HardwareRequestSpecDto>(hrDto.Where(x => x.Email == uEmail));
            }

            return Ok(hrDto.OrderByDescending(u => u.Id));
        }

        [HttpGet]
        [Route("api/hr/assign")]
        public IHttpActionResult GetByTech()
        {
            var techDto = _db.HardwareRequest
                .Include(x => x.Hardware)
                .ToList().Select(Mapper.Map<HardwareRequest, HardwareRequestSpecDto>);
            if (User.IsInRole("Technicians"))
            {
                var uemail = User.Identity.GetUserEmail();
                techDto = new List<HardwareRequestSpecDto>(techDto.Where(x => x.TechEmail == uemail));
            }
            return Ok(techDto);
        }

        [HttpGet]
        [Route("api/v2/hr/assign")]
        public IHttpActionResult GetByTechList()
        {
            var techDto = _db.HardwareRequest
                .Include(x => x.Hardware)
                .Include(x => x.Departments)
                .Include(x => x.Divisions)
                .Include(x => x.Status)
                .ToList().Select(Mapper.Map<HardwareRequest, HardwareRequestSpecDto>);
            if (User.IsInRole("Technicians"))
            {
                var uemail = User.Identity.GetUserEmail();
                techDto = new List<HardwareRequestSpecDto>(techDto.Where(x => x.TechEmail == uemail));
            }
            return Ok(techDto.OrderByDescending(x => x.Id).Take(5));
        }

        //GET
        [Route("api/hardwareRequest/GetRequest")]
        [HttpGet]
        public IHttpActionResult GetHardwareRequest()
        {
            var hardwarRequest = _db.HardwareRequest
                .Include(m => m.HardwareTechnician)
                .ToList()
                .Select(Mapper.Map<HardwareRequest, HardwareRequestSpecDto>);

            return Ok(hardwarRequest);
        }
        [Route("api/v2/hardwareRequest/GetDash")]
        public IHttpActionResult GetHardwareReq()
        {
            var hr = _db.HardwareRequest
                .Include(x => x.Hardware)
                .Include(x => x.HardwareTechnician)
                .Include(x => x.Finding)
                .Include(x => x.Departments)
                .ToList().Select(Mapper.Map<HardwareRequest, HardwareRequestSpecDto>);

            //return Ok(hr.OrderByDescending(u=>u.DateAdded));
            return Ok(hr.OrderByDescending(u => u.Ticket).Take(8));
        }

        [Route("api/v2/hardwareRequest/GetRequest")]
        public IHttpActionResult GetRequestUser()
        {
            var hr = _db.HardwareRequest
                .Include(x => x.Hardware)
                .Include(x => x.HardwareTechnician)
                .Include(x => x.Finding)
                .ToList().Select(Mapper.Map<HardwareRequest, HardwareRequestSpecDto>);

            //return Ok(hr.OrderByDescending(u=>u.DateAdded));
            return Ok(hr.OrderByDescending(u => u.Id));
        }


        [HttpGet]
        [Route("api/hardwareRequest/GetRequestbyId/{id}")]
        public IHttpActionResult GetRequestbyId(int id)
        {
            var hardwareRequestList = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);
            if (hardwareRequestList == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<HardwareRequest, HardwareRequestDto>(hardwareRequestList));
        }

        [Route("api/v1/GetRequest")]
        [HttpGet]
        public IHttpActionResult GetHardwareRequest2()
        {
            var hardwarRequest = _db.HardwareRequest.ToList();
            return Ok(hardwarRequest);
        }

        //Get Department
        [Route("api/hardwareRequest/department")]
        [HttpGet]
        public IHttpActionResult GetDepartment()
        {
            var department = _db.Departments.ToList().OrderByDescending(x=>x.Id);
            return Ok(department);
        }
        //Get UnitType
        [Route("api/hardwareRequest/unitType")]
        [HttpGet]
        public IHttpActionResult GetUnitType()
        {
            var unitType = _db.UnitType.ToList();
            return Ok(unitType);
        }
        //get Division
        [Route("api/hardwareRequest/division")]
        [HttpGet]
        public IHttpActionResult GetDivision()
        {
            var division = _db.Divisions.ToList();
            return Ok(division);
        }

        [Route("api/hardwareRequest/Technician")]
        [HttpGet]
        public IHttpActionResult GetTechnician()
        {
            var hardwareTechnician = _db.HardwareTechnician.ToList();
            return Ok(hardwareTechnician);
        }

        [Route("api/hardwareRequest/Findings")]
        [HttpGet]
        public IHttpActionResult GetFinding()
        {
            var findings = _db.Finding.ToList();
            return Ok(findings);
        }

        [Route("api/hardwareRequest/hardware")]
        [HttpGet]
        public IHttpActionResult GetHardware()
        {
            var hardware = _db.Hardware.ToList();
            return Ok(hardware);
        }

        [Route("api/hardwareRequest/status")]
        [HttpGet]
        public IHttpActionResult GetStatus()
        {
            var status = _db.Status.ToList();
            return Ok(status);
        }

        //Post
        [HttpPost]
        [Route("api/hardwareRequest/SaveRequest")]
        public async Task<string> SaveUpload()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/HardwareImage/");


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);

            var provider = new MultipartFormDataStreamProvider(root);
            var ftpAddress = "172.16.100.249";
            var ftpUserName = "cgpp-SR";
            var ftpPassword = "T]ZXm84q";

            FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
            ftpClient.Port = 21;
            ftpClient.Connect();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                HardwareRequest hardwareR = new HardwareRequest();

                if (hardwareR.Id == 0)
                {
                    hardwareR.DocumentLabel = provider.FormData["FileName"];
                    hardwareR.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    hardwareR.DateCreated = DateTime.Now;
                    hardwareR.FullName = User.Identity.GetFullName();
                    hardwareR.Email = User.Identity.GetUserEmail();
                    hardwareR.MobileNumber = User.Identity.GetUserMobileNumber();
                    hardwareR.DepartmentsId = depId;
                    hardwareR.DivisionsId = divId;
                    hardwareR.Description = provider.FormData["description"];
                    hardwareR.UnitTypeId = Convert.ToInt32(provider.FormData["unitTypeId"]);
                    hardwareR.BrandName = provider.FormData["brandName"];
                    hardwareR.ModelName = provider.FormData["modelName"];
                    hardwareR.HardwareId = Convert.ToInt32(provider.FormData["hardwareId"]);
                    hardwareR.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;
                    hardwareR.StatusId = 1;
                    hardwareR.IsNew = true;
                    _db.HardwareRequest.Add(hardwareR);

                    _db.RequestHistory.Add(new RequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        UploadMessage = "Added a new Hardware Request",
                        UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.HardwareRequestHistoty.Add(new HardwareRequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        RequetMessage = "Added a new Hardware Request",
                        RequetDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a new Hardware Request",
                        ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                    });
                }

                _db.SaveChanges();

                foreach (var file in provider.FileData)
                {
                    foreach (var key in provider.FormData.AllKeys)
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {

                            if (key == "FileName")
                            {
                                var name = file.Headers.ContentDisposition.FileName;

                                // remove double quotes from string.
                                var dateNew = Convert.ToString(DateTime.Now.Ticks) + "-";
                                name = name.Trim('"');

                                var localFileName = file.LocalFileName;
                                var filePath = Path.Combine(root, dateNew + name);

                                ftpClient.UploadFile(localFileName, "/CGPP-SR/" + dateNew + name);

                                File.Delete(localFileName);

                                HardwareUploads hardwareUploads = new HardwareUploads();
                                hardwareUploads.FileName = name;
                                hardwareUploads.ImagePath = "/CGPP-SR/" + dateNew + name;
                                hardwareUploads.HardwareRequestId = hardwareR.Id;
                                hardwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                _db.HardwareUploads.Add(hardwareUploads);

                                _db.SaveChanges();
                            }

                        }
                    }
                }
            }

            catch (Exception e)
            {
                return $"Error: {e.Message}";
            }
            ftpClient.Disconnect();
            return "File uploaded!";
        }


        [HttpPut]
        [Route("api/hardwareRequest/updateStatus/{id}")]
        public IHttpActionResult UpateStatus(int id, HardwareRequestDto hardwareRequestDto)
        {

            var hardwareRequestInDb = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);
            hardwareRequestDto.Id = hardwareRequestInDb.Id;
            hardwareRequestInDb.StatusId = hardwareRequestDto.StatusId;
            hardwareRequestInDb.IsNew = false;
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Updated a Hardware Status",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }


        // technican updateStatus
        [HttpPut]
        [Route("api/v2/hardwareRequest/updateStatus/{id}")]
        public IHttpActionResult UpateStatusTech(int id, HardwareRequestDto hardwareRequestDto)
        {

            var hardwareRequestInDb = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);
            hardwareRequestDto.Id = hardwareRequestInDb.Id;
            hardwareRequestInDb.StatusId = hardwareRequestDto.StatusId;
            hardwareRequestInDb.IsNew = false;

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Updated a Hardware Status",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();

            return Ok();
        }

        //Technician UpdateStatus Technician
        [HttpPut]
        [Route("api/hardwareRequest/updateTechnician/{id}")]
        public IHttpActionResult UpdateTechnicianTech(int id, HardwareRequestDto hardwareRequestDto)
        {
            //var  technicianId = _db.HardwareTechnician.Where(x=>x.Email == )
            //get email of technician where (x => x.Email of 
            var hardwareRequestInDb = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);
            var hardwareTech = _db.HardwareTechnician.SingleOrDefault(d => d.Id == hardwareRequestDto.HardwareTechnicianId).Email;

            hardwareRequestDto.Id = hardwareRequestInDb.Id;
            hardwareRequestDto.IsAssigned = true;
            hardwareRequestInDb.HardwareTechnicianId = hardwareRequestDto.HardwareTechnicianId;
            hardwareRequestInDb.TechEmail = hardwareTech;
            hardwareRequestInDb.IsNew = false;
            hardwareRequestInDb.StatusId = 4;


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Assigned a Hardware Technician",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });

            _db.SaveChanges();
            return Ok();
        }

        //Hardware Admin update Technician
        [HttpPut]
        [Route("api/v2/hardwareRequest/updateTechnician/{id}")]
        public IHttpActionResult UpdateTechnician(int id, HardwareRequestDto hardwareRequestDto)
        {
            var hardwareRequestInDb = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);
            hardwareRequestDto.Id = hardwareRequestInDb.Id;
            hardwareRequestDto.IsAssigned = true;
            hardwareRequestInDb.IsNew = false;

            hardwareRequestInDb.HardwareTechnicianId = hardwareRequestDto.HardwareTechnicianId;

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Assigned a Hardware Technician",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/hardwareRequest/Assign/{id}")]
        public IHttpActionResult AssignTo(int id, HardwareRequestDto hardwareRequestDto)
        {
            var email = User.Identity.GetUserEmail();
            var techId = _db.HardwareTechnician.SingleOrDefault(x => x.Email == email).Id;

            var hardwareRequestInDb = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);
            hardwareRequestDto.Id = hardwareRequestInDb.Id;
            hardwareRequestInDb.HardwareTechnicianId = techId;
            hardwareRequestInDb.StatusId = 4;
            hardwareRequestInDb.TechEmail = User.Identity.GetUserEmail();
            hardwareRequestInDb.IsNew = false;

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Get Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });

            _db.SaveChanges();
            return Ok();
        }


        //technician Analysis Report
        [HttpPut]
        [Route("api/hardwareRequest/TechnicianForm/{id}")]
        public IHttpActionResult UpdateRequest(int id, HardwareRequestDto hardwareRequestDto)
        {

            var hardwareRequestInDb = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);

            hardwareRequestInDb.Id = id;
            hardwareRequestDto.Id = id;
            hardwareRequestInDb.FindingId = hardwareRequestDto.FindingId;
            hardwareRequestInDb.BrandName= hardwareRequestDto.BrandName;
            hardwareRequestInDb.HardwareId = hardwareRequestDto.HardwareId;
            hardwareRequestInDb.ModelName= hardwareRequestDto.ModelName;
            hardwareRequestInDb.UnitTypeId = hardwareRequestDto.UnitTypeId;
            hardwareRequestInDb.PossibleCause = hardwareRequestDto.PossibleCause;
            hardwareRequestInDb.DateStarted = hardwareRequestDto.DateStarted;
            hardwareRequestInDb.DateEnded = hardwareRequestDto.DateEnded;
            hardwareRequestInDb.Remarks = hardwareRequestDto.Remarks;
            hardwareRequestInDb.TimeEnded = hardwareRequestDto.TimeEnded;
            hardwareRequestInDb.TimeStarted = hardwareRequestDto.TimeStarted;
            hardwareRequestInDb.SerialNumber = hardwareRequestDto.SerialNumber;
            hardwareRequestInDb.ControlNumber = hardwareRequestDto.ControlNumber;
            hardwareRequestInDb.IsReported = true;


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added Hardware Analysis Report",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        //User Cancel Request
        [HttpPut]
        [Route("api/hardwareRequest/cancel/{id}")]
        public IHttpActionResult UpdateCancel(int id, HardwareRequestDto hardwareRequestDto)
        {

            var hardwareRequestInDb = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);

            hardwareRequestDto.Id = hardwareRequestInDb.Id;
            hardwareRequestInDb.StatusId = 5;
            hardwareRequestInDb.HardwareTechnicianId = null;
            hardwareRequestInDb.IsNew = false;

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.RequestHistory.Add(new RequestHistory()
            {
                UserName = User.Identity.GetFullName(),
                UploadMessage = "Cancelled A Hardware Request",
                UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentsId = depId,
                DivisionsId = divId,
            });

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Cancelled A Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();

            return Ok();
        }

        //Technician
        [HttpPut]
        [Route("api/hardwareRequest/approver/{id}")]
        public IHttpActionResult UpdateApprover(int id, HardwareRequestDto hardwareRequestDto)
        {

            var hardwareRequestInDb = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);

            hardwareRequestInDb.Id = id;
            hardwareRequestDto.Id = id;
            hardwareRequestInDb.Viewer = User.Identity.GetFullName();
            hardwareRequestInDb.IsVerified = true;
            hardwareRequestInDb.DateView = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Approved A Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }


        //Technician
        [HttpDelete]
        [Route("api/hardwareRequest/Delete/{id}")]
        public IHttpActionResult DeleteHardwareTech(int id)
        {
            var hardwareReq = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);
            if (hardwareReq == null)
            {
                return NotFound();
            }
            _db.HardwareRequest.Remove(hardwareReq);
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


        //Technician
        [HttpPut]
        [Route("api/hardwareRequest/viewer/{id}")]
        public IHttpActionResult UpdateViewed(int id, HardwareRequestDto hardwareRequestDto)
        {

            var hardwareRequestInDb = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);

            hardwareRequestInDb.Id = id;
            hardwareRequestDto.Id = id;
            hardwareRequestInDb.Approver = User.Identity.GetFullName();
            hardwareRequestInDb.ViewedRemarks = hardwareRequestDto.ViewedRemarks;
            hardwareRequestInDb.IsApproved = true;
            hardwareRequestInDb.DateApproved = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Approved a Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        //edit Request by User If Status is not updated by technican/Admin
        [HttpPut]
        [Route("api/hardwareRequest/Request/{id}")]
        public IHttpActionResult UpdateRequestDetails(int id, HardwareRequestDto hardwareRequestDto)
        {

            var hardwareRequestInDb = _db.HardwareRequest.SingleOrDefault(h => h.Id == id);

            hardwareRequestInDb.Id = id;
            hardwareRequestDto.Id = id;
            hardwareRequestInDb.UnitTypeId = hardwareRequestDto.UnitTypeId;
            hardwareRequestInDb.HardwareId = hardwareRequestDto.HardwareId;
            hardwareRequestInDb.BrandName = hardwareRequestDto.BrandName;
            hardwareRequestInDb.Description = hardwareRequestDto.Description;
            hardwareRequestInDb.ModelName = hardwareRequestDto.ModelName;
            hardwareRequestInDb.DocumentLabel = hardwareRequestDto.DocumentLabel;


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.RequestHistory.Add(new RequestHistory()
            {
                UserName = User.Identity.GetFullName(),
                UploadMessage = "Updated A Hardware Request",
                UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentsId = depId,
                DivisionsId = divId,
            });

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Updated A Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/hardware/countReq")]
        public IHttpActionResult GetCounthardware()
        {
            var hardwareReqDto = _db.HardwareRequest.ToList();
            return Ok(hardwareReqDto.Count);
        }


        //Query Request by Email
        [Route("api/hardware/requestbyUser")]
        public IHttpActionResult getsoftwareCount()
        {
            var hrDto = _db.HardwareRequest
                .ToList();
            if (User.IsInRole("Users"))
            {
                var rEmail = User.Identity.GetUserEmail();
                hrDto = new List<HardwareRequest>(hrDto.Where(x => x.Email == rEmail));
            }
            return Ok(hrDto.Count);
        }

        //Query Request by Email
        [Route("api/hardware/requestbyUserList")]
        public IHttpActionResult getsoftwareList()
        {
            var hrDto = _db.HardwareRequest
                .Include(x => x.Hardware)
                .ToList();
            if (User.IsInRole("Users"))
            {
                var rEmail = User.Identity.GetUserEmail();
                hrDto = new List<HardwareRequest>(hrDto.Where(x => x.Email == rEmail));
            }
            return Ok(hrDto.OrderByDescending(x => x.Id).Take(10));
        }

        //get by technicianId 
        [HttpGet]
        [Route("api/get/bytechId")]
        public IHttpActionResult TechId(string Email)
        {
            var TechId = _db.HardwareTechnician.Where(x => x.Email == Email);
            return Ok();
        }

        [HttpGet]
        [Route("api/hardware/gethardware")]
        public IHttpActionResult GetDashSoftware()
        {
            var sf = _db.HardwareRequest
                .Include(x => x.Hardware)
                .Include(x => x.HardwareTechnician)
                .Include(x => x.Departments)
                .Include(x => x.Status)
                .Include(x => x.Divisions)
                .ToList().Select(Mapper.Map<HardwareRequest, HardwareRequestSpecDto>);
            return Ok(sf.OrderByDescending(s => s.Ticket).Take(5));
        }

        [HttpGet]
        [Route("api/hrdStatus/count")]
        public IHttpActionResult HardwareCount()
        {
            var byName = from s in _db.HardwareRequest
                         group s by s.Status.Name into g
                         select new { Name = g.Key, Count = g.Count() };

            return Ok(byName);
        }

        [HttpGet]
        [Route("api/v2/hrd/count")]
        public IHttpActionResult UnitCountList()
        {
            var result = _db.HardwareRequest.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.UnitType.Name })
                 .Where(grp => grp.Count() > 0)
                 .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                 .OrderBy(x => x.Year).ThenBy(x => x.Month)
                 .ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("api/v2/hrdTech/count")]
        public IHttpActionResult TechCountList()
        {
            var result = _db.HardwareRequest.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.HardwareTechnician.Name })
                            .Where(grp => grp.Count() > 0)
                            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                            .OrderBy(x => x.Year).ThenBy(x => x.Month)
                            .ToList();
            return Ok(result);
        }
        [HttpGet]
        [Route("api/v2/hrdServices/count")]
        public IHttpActionResult HardwareServices()
        {
            var result = _db.HardwareRequest.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.Hardware.Name })
                .Where(grp => grp.Count() > 0)
                .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("api/v1/uploadDislpay/{id}")]
        public IHttpActionResult UploadDisplay(int id)
        {
            var file = _db.HardwareUploads.SingleOrDefault(f => f.Id == id);
            if (file == null)
                return NotFound();
            return Ok(Mapper.Map<HardwareUploads, HardwareUploadsDto>(file));
        }

        [HttpGet]
        [Route("api/v2/uploadDislpayList")]
        public IHttpActionResult UploadDisplayList()
        {
            var uploadDisplay = _db.HardwareUploads
                .Include(x => x.HardwareRequest).ToList().Select(Mapper.Map<HardwareUploads, HardwareUploadsDto>);

            //return Ok(hr.OrderByDescending(u=>u.DateAdded));
            return Ok(uploadDisplay.OrderByDescending(u => u.Id));
        }

        [Route("api/v2/uploadDislpay/{id}")]
        public IHttpActionResult GetUpload(int id)
        {
            var uploadHardware = _db.HardwareUploads.Include(x => x.HardwareRequest).ToList().Select(Mapper.Map<HardwareUploads, HardwareUploadsDto>);
            uploadHardware = new List<HardwareUploadsDto>(uploadHardware.Where(x => x.HardwareRequestId == id));

            var ftpAddress = "172.16.100.249";
            var ftpUserName = "cgpp-SR";
            var ftpPassword = "T]ZXm84q";


            var hardwareblob = new List<HardwareUploadsDto>();
            using (var ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword))
            {
                ftpClient.Port = 21;
                ftpClient.Connect();

                foreach (var s in uploadHardware)
                {
                    byte[] file;
                    ftpClient.DownloadBytes(out file, s.ImagePath);
                    s.DocumentBlob = file;
                    hardwareblob.Add(s);
                }
                ftpClient.Disconnect();
            }
            return Ok(uploadHardware);
        }

        //delete image
        [Route("api/v1/deleteImage/{id}")]
        public IHttpActionResult DeleteImage(int id)
        {
            var file = _db.HardwareUploads.SingleOrDefault(x => x.Id == id);
            if (file == null)
            {
                return NotFound();
            }
            _db.HardwareUploads.Remove(file);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/v2/saveFile")]
        public async Task<string> SaveUpload2()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/HardwareImage/");


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);

            var provider = new MultipartFormDataStreamProvider(root);

            var ftpAddress = "172.16.100.249";
            var ftpUserName = "cgpp-SR";
            var ftpPassword = "T]ZXm84q";

            FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
            ftpClient.Port = 21;
            ftpClient.Connect();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var file in provider.FileData)
                {
                    foreach (var key in provider.FormData.AllKeys)
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {
                            if (key == "hardwareRequestId")
                            {
                                var name = file.Headers
                                    .ContentDisposition
                                    .FileName;

                                // remove double quotes from string.
                                name = name.Trim('"');
                                var dateNew = Convert.ToString(DateTime.Now.Ticks) + "-";

                                var localFileName = file.LocalFileName;
                                var filePath = Path.Combine(root, dateNew + name);

                                ftpClient.UploadFile(localFileName, "/CGPP-SR/" + dateNew + name);

                                File.Move(localFileName, filePath);

                                HardwareUploads hardwareUploads = new HardwareUploads();
                                hardwareUploads.FileName = name;
                                hardwareUploads.ImagePath = "/CGPP-SR/" + dateNew + name;
                                hardwareUploads.HardwareRequestId = Convert.ToInt32(val);
                                hardwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                _db.HardwareUploads.Add(hardwareUploads);

                                _db.RequestHistory.Add(new RequestHistory()
                                {
                                    UserName = User.Identity.GetFullName(),
                                    UploadMessage = "Added Image in Hardware Request",
                                    UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                                    Email = User.Identity.GetUserName(),
                                    DepartmentsId = depId,
                                    DivisionsId = divId,
                                });

                                _db.HardwareRequestHistoty.Add(new HardwareRequestHistory()
                                {
                                    UserName = User.Identity.GetFullName(),
                                    RequetMessage = "Added Image in Hardware Request",
                                    RequetDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                                    Email = User.Identity.GetUserName(),
                                    DepartmentsId = depId,
                                    DivisionsId = divId,
                                });

                                _db.LoginActivity.Add(new LoginActivity()
                                {
                                    UserName = User.Identity.GetFullName(),
                                    ActivityMessage = "Added Image in Hardware Request",
                                    ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                                    Email = User.Identity.GetUserName(),

                                });
                                _db.SaveChanges();
                            }

                        }
                    }
                }
            }

            catch (Exception e)
            {
                return $"Error: {e.Message}";
            }
            ftpClient.Disconnect();

            return "File uploaded!";

        }


        [HttpPost]
        [Route("api/manualhardware/SaveRequest")]
        public async Task<string> manualSave()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/HardwareImage/");


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);
            var email = User.Identity.GetUserEmail();
            var techId = _db.HardwareTechnician.SingleOrDefault(x => x.Email == email).Id;

            var provider = new MultipartFormDataStreamProvider(root);
            var ftpAddress = "172.16.100.249";
            var ftpUserName = "cgpp-SR";
            var ftpPassword = "T]ZXm84q";

            FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
            ftpClient.Port = 21;
            ftpClient.Connect();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                HardwareRequest hardwareR = new HardwareRequest();

                if (hardwareR.Id == 0)
                {
                    hardwareR.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    hardwareR.DateCreated = DateTime.Now;
                    hardwareR.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;

                    hardwareR.FullName = provider.FormData["fullName"];
                    hardwareR.Email = provider.FormData["email"];
                    hardwareR.MobileNumber = provider.FormData["mobileNumber"];
                    hardwareR.DepartmentsId = Convert.ToInt32(provider.FormData["departmentsId"]);
                    hardwareR.DivisionsId = Convert.ToInt32(provider.FormData["divisionsId"]);
                    hardwareR.HardwareId = Convert.ToInt32(provider.FormData["hardwareId"]);
                    hardwareR.BrandName = provider.FormData["brandName"];
                    hardwareR.ModelName = provider.FormData["modelName"];
                    hardwareR.UnitTypeId = Convert.ToInt32(provider.FormData["unitTypeId"]);
                    hardwareR.DocumentLabel = provider.FormData["FileName"];
                    hardwareR.Description = provider.FormData["description"];

                    hardwareR.StatusId = 2;
                    hardwareR.TechEmail = User.Identity.GetUserName();
                    hardwareR.HardwareTechnicianId = techId;
                    hardwareR.IsReported = true;

                    hardwareR.PossibleCause = provider.FormData["possibleCause"];
                    hardwareR.FindingId = Convert.ToInt32(provider.FormData["findingId"]);
                    hardwareR.SerialNumber = provider.FormData["serialNumber"];
                    hardwareR.ControlNumber = provider.FormData["controlNumber"];
                    hardwareR.DateStarted = provider.FormData["dateStarted"];
                    hardwareR.DateEnded = provider.FormData["dateEnded"];
                    hardwareR.TimeStarted = provider.FormData["timeStarted"];
                    hardwareR.TimeEnded = provider.FormData["timeEnded"];
                    hardwareR.Remarks = provider.FormData["remarks"];



                    _db.HardwareRequest.Add(hardwareR);
                    _db.RequestHistory.Add(new RequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        UploadMessage = "Added a new Hardware Request",
                        UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.HardwareRequestHistoty.Add(new HardwareRequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        RequetMessage = "Added a new Hardware Request",
                        RequetDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a new Hardware Request",
                        ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                    });
                }

                _db.SaveChanges();

                foreach (var file in provider.FileData)
                {
                    foreach (var key in provider.FormData.AllKeys)
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {

                            if (key == "FileName")
                            {
                                var name = file.Headers.ContentDisposition.FileName;

                                // remove double quotes from string.
                                var dateNew = Convert.ToString(DateTime.Now.Ticks) + "-";
                                name = name.Trim('"');

                                var localFileName = file.LocalFileName;
                                var filePath = Path.Combine(root, dateNew + name);

                                ftpClient.UploadFile(localFileName, "/CGPP-SR/" + dateNew + name);

                                File.Delete(localFileName);

                                HardwareUploads hardwareUploads = new HardwareUploads();
                                hardwareUploads.FileName = name;
                                hardwareUploads.ImagePath = "/CGPP-SR/" + dateNew + name;
                                hardwareUploads.HardwareRequestId = hardwareR.Id;
                                hardwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                _db.HardwareUploads.Add(hardwareUploads);

                                _db.SaveChanges();
                            }

                        }
                    }
                }
            }

            catch (Exception e)
            {
                return $"Error: {e.Message}";
            }
            ftpClient.Disconnect();
            return "File uploaded!";

        }

        [HttpPost]
        [Route("api/Admin/AdminSaveRequest")]
        public async Task<string> manualSaveAdmin()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/HardwareImage/");


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);
            var email = User.Identity.GetUserEmail();
            var techId = _db.HardwareTechnician.SingleOrDefault(x => x.Email == email).Id;

            var provider = new MultipartFormDataStreamProvider(root);
            var ftpAddress = "172.16.100.249";
            var ftpUserName = "cgpp-SR";
            var ftpPassword = "T]ZXm84q";

            FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
            ftpClient.Port = 21;
            ftpClient.Connect();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                HardwareRequest hardwareR = new HardwareRequest();

                if (hardwareR.Id == 0)
                {
                    hardwareR.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    hardwareR.DateCreated = DateTime.Now;
                    hardwareR.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;

                    hardwareR.FullName = provider.FormData["fullName"];
                    hardwareR.Email = provider.FormData["email"];
                    hardwareR.MobileNumber = provider.FormData["mobileNumber"];
                    hardwareR.DepartmentsId = Convert.ToInt32(provider.FormData["departmentsId"]);
                    hardwareR.DivisionsId = Convert.ToInt32(provider.FormData["divisionsId"]);
                    hardwareR.HardwareId = Convert.ToInt32(provider.FormData["hardwareId"]);
                    hardwareR.BrandName = provider.FormData["brandName"];
                    hardwareR.ModelName = provider.FormData["modelName"];
                    hardwareR.UnitTypeId = Convert.ToInt32(provider.FormData["unitTypeId"]);
                    hardwareR.DocumentLabel = provider.FormData["FileName"];
                    hardwareR.Description = provider.FormData["description"];

                    hardwareR.StatusId = 2;
                    hardwareR.TechEmail = User.Identity.GetUserName();
                    hardwareR.HardwareTechnicianId = techId;
                    hardwareR.IsReported = true;
                    hardwareR.Viewer = User.Identity.GetFullName();
                    hardwareR.DateView = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    hardwareR.IsVerified = true;

                    hardwareR.PossibleCause = provider.FormData["possibleCause"];
                    hardwareR.FindingId = Convert.ToInt32(provider.FormData["findingId"]);
                    hardwareR.SerialNumber = provider.FormData["serialNumber"];
                    hardwareR.ControlNumber = provider.FormData["controlNumber"];
                    hardwareR.DateStarted = provider.FormData["dateStarted"];
                    hardwareR.DateEnded = provider.FormData["dateEnded"];
                    hardwareR.TimeStarted = provider.FormData["timeStarted"];
                    hardwareR.TimeEnded = provider.FormData["timeEnded"];
                    hardwareR.Remarks = provider.FormData["remarks"];



                    _db.HardwareRequest.Add(hardwareR);
                    _db.RequestHistory.Add(new RequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        UploadMessage = "Added a new Hardware Request",
                        UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.HardwareRequestHistoty.Add(new HardwareRequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        RequetMessage = "Added a new Hardware Request",
                        RequetDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a new Hardware Request",
                        ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                    });
                }

                _db.SaveChanges();

                foreach (var file in provider.FileData)
                {
                    foreach (var key in provider.FormData.AllKeys)
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {

                            if (key == "FileName")
                            {
                                var name = file.Headers.ContentDisposition.FileName;

                                // remove double quotes from string.
                                var dateNew = Convert.ToString(DateTime.Now.Ticks) + "-";
                                name = name.Trim('"');

                                var localFileName = file.LocalFileName;
                                var filePath = Path.Combine(root, dateNew + name);

                                ftpClient.UploadFile(localFileName, "/CGPP-SR/" + dateNew + name);

                                File.Delete(localFileName);

                                HardwareUploads hardwareUploads = new HardwareUploads();
                                hardwareUploads.FileName = name;
                                hardwareUploads.ImagePath = "/CGPP-SR/" + dateNew + name;
                                hardwareUploads.HardwareRequestId = hardwareR.Id;
                                hardwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                _db.HardwareUploads.Add(hardwareUploads);

                                _db.SaveChanges();
                            }

                        }
                    }
                }
            }

            catch (Exception e)
            {
                return $"Error: {e.Message}";
            }
            ftpClient.Disconnect();
            return "File uploaded!";

        }
    }
}
