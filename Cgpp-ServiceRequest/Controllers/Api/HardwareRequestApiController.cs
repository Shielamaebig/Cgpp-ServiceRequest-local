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
using System.Security.Cryptography;
//using Cgpp_ServiceRequest.Migrations;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class HardwareRequestApiController : ApiController
    {
        private ApplicationDbContext _db;

        IlsContext Db = new IlsContext();

        public HardwareRequestApiController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        [Route("api/v2/getuseremail")]
        public IHttpActionResult GetLogEmail()
        {
            var progEmail = User.Identity.GetLogEmail();
            return Ok(progEmail);
        }
        [Route("api/v2/hr/get")]


        [HttpGet]
        [Route("api/hardwareRequest/GetRequestbyId/{id}")]
        public IHttpActionResult GetRequestbyId(int id)
        {
            var hardwareRequestList = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);
            if (hardwareRequestList == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<HardwareUserRequest, HardwareUserRequestDto>(hardwareRequestList));
        }

        [HttpGet]
        [Route("api/HardwareUserRequest/get/Admin")]
        public IHttpActionResult GetAllHardwareRequest()
        {
            var req = _db.HardwareUserRequests.ToList();
            return Ok(req.OrderByDescending(x => x.Id).Take(8));
        }

        //Get Department
        [Route("api/hardwareRequest/department")]
        [HttpGet]
        public IHttpActionResult GetDepartment()
        {
            var department = _db.Departments.ToList().OrderByDescending(x => x.Id);
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

        [HttpPut]
        [Route("api/hardware/approve/{id}")]
        public IHttpActionResult UpdateApproverHr(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            var hardwareInDb = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);

            hardwareInDb.Id = id;
            hardwareUserRequestDto.Id = id;
            hardwareInDb.Status = "Open";
            hardwareInDb.IsNew = false;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Accept A Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),


            });
            _db.SaveChanges();
            return Ok();
        }

        //From Returned
        [HttpPut]
        [Route("api/hardware/approve/returned/{id}")]
        public IHttpActionResult UpdateApproverReturns(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            var hardwareInDb = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);

            hardwareInDb.Id = id;
            hardwareUserRequestDto.Id = id;
            hardwareInDb.Status = "In Progress";
            hardwareInDb.IsNew = false;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Accept A Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),


            });


            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/hardware/cancel/{id}")]
        public IHttpActionResult UpdateCancel(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            var hardwareInDb = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);

            hardwareInDb.Id = id;
            hardwareUserRequestDto.Id = id;
            hardwareInDb.Status = "Cancel";
            hardwareInDb.IsNew = false;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Canceled A Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),


            });
            _db.SaveChanges();
            return Ok();
        }


        [HttpPut]
        [Route("api/hardware/returned/{id}")]
        public IHttpActionResult UpdateReturn(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            var hardwareInDb = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);

            hardwareInDb.Id = id;
            hardwareUserRequestDto.Id = id;
            hardwareInDb.Status = "Return Request";
            hardwareInDb.IsNew = false;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Return Request to User",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),


            });

            Db.Drafts.Add(new Draft
            {
                Sendto = hardwareInDb.MobileNumber,
                msg = "Your Request Returned",
                tag = 0
            });
            Db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }
        //Post //hardwareUserRequest v2
        [HttpPost]
        [Route("api/hardwareRequest/v2/SaveRequest")]
        public async Task<string> SaveUpload()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/HardwareImage/");

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            //var hardwareTech = _db.HardwareTechnician.SingleOrDefault(d => d.Id == hardwareRequestDto.HardwareTechnicianId).Email;
            //int hardware = _db.Hardware.SingleOrDefault(x=>x.Id == ).Name;



            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);
            var devNumber = "09165778160";
            var mich = "09666443665";
            var mark = "09491511839";
            var jess = "09182695626";
            var provider = new MultipartFormDataStreamProvider(root);

            //var ftpAddress = "192.168.1.2";
            //var ftpAddress = "172.16.1.225";
            //var ftpUserName = "shielamaemalaque2022@outlook.com";
            //var ftpPassword = "Malaque@22+08";



            var ftpAddress = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpHost;
            var ftpUserName = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpUsername;
            var ftpPassword = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpPassword;
            FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
            ftpClient.Port = 21;
            ftpClient.Connect();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                HardwareUserRequest hardwareuseReq = new HardwareUserRequest();

                if (hardwareuseReq.Id == 0)
                {
                    hardwareuseReq.DocumentLabel = provider.FormData["FileName"];
                    hardwareuseReq.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    hardwareuseReq.DateCreated = DateTime.Now;
                    hardwareuseReq.FullName = User.Identity.GetFirstName() + ' ' + User.Identity.GetMiddleName() + ' ' + User.Identity.GetLastName();
                    hardwareuseReq.FirstName = User.Identity.GetFirstName();
                    hardwareuseReq.LastName = User.Identity.GetLastName();
                    hardwareuseReq.MiddleName = User.Identity.GetMiddleName();
                    hardwareuseReq.Email = User.Identity.GetLogEmail();
                    hardwareuseReq.MobileNumber = User.Identity.GetUserMobileNumber();
                    hardwareuseReq.DepartmentsId = depId;
                    hardwareuseReq.DepartmentName = _db.Departments.SingleOrDefault(d => d.Id == depId).Name;
                    hardwareuseReq.DivisionsId = divId;
                    hardwareuseReq.DivisionName = _db.Divisions.SingleOrDefault(d => d.Id == divId).Name;
                    hardwareuseReq.Description = provider.FormData["description"];
                    hardwareuseReq.UnitTypeId = Convert.ToInt32(provider.FormData["unitTypeId"]);
                    hardwareuseReq.UniTypes = _db.UnitType.SingleOrDefault(x => x.Id == hardwareuseReq.UnitTypeId).Name;
                    hardwareuseReq.BrandName = provider.FormData["brandName"];
                    hardwareuseReq.ModelName = provider.FormData["modelName"];
                    hardwareuseReq.HardwareId = Convert.ToInt32(provider.FormData["hardwareId"]);
                    hardwareuseReq.HardwareName = _db.Hardware.SingleOrDefault(x => x.Id == hardwareuseReq.HardwareId).Name;
                    hardwareuseReq.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;
                    hardwareuseReq.AnyDesk = provider.FormData["anyDesk"];
                    hardwareuseReq.TelNumber = provider.FormData["telNumber"];


                    //var isApprover = _db.Divisions.SingleOrDefault(x => x.Id == divId).IsDivisionApprover;
                    //var deptApprover = _db.Departments.SingleOrDefault(x => x.Id == depId).IsDepartmentApprover;
                    var isApprover = _db.Divisions.SingleOrDefault(x => x.Id == divId).IsDivisionApprover;
                    var deptApprover = _db.Departments.SingleOrDefault(x => x.Id == depId).IsDepartmentApprover;
                    if (deptApprover == true)
                    {
                        hardwareuseReq.Status = "Pending Department Approval";

                        var roles = _db.Roles.Where(r => r.Name == "DepartmentApprover");

                        var approvers = new List<ApplicationUser>();

                        if (roles.Any())
                        {
                            var roleId = roles.First().Id;
                            approvers = _db.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == roleId) && u.DepartmentsId == hardwareuseReq.DepartmentsId).ToList();


                        }

                        foreach (var approver in approvers)
                        {

                            Db.Drafts.Add(new Draft
                            {
                                Sendto = approver.MobileNumber,
                                msg = " New Hardware Request" + "  " + "Reported By :" + hardwareuseReq.FullName + " " + "Issue :" + hardwareuseReq.HardwareName + " " + "Department : " + " " + hardwareuseReq.DepartmentName,
                                tag = 0
                            });
                        }

                        Db.SaveChanges();
                    }
                    else if (isApprover == true)
                    {
                        hardwareuseReq.Status = "Pending Division Approval";

                        var roles = _db.Roles.Where(r => r.Name == "DivisionApprover");

                        var approvers = new List<ApplicationUser>();

                        if (roles.Any())
                        {
                            var roleId = roles.First().Id;
                            approvers = _db.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == roleId) && u.DivisionsId == hardwareuseReq.DivisionsId).ToList();


                        }

                        foreach (var approver in approvers)
                        {

                            Db.Drafts.Add(new Draft
                            {
                                Sendto = approver.MobileNumber,
                                msg = " New Hardware Request" + "  " + "Reported By :" + hardwareuseReq.FullName + " " + "Issue :" + hardwareuseReq.HardwareName + " " + "Department : " + " " + hardwareuseReq.DepartmentName,
                                tag = 0
                            });
                        }

                        Db.SaveChanges();
                    }

                    else
                    {
                        hardwareuseReq.Status = "Open";

                        Db.Drafts.Add(new Draft
                        {
                            Sendto = mich,
                            msg = " New Hardware Request" + "  " + "Reported By :" + hardwareuseReq.FullName + " " + "Issue :" + hardwareuseReq.HardwareName + " " + "Department : " + " " + hardwareuseReq.DepartmentName,
                            tag = 0
                        });
                        Db.Drafts.Add(new Draft
                        {
                            Sendto = devNumber,
                            msg = " New Hardware Request" + "  " + "Reported By :" + hardwareuseReq.FullName + " " + "Issue :" + hardwareuseReq.HardwareName + " " + "Department : " + " " + hardwareuseReq.DepartmentName,
                            tag = 0
                        });
                        Db.Drafts.Add(new Draft
                        {
                            Sendto = mark,
                            msg = " New Hardware Request" + "  " + "Reported By :" + hardwareuseReq.FullName + " " + "Issue :" + hardwareuseReq.HardwareName + " " + "Department : " + " " + hardwareuseReq.DepartmentName,
                            tag = 0
                        });
                        Db.Drafts.Add(new Draft
                        {
                            Sendto = jess,
                            msg = " New Hardware Request" + "  " + "Reported By :" + hardwareuseReq.FullName + " " + "Issue :" + hardwareuseReq.HardwareName + " " + "Department : " + " " + hardwareuseReq.DepartmentName,
                            tag = 0
                        });
                        Db.SaveChanges();
                    }



                    hardwareuseReq.IsNew = true;
                    _db.HardwareUserRequests.Add(hardwareuseReq);

                    _db.HardwareTasksLists.Add(new HardwareTasksList()
                    {
                        HardwareTaskId = 1,
                        DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        HardwareUserRequestId = hardwareuseReq.Id,
                    });


                    _db.RequestHistory.Add(new RequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        Email = User.Identity.GetLogEmail(),
                        DivisionsId = divId,
                        DepartmentsId = depId,
                        Category = false,
                        UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        UploadMessage = "Added a new Hardware Request",
                        HardwareUserRequestId = hardwareuseReq.Id,
                    });
                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a new Hardware Request",
                        ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetLogEmail(),
                        DivisionName = User.Identity.GetDivisionName(),
                        DepartmentName = User.Identity.GetDepartmentName(),


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
                                var codeNumber = Guid.NewGuid().ToString();
                                name = name.Trim('"');

                                var localFileName = file.LocalFileName;
                                var filePath = Path.Combine(root, codeNumber + name);

                                var folderName = _db.Departments.SingleOrDefault(x => x.Id == depId).Ftp.FolderName;

                                ftpClient.UploadFile(localFileName, folderName + codeNumber + name);

                                File.Delete(localFileName);



                                HardwareUserUploads hardwareuserUps = new HardwareUserUploads();
                                hardwareuserUps.FileName = codeNumber + name;
                                hardwareuserUps.ImagePath = _db.Departments.SingleOrDefault(x => x.Id == depId).Ftp.FolderName;
                                hardwareuserUps.HardwareUserRequestId = hardwareuseReq.Id;
                                hardwareuserUps.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                hardwareuserUps.FileExtension = Path.GetExtension(name);
                                hardwareuserUps.FtpId = _db.Departments.SingleOrDefault(x => x.Id == depId).FtpId;
                                _db.HardwareUserUploads.Add(hardwareuserUps);
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




        [HttpGet]
        [Route("api/v2/Approver/hardware/{id}")]
        public IHttpActionResult GetPendings(int id)
        {
            var accepts = _db.HardwareUserRequests.SingleOrDefault(x => x.Id == id);
            if (accepts == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<HardwareUserRequest, HardwareUserRequestDto>(accepts));
        }





        [HttpGet]
        [Route("api/v2/hr/assign")]
        public IHttpActionResult GetDashProgrammers()
        {
            var progDto = _db.TechnicianReports
                .Include(x => x.Hardware)
                .Include(x => x.HardwareUserRequest)
                .ToList().Select(Mapper.Map<TechnicianReport, TechnicianReportDto>);
            if (User.IsInRole("Technicians"))
            {
                var progEmail = User.Identity.GetUserEmail();
                progDto = new List<TechnicianReportDto>(progDto.Where(x => x.TechEmail == progEmail));
            }

            return Ok(progDto.OrderByDescending(x => x.Id).Take(5));
        }


        // Post
        [HttpPost]
        [Route("api/AssignTome/hardware/save")]
        public IHttpActionResult AssignedRequest(TechnicianReportDto technicianReportDto)
        {
            var assigned = Mapper.Map<TechnicianReportDto, TechnicianReport>(technicianReportDto);
            var userEmail = User.Identity.GetFullName();
            TechnicianReport technician = new TechnicianReport();
            technicianReportDto.Id = assigned.Id;
            technician.DateAssigned = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            technician.HardwareUserRequestId = technicianReportDto.HardwareUserRequestId;
            technician.HardwareTechnicianId = _db.HardwareTechnician.SingleOrDefault(x => x.Name == userEmail).Id;
            technician.TechnicianName = User.Identity.GetFullName();
            technician.TechEmail = User.Identity.GetLogEmail();
            //technician.HardwareTechnicianId = _db.HardwareTechnician.SingleOrDefault(x=>x.Name ==  userEmail).Id;
            //technician.TechEmail = _db.HardwareTechnician.SingleOrDefault(x => x.Id == technician.HardwareTechnicianId).Email;
            //technician.TechnicianName = _db.HardwareTechnician.SingleOrDefault(x => x.Id == technician.HardwareTechnicianId).Name;
            technician.DateCreated = DateTime.Now;
            technician.DateStarted = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            technician.Status = "In Progress";
            _db.TechnicianReports.Add(technician);
            _db.HardwareTasksLists.Add(new HardwareTasksList()
            {
                HardwareTaskId = 2,
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                HardwareUserRequestId = technician.HardwareUserRequestId,
                TechnicianReportId = assigned.Id
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Assigned A Technician",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + assigned.Id), technicianReportDto);
        }
        //re-assign
        [HttpPut]
        [Route("api/admin/Re-assign/{id}")]
        public IHttpActionResult Reassign(int id, TechnicianReportDto technicianReportDto)
        {

            var reAssign = _db.TechnicianReports.SingleOrDefault(h => h.Id == id);
            reAssign.Id = id;
            technicianReportDto.Id = id;
            reAssign.DateAssigned = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            reAssign.HardwareTechnicianId = technicianReportDto.HardwareTechnicianId;
            reAssign.TechEmail = _db.HardwareTechnician.SingleOrDefault(x => x.Id == technicianReportDto.HardwareTechnicianId).Email;
            reAssign.TechnicianName = _db.HardwareTechnician.SingleOrDefault(x => x.Id == technicianReportDto.HardwareTechnicianId).Name;
            reAssign.Status = "In Progress";
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Re-ssigned A Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }
        //technician Analysis Report
        [HttpPut]
        [Route("api/IsNew/UserRequest/{id}")]
        public IHttpActionResult updateIsNew(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var requestList = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);
            requestList.Id = id;
            hardwareUserRequestDto.Id = id;
            requestList.IsNew = false;
            requestList.Status = "In Progress";
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/User/UpdateStatus/{id}")]
        public IHttpActionResult updateStatus(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var requestList = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);
            requestList.Id = id;
            hardwareUserRequestDto.Id = id;
            requestList.Status = "Resolved";

            _db.SaveChanges();
            return Ok();
        }


        [HttpPut]
        [Route("api/manual/UpdateStatus/{id}")]
        public IHttpActionResult updateStatusManual(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var requestList = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);
            requestList.Id = id;
            hardwareUserRequestDto.Id = id;
            requestList.Status = "Resolved";


            _db.SaveChanges();
            return Ok();
        }









        //edit Request by User If technican/Admin
        [HttpPut]
        [Route("api/hardwareRequest/v2/Request/{id}")]
        public IHttpActionResult UpdateRequestDetailsTech(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {
            var devNumber = "09165778160";
            var mich = "09666443665";
            var mark = "09491511839";
            var jess = "09182695626";

            var hardwareRequestInDb = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);

            hardwareRequestInDb.Id = id;
            hardwareUserRequestDto.Id = id;
            hardwareRequestInDb.UnitTypeId = hardwareUserRequestDto.UnitTypeId;
            hardwareRequestInDb.UniTypes = _db.UnitType.SingleOrDefault(x => x.Id == hardwareUserRequestDto.UnitTypeId).Name;
            hardwareRequestInDb.HardwareId = hardwareUserRequestDto.HardwareId;
            hardwareRequestInDb.HardwareName = _db.Hardware.SingleOrDefault(x => x.Id == hardwareUserRequestDto.HardwareId).Name;
            hardwareRequestInDb.BrandName = hardwareUserRequestDto.BrandName;
            hardwareRequestInDb.Description = hardwareUserRequestDto.Description;
            hardwareRequestInDb.ModelName = hardwareUserRequestDto.ModelName;
            hardwareRequestInDb.DocumentLabel = hardwareUserRequestDto.DocumentLabel;



            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            var divApprover = _db.Divisions.SingleOrDefault(x => x.Id == divId).IsDivisionApprover;
            var deptApprover = _db.Departments.SingleOrDefault(x => x.Id == depId).IsDepartmentApprover;

            if (deptApprover == true)
            {
                hardwareRequestInDb.Status = "Pending Approval";

                var roles = _db.Roles.Where(r => r.Name == "DepartmentApprover");

                var approvers = new List<ApplicationUser>();

                if (roles.Any())
                {
                    var roleId = roles.First().Id;
                    approvers = _db.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == roleId) && u.DepartmentsId == hardwareRequestInDb.DepartmentsId).ToList();


                }

                foreach (var approver in approvers)
                {

                    Db.Drafts.Add(new Draft
                    {
                        Sendto = approver.MobileNumber,
                        msg = " New Hardware Request" + "  " + "Reported By :" + hardwareRequestInDb.FullName + " " + "Issue :" + hardwareRequestInDb.HardwareName + " " + "Department : " + " " + hardwareRequestInDb.DepartmentName,
                        tag = 0
                    });
                }

                Db.SaveChanges();

            }
            else if (divApprover == true)
            {
                hardwareRequestInDb.Status = "Pending Approval";


                var roles = _db.Roles.Where(r => r.Name == "DivisionApprover");

                var approvers = new List<ApplicationUser>();

                if (roles.Any())
                {
                    var roleId = roles.First().Id;
                    approvers = _db.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == roleId) && u.DivisionsId == hardwareRequestInDb.DivisionsId).ToList();


                }

                foreach (var approver in approvers)
                {

                    Db.Drafts.Add(new Draft
                    {
                        Sendto = approver.MobileNumber,
                        msg = " New Hardware Request" + "  " + "Reported By :" + hardwareRequestInDb.FullName + " " + "Issue :" + hardwareRequestInDb.HardwareName + " " + "Department : " + " " + hardwareRequestInDb.DepartmentName,
                        tag = 0
                    });
                }

                Db.SaveChanges();

            }
            else
            {
                hardwareRequestInDb.Status = "In Progress";

                Db.Drafts.Add(new Draft
                {
                    Sendto = mich,
                    msg = " New Hardware Request" + "  " + "Reported By :" + hardwareRequestInDb.FullName + " " + "Issue :" + hardwareRequestInDb.HardwareName + " " + "Department : " + " " + hardwareRequestInDb.DepartmentName,
                    tag = 0
                });
                Db.Drafts.Add(new Draft
                {
                    Sendto = devNumber,
                    msg = " New Hardware Request" + "  " + "Reported By :" + hardwareRequestInDb.FullName + " " + "Issue :" + hardwareRequestInDb.HardwareName + " " + "Department : " + " " + hardwareRequestInDb.DepartmentName,
                    tag = 0
                });
                Db.Drafts.Add(new Draft
                {
                    Sendto = mark,
                    msg = " New Hardware Request" + "  " + "Reported By :" + hardwareRequestInDb.FullName + " " + "Issue :" + hardwareRequestInDb.HardwareName + " " + "Department : " + " " + hardwareRequestInDb.DepartmentName,
                    tag = 0
                });
                Db.Drafts.Add(new Draft
                {
                    Sendto = jess,
                    msg = " New Hardware Request" + "  " + "Reported By :" + hardwareRequestInDb.FullName + " " + "Issue :" + hardwareRequestInDb.HardwareName + " " + "Department : " + " " + hardwareRequestInDb.DepartmentName,
                    tag = 0
                });
                Db.SaveChanges();
            }


            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edit A Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }





        [HttpPut]
        [Route("api/hardwareRequest/Request/{id}")]
        public IHttpActionResult UpdateRequestDetails(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var hardwareRequestInDb = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);

            hardwareRequestInDb.Id = id;
            hardwareUserRequestDto.Id = id;
            hardwareRequestInDb.UnitTypeId = hardwareUserRequestDto.UnitTypeId;
            hardwareRequestInDb.UniTypes = _db.UnitType.SingleOrDefault(x => x.Id == hardwareUserRequestDto.UnitTypeId).Name;
            hardwareRequestInDb.HardwareId = hardwareUserRequestDto.HardwareId;
            hardwareRequestInDb.HardwareName = _db.Hardware.SingleOrDefault(x => x.Id == hardwareUserRequestDto.HardwareId).Name;
            hardwareRequestInDb.BrandName = hardwareUserRequestDto.BrandName;
            hardwareRequestInDb.Description = hardwareUserRequestDto.Description;
            hardwareRequestInDb.ModelName = hardwareUserRequestDto.ModelName;
            hardwareRequestInDb.DocumentLabel = hardwareUserRequestDto.DocumentLabel;


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edit A Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/dev/hardwaretech/Edit/{id}")]
        public IHttpActionResult UpdateTechReport(int id, TechnicianReportDto technicianReportDto)
        {
            var techReportDto = _db.TechnicianReports.SingleOrDefault(x => x.Id == id);

            technicianReportDto.Id = id;
            techReportDto.Id = id;
            techReportDto.UnitTypeId = technicianReportDto.UnitTypeId;
            techReportDto.UniTypes = _db.UnitType.SingleOrDefault(x => x.Id == technicianReportDto.UnitTypeId).Name;
            techReportDto.HardwareId = technicianReportDto.HardwareId;
            techReportDto.HardwareName = _db.Hardware.SingleOrDefault(x => x.Id == technicianReportDto.HardwareId).Name;
            techReportDto.BrandName = technicianReportDto.BrandName;
            techReportDto.ModelName = technicianReportDto.ModelName;
            techReportDto.Remarks = technicianReportDto.Remarks;

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edit A Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }


        //Cancel
        [HttpPut]
        [Route("api/hardwareRequest/CancelRequest/{id}")]
        public IHttpActionResult CancelRequest(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var hardwareRequestInDb = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);

            hardwareRequestInDb.Id = id;
            hardwareUserRequestDto.Id = id;
            hardwareRequestInDb.Status = "Cancel";
            hardwareRequestInDb.IsNew = false;


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Cancelled A Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("api/hardware/countReq")]
        public IHttpActionResult GetCounthardware()
        {
            var hardwareReqDto = _db.HardwareUserRequests.ToList();
            return Ok(hardwareReqDto.Count);
        }


        //Query Request by Email
        [Route("api/hardware/requestbyUser")]
        public IHttpActionResult getsoftwareCount()
        {
            var hrDto = _db.HardwareUserRequests
                .ToList();
            if (User.IsInRole("Users"))
            {
                var rEmail = User.Identity.GetLogEmail();
                hrDto = new List<HardwareUserRequest>(hrDto.Where(x => x.Email == rEmail));
            }
            return Ok(hrDto.Count);
        }

        //Query Request by Email
        [Route("api/hardware/requestbyUserList")]
        public IHttpActionResult getsoftwareList()
        {
            var hrDto = _db.HardwareUserRequests
                .Include(x => x.Hardware)
                .ToList();
            if (User.IsInRole("Users"))
            {
                var rEmail = User.Identity.GetLogEmail();
                hrDto = new List<HardwareUserRequest>(hrDto.Where(x => x.Email == rEmail));
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
        [Route("api/v2/hrd/count")]
        public IHttpActionResult UnitCountList()
        {
            var result = _db.HardwareUserRequests.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.UnitType.Name })
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
            var result = _db.TechnicianReports.GroupBy(r => new { r.HardwareTechnician.Name })
                            .Where(grp => grp.Count() > 0)
                            .Select(g => new { g.Key.Name, Count = g.Count() })
                            .ToList();
            return Ok(result);
        }


        [HttpGet]
        [Route("api/v2/div/count")]
        public IHttpActionResult DivisionCountList()
        {
            var divName = User.Identity.GetDivisionName();

            var hr = _db.HardwareUserRequests.Where(x => x.DivisionName == divName);
            var reports = hr.GroupBy(x => new { x.DateCreated.Year, x.DateCreated.Month, x.FullName, x.HardwareName })
                .Where(grp => grp.Count() > 0)
                            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.HardwareName, g.Key.FullName, Count = g.Count() })
                            .ToList();
            return Ok(reports);
        }
        [HttpGet]
        [Route("api/v2/dept/count")]
        public IHttpActionResult DepartmentCountList()
        {
            var deptName = User.Identity.GetDepartmentName();

            var hr = _db.HardwareUserRequests.Where(x => x.DepartmentName == deptName);
            var reports = hr.GroupBy(x => new { x.DateCreated.Year, x.DateCreated.Month, x.FullName, x.HardwareName })
                .Where(grp => grp.Count() > 0)
                            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.HardwareName, g.Key.FullName, Count = g.Count() })
                            .ToList();
            return Ok(reports);
        }
        [HttpGet]
        [Route("api/v2/div/Yearly/count")]
        public IHttpActionResult DivisionYearlyCountList()
        {
            var divName = User.Identity.GetDivisionName();
            var hr = _db.HardwareUserRequests.Where(x => x.DivisionName == divName);
            var reports = hr.GroupBy(x => new { x.DateCreated.Year, x.FullName })
                            .Select(g => new { g.Key.Year, g.Key.FullName, Count = g.Count() })
                            .ToList();
            return Ok(reports);
        }

        //MyReport
        [HttpGet]
        [Route("api/v2/myReport/count")]
        public IHttpActionResult MyReportList()
        {
            var techName = User.Identity.GetFullName();
            var result = _db.TechnicianReports.GroupBy(r => new { r.DateCreated.Year, r.HardwareTechnician.Name })
                            .Where(grp => grp.Count() > 0)
                            .Select(g => new { g.Key.Year, g.Key.Name, Count = g.Count() })
                            .Where(x => x.Name == techName)
                            .ToList();
            return Ok(result);
        }

        //all yearly
        [HttpGet]
        [Route("api/v2/AllYearly/count")]
        public IHttpActionResult YearlyReport()
        {
            var result = _db.TechnicianReports.GroupBy(r => new { r.DateCreated.Year })
                            .Where(grp => grp.Count() > 0)
                            .Select(g => new { g.Key.Year, Count = g.Count() })
                            .ToList();
            return Ok(result);
        }
        //monthly
        [HttpGet]
        [Route("api/v2/mytech/count")]
        public IHttpActionResult MonthlytechList()
        {
            int dateNow = Convert.ToInt32(DateTime.Now.Year.ToString());
            var techName = User.Identity.GetFullName();
            var result = _db.TechnicianReports.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.HardwareTechnician.Name })
                            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                            .Where(x => x.Year == dateNow && x.Name == techName)
                            .OrderBy(x => x.Year).ThenBy(x => x.Month)
                            .ToList();

            return Ok(result);
        }


        [HttpGet]
        [Route("api/getfullName/count")]
        public IHttpActionResult fullName()
        {
            var techName = User.Identity.GetFullName();

            return Ok(techName);
        }
        //count monthly All

        [HttpGet]
        [Route("api/v2/alltech/count")]
        public IHttpActionResult AlltechList()
        {
            int dateNow = Convert.ToInt32(DateTime.Now.Year.ToString());
            var result = _db.TechnicianReports.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.HardwareTechnician.Name })
                            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                            .Where(x => x.Year == dateNow)
                            .OrderBy(x => x.Year).ThenBy(x => x.Month)
                            .ToList();

            return Ok(result);
        }

        //6months
        [HttpGet]
        [Route("api/v2/SixMonths/count")]
        public IHttpActionResult SixMonthAYear()
        {
            int dateNow = Convert.ToInt32(DateTime.Now.Year.ToString());
            //int sixMonths = Convert.ToInt32((DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-5));

            var result = _db.TechnicianReports.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.HardwareTechnician.Name })
                            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                            .Where(x => x.Month == dateNow)
                            .OrderBy(x => x.Year).ThenBy(x => x.Month)
                            .ToList();
            return Ok(result);
        }
        //count perTech
        [HttpGet]
        [Route("api/v2/perTech/count")]
        public IHttpActionResult PerTechList()
        {
            var techName = "Technician Developer Tech";
            int dateNow = Convert.ToInt32(DateTime.Now.Year.ToString());
            var result = _db.TechnicianReports.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.HardwareTechnician.Name })
                            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                            .Where(x => x.Name == techName && x.Year == dateNow)
                            .OrderBy(x => x.Year).ThenBy(x => x.Month)
                            .ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("api/v2/hrdServices/count")]
        public IHttpActionResult HardwareServices()
        {
            var result = _db.HardwareUserRequests.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.Hardware.Name })
                .Where(grp => grp.Count() > 0)
                .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();
            return Ok(result);
        }





        //delete image
        [Route("api/v1/deleteImage/{id}")]
        public IHttpActionResult DeleteImage(int id)
        {
            var file = _db.HardwareUserUploads.SingleOrDefault(x => x.Id == id);
            if (file == null)
            {
                return NotFound();
            }
            _db.HardwareUserUploads.Remove(file);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted a Hardware Request Image file",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
            });
            _db.SaveChanges();
            return Ok();
        }

        //delete image
        [Route("api/v2/deleteImage/{id}")]
        public IHttpActionResult DeleteImage2(int id)
        {
            var file = _db.TechnicianUploads.SingleOrDefault(x => x.Id == id);
            if (file == null)
            {
                return NotFound();
            }
            _db.TechnicianUploads.Remove(file);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted a Hardware Request Image file",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
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

            var ftpAddress = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpHost;
            var ftpUserName = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpUsername;
            var ftpPassword = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpPassword;
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
                            if (key == "hardwareUserRequestId")
                            {
                                var name = file.Headers
                                    .ContentDisposition
                                    .FileName;
                                var codeNumber = Guid.NewGuid().ToString();

                                // remove double quotes from string.
                                name = name.Trim('"');

                                var localFileName = file.LocalFileName;
                                var filePath = Path.Combine(root, codeNumber + name);
                                var folderName = _db.Departments.SingleOrDefault(x => x.Id == depId).Ftp.FolderName;
                                ftpClient.UploadFile(localFileName, folderName + codeNumber + name);

                                File.Delete(localFileName);

                                HardwareUserUploads hardwareUploads = new HardwareUserUploads();
                                hardwareUploads.FileName = codeNumber + name;
                                hardwareUploads.ImagePath = folderName;
                                hardwareUploads.HardwareUserRequestId = Convert.ToInt32(val);
                                hardwareUploads.FileExtension = Path.GetExtension(name);
                                hardwareUploads.FtpId = _db.Departments.SingleOrDefault(x => x.Id == depId).FtpId;
                                hardwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                _db.HardwareUserUploads.Add(hardwareUploads);


                                _db.LoginActivity.Add(new LoginActivity()
                                {
                                    UserName = User.Identity.GetFullName(),
                                    ActivityMessage = "Added Image in Hardware Request",
                                    ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                                    Email = User.Identity.GetLogEmail(),
                                    DepartmentName = User.Identity.GetDepartmentName(),
                                    DivisionName = User.Identity.GetDivisionName(),
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

        [HttpPut]
        [Route("api/v2/sms/techReport/{id}")]
        public IHttpActionResult SendSms(int id, TechnicianReportDto technicianReportDto)
        {

            var techReports = _db.TechnicianReports.SingleOrDefault(h => h.Id == id);


            technicianReportDto.Id = techReports.Id;
            techReports.SmsMessage = technicianReportDto.SmsMessage;
            techReports.DateSend = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Send a Message",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
            });

            Db.Drafts.Add(new Draft
            {
                Sendto = technicianReportDto.MobileNumber,
                msg = techReports.SmsMessage + "This message is system-generated. No need to reply. Thank you. From: Office of the City Management Information System",
                tag = 0
            });
            Db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }
        [HttpPost]
        [Route("api/tech/saveFile")]
        public async Task<string> SaveUpload3()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/HardwareImage/");


            var dept = User.Identity.GetDepartmentName();
            var div = User.Identity.GetDivisionName();
            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);

            var provider = new MultipartFormDataStreamProvider(root);

            var ftpAddress = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Name == dept).Ftp.FtpHost;
            var ftpUserName = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Name == dept).Ftp.FtpUsername;
            var ftpPassword = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Name == dept).Ftp.FtpPassword;


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
                            if (key == "technicianReportId")
                            {
                                var name = file.Headers
                                    .ContentDisposition
                                    .FileName;

                                // remove double quotes from string.
                                name = name.Trim('"');
                                var codeNumber = Guid.NewGuid().ToString();

                                var localFileName = file.LocalFileName;
                                var filePath = Path.Combine(root, codeNumber + name);
                                var folderName = _db.Departments.SingleOrDefault(x => x.Name == dept).Ftp.FolderName;
                                ftpClient.UploadFile(localFileName, folderName + codeNumber + name);


                                File.Delete(localFileName);

                                TechnicianUploads techUploads = new TechnicianUploads();
                                techUploads.FileName = codeNumber + name;
                                techUploads.ImagePath = folderName;
                                techUploads.TechnicianReportId = Convert.ToInt32(val);
                                techUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                techUploads.FileExtension = Path.GetExtension(name);
                                techUploads.FtpId = _db.Departments.SingleOrDefault(x => x.Name == dept).FtpId;
                                _db.TechnicianUploads.Add(techUploads);
                                _db.LoginActivity.Add(new LoginActivity()
                                {
                                    UserName = User.Identity.GetFullName(),
                                    ActivityMessage = "Added Image attachment in Hardware Request",
                                    ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                                    Email = User.Identity.GetLogEmail(),
                                    DepartmentName = User.Identity.GetDepartmentName(),
                                    DivisionName = User.Identity.GetDivisionName(),
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





        [HttpPut]
        [Route("api/hardware/acceptAssigned/{id}")]
        public IHttpActionResult HardwareAccepts(int id, HardwareRequestDto hardwareRequestDto)
        {
            var hardwareRequest = _db.HardwareUserRequests.SingleOrDefault(s => s.Id == id);
            hardwareRequestDto.Id = hardwareRequest.Id;
            hardwareRequest.Status = "Pending Assigned Request";
            hardwareRequest.IsNew = false;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Accept Assigned Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/userTask/get")]
        public IHttpActionResult GetTask()
        {
            var userTask = _db.HardwareTasksLists.Include(x => x.HardwareTask).Include(x => x.TechnicianReport).Include(x => x.HardwareUserRequest).ToList().Select(Mapper.Map<HardwareTasksList, HardwareTasksListDto>);
            return Ok(userTask);
        }
        [HttpGet]
        [Route("api/userRequest/get")]
        public IHttpActionResult GetUserRequestk()
        {
            var user = _db.HardwareUserRequests.ToList()
                .Select(Mapper.Map<HardwareUserRequest, HardwareUserRequestDto>);
            return Ok(user);
        }
        [HttpGet]
        [Route("api/tech/get")]
        public IHttpActionResult GetUserTech()
        {
            var techlist = _db.TechnicianReports.Include(x => x.HardwareUserRequest).ToList()
                .Select(Mapper.Map<TechnicianReport, TechnicianReportDto>);
            return Ok(techlist);
        }
        //[HttpGet]
        //[Route("api/techAssign/get/{id}")]
        //public IHttpActionResult GetRequestList(int id)
        //{
        //    var assign = _db.HardwareTaskLists.Include(x => x.TechnicianReport).Include(x => x.HardwareUserRequest).Include(x => x.HardwareVerify).Include(x => x.HardwareApproval).SingleOrDefault(x => x.Id == id);
        //    if (assign == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(Mapper.Map<HardwareTaskList, HardwareTasksListDto>(assign));
        //}

        [HttpGet]
        [Route("api/techAssign/get/{id}")]
        public IHttpActionResult GetRequestList(int id)
        {
            var assign = _db.HardwareTasksLists.Include(x => x.TechnicianReport).Include(x => x.HardwareUserRequest).Include(x => x.HardwareVerify).Include(x => x.HardwareApproval).Include(x => x.HardwareAcceptsRequest).SingleOrDefault(x => x.Id == id);
            if (assign == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<HardwareTasksList, HardwareTasksListDto>(assign));
        }
        [HttpGet]
        [Route("api/tech/hardwareAccepts/{id}")]
        public IHttpActionResult GetHardwareAcceptsRequest(int id)
        {
            var test = _db.HardwareTasksLists.Include(x => x.HardwareAcceptsRequest).SingleOrDefaultAsync(x => x.Id == id);

            return Ok(test);
        }

        [HttpGet]
        [Route("api/userRequest/getbyId/{id}")]
        public IHttpActionResult GetRequest(int id)
        {
            var list = _db.HardwareUserRequests.SingleOrDefault(x => x.Id == id);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<HardwareUserRequest, HardwareUserRequestDto>(list));
        }


        [Route("api/v2/userUploads/{id}")]
        public IHttpActionResult GetUserUpload(int id)
        {

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());


            var uploadHardware = _db.HardwareUserUploads.Include(x => x.HardwareUserRequest).ToList().Select(Mapper.Map<HardwareUserUploads, HardwareUserUploadsDto>);
            uploadHardware = new List<HardwareUserUploadsDto>(uploadHardware.Where(x => x.HardwareUserRequestId == id));

            var ftpAddress = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpHost;
            var ftpUserName = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpUsername;
            var ftpPassword = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpPassword;


            var hardwareblob = new List<HardwareUserUploadsDto>();
            using (var ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword))
            {
                ftpClient.Port = 21;
                ftpClient.Connect();

                foreach (var s in uploadHardware)
                {
                    byte[] file;
                    ftpClient.DownloadBytes(out file, s.ImagePath + s.FileName);
                    s.DocumentBlob = file;
                    hardwareblob.Add(s);
                }
                ftpClient.Disconnect();
            }
            return Ok(uploadHardware);
        }


        [Route("api/techUploads/get/{id}")]
        public IHttpActionResult GettechUploads(int id)
        {
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());


            var uploadHardwaretech = _db.TechnicianUploads.Include(x => x.TechnicianReport).ToList().Select(Mapper.Map<TechnicianUploads, TechnicianUploadsDto>);
            uploadHardwaretech = new List<TechnicianUploadsDto>(uploadHardwaretech.Where(x => x.TechnicianReportId == id));

            var ftpAddress = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpHost;
            var ftpUserName = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpUsername;
            var ftpPassword = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpPassword;

            var hardwareblob = new List<TechnicianUploadsDto>();
            using (var ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword))
            {
                ftpClient.Port = 21;
                ftpClient.Connect();

                foreach (var s in uploadHardwaretech)
                {
                    byte[] file;
                    ftpClient.DownloadBytes(out file, s.ImagePath + s.FileName);
                    s.DocumentBlob = file;
                    hardwareblob.Add(s);
                }
                ftpClient.Disconnect();
            }
            return Ok(uploadHardwaretech);
        }
        [HttpGet]
        [Route("api/in-progress/v2/{id}")]
        public IHttpActionResult Getinprogress(int id)
        {
            var techReport = _db.TechnicianReports.Include(x => x.HardwareUserRequest).ToList().Select(Mapper.Map<TechnicianReport, TechnicianReportDto>);
            techReport = new List<TechnicianReportDto>(techReport.Where(x => x.HardwareUserRequestId == id));

            return Ok(techReport);
        }
        [HttpGet]
        [Route("api/techAssigned/getbyId/{id}")]
        public IHttpActionResult GetAssigned(int id)
        {
            var list = _db.TechnicianReports.SingleOrDefault(x => x.Id == id);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<TechnicianReport, TechnicianReportDto>(list));
        }

        [HttpGet]
        [Route("api/v7/tech/getbyId/{id}")]
        public IHttpActionResult GetTechReportas(int id)
        {
            var techReport = _db.TechnicianUploads.Include(x => x.TechnicianReport).ToList().Select(Mapper.Map<TechnicianUploads, TechnicianUploadsDto>);
            techReport = new List<TechnicianUploadsDto>(techReport.Where(x => x.TechnicianReportId == id));

            return Ok(techReport);
        }
        [HttpGet]
        [Route("api/usertech/getbyId/{id}")]
        public IHttpActionResult GetTechReport(int id)
        {
            var techReport = _db.TechnicianReports.Include(x => x.HardwareUserRequest).ToList().Select(Mapper.Map<TechnicianReport, TechnicianReportDto>);
            techReport = new List<TechnicianReportDto>(techReport.Where(x => x.HardwareUserRequestId == id));

            return Ok(techReport);
        }
        [HttpGet]
        [Route("api/technician/get")]
        public IHttpActionResult GetInfo()
        {
            var techlist = _db.TechnicianReports.Include(x => x.HardwareUserRequest).Include(x => x.HardwareTechnician).ToList()
                                .Select(Mapper.Map<TechnicianReport, TechnicianReportDto>);
            return Ok(techlist);
        }

        [HttpGet]
        [Route("api/userTask/getbyId/{id}")]
        public IHttpActionResult GetTask(int id)
        {
            var task = _db.HardwareTasksLists.Include(x => x.HardwareUserRequest).Include(x => x.HardwareTask).Include(x => x.TechnicianReport).Include(x => x.HardwareVerify).Include(x => x.HardwareApproval).Include(x => x.HardwareAcceptsRequest).ToList().Select(Mapper.Map<HardwareTasksList, HardwareTasksListDto>);
            task = new List<HardwareTasksListDto>(task.Where(x => x.HardwareUserRequestId == id));

            return Ok(task);
        }
        [HttpGet]
        [Route("api/V2/usertask/getbyId/{id}")]
        public IHttpActionResult GetTask2(int id)
        {
            var task = _db.HardwareTasksLists.Include(x => x.HardwareUserRequest).Include(x => x.HardwareTask).Include(x => x.TechnicianReport).ToList().Select(Mapper.Map<HardwareTasksList, HardwareTasksListDto>);
            task = new List<HardwareTasksListDto>(task.Where(x => x.TechnicianReportId == id));

            return Ok(task);
        }


        [HttpGet]
        [Route("api/verified/getbyId/{id}")]
        public IHttpActionResult GetVerified(int id)
        {
            var tech = _db.TechnicianReports.Include(x => x.HardwareUserRequest).ToList().Select(Mapper.Map<TechnicianReport, TechnicianReportDto>);
            tech = new List<TechnicianReportDto>(tech.Where(x => x.HardwareUserRequestId == id));

            return Ok(tech);
        }
        [HttpGet]
        [Route("api/v2/getbyId/{id}")]
        public IHttpActionResult GetTechnicianReportId(int id)
        {
            var technician = _db.TechnicianReports.Include(x => x.HardwareUserRequest).SingleOrDefault(x => x.Id == id);
            if (technician == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<TechnicianReport, TechnicianReportDto>(technician));
        }
        [HttpGet]
        [Route("api/v2/UserTask/getbyId/{id}")]
        public IHttpActionResult GetTaskList(int id)
        {
            //var list = _db.HardwareTaskLists.Include(x => x.HardwareUserRequest).SingleOrDefault(x => x.Id == id);
            //if (list == null)
            //{
            //    return NotFound();
            //}
            //return Ok(Mapper.Map<HardwareTaskList, HardwareTasksListDto>(list));

            var list = _db.HardwareTasksLists.Include(x => x.HardwareUserRequest).SingleOrDefault(x => x.Id == id);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<HardwareTasksList, HardwareTasksListDto>(list));
        }
        [HttpGet]
        [Route("api/v2/Tech/getbyId/{id}")]
        public IHttpActionResult GetTech(int id)
        {
            var tech = _db.TechnicianReports.Include(x => x.HardwareUserRequest).SingleOrDefault(x => x.Id == id);
            if (tech == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<TechnicianReport, TechnicianReportDto>(tech));
        }

        [HttpGet]
        [Route("api/v3/Tech/getbyId/{id}")]
        public IHttpActionResult GetTechbyAdmin(int id)
        {
            var tech = _db.TechnicianReports.Include(x => x.HardwareUserRequest).SingleOrDefault(x => x.HardwareUserRequestId == id);
            if (tech == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<TechnicianReport, TechnicianReportDto>(tech));
        }

        [HttpPut]
        [Route("api/hardwareRequest/techReport/{id}")]
        public IHttpActionResult TechReport(int id, TechnicianReportDto technicianReportDto)
        {

            var techReports = _db.TechnicianReports.SingleOrDefault(h => h.Id == id);


            technicianReportDto.Id = techReports.Id;
            techReports.HardwareId = technicianReportDto.HardwareId;
            techReports.HardwareName = _db.Hardware.SingleOrDefault(x => x.Id == techReports.HardwareId).Name;
            techReports.UnitTypeId = technicianReportDto.UnitTypeId;
            techReports.UniTypes = _db.UnitType.SingleOrDefault(x => x.Id == techReports.UnitTypeId).Name;
            techReports.BrandName = technicianReportDto.BrandName;
            techReports.ModelName = technicianReportDto.ModelName;
            techReports.FindingId = technicianReportDto.FindingId;
            techReports.FindingName = _db.Finding.SingleOrDefault(x => x.Id == techReports.FindingId).Name;
            techReports.PossibleCause = technicianReportDto.PossibleCause;
            techReports.SerialNumber = technicianReportDto.SerialNumber;
            techReports.ControlNumber = technicianReportDto.ControlNumber;
            techReports.Remarks = technicianReportDto.Remarks;
            techReports.Status = "Resolved";
            techReports.DateEnded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");


            _db.HardwareTasksLists.Add(new HardwareTasksList()
            {
                HardwareTaskId = 3,
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                HardwareUserRequestId = technicianReportDto.HardwareUserRequestId,
                TechnicianReportId = techReports.Id
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Reported A Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            Db.Drafts.Add(new Draft
            {
                Sendto = technicianReportDto.MobileNumber,
                msg = "Your Service Request" + " " + "Ticket No. : " + technicianReportDto.Ticket + " " + "Is Resolved",
                tag = 0
            });
            Db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/hardwareRequestv2/techReport/{id}")]
        public IHttpActionResult TechReportManual(int id, TechnicianReportDto technicianReportDto)
        {

            var techReports = _db.TechnicianReports.SingleOrDefault(h => h.Id == id);


            technicianReportDto.Id = techReports.Id;
            techReports.HardwareId = technicianReportDto.HardwareId;
            techReports.HardwareName = _db.Hardware.SingleOrDefault(x => x.Id == techReports.HardwareId).Name;
            techReports.UnitTypeId = technicianReportDto.UnitTypeId;
            techReports.UniTypes = _db.UnitType.SingleOrDefault(x => x.Id == techReports.UnitTypeId).Name;
            techReports.BrandName = technicianReportDto.BrandName;
            techReports.ModelName = technicianReportDto.ModelName;
            techReports.FindingId = technicianReportDto.FindingId;
            techReports.FindingName = _db.Finding.SingleOrDefault(x => x.Id == techReports.FindingId).Name;
            techReports.PossibleCause = technicianReportDto.PossibleCause;
            techReports.SerialNumber = technicianReportDto.SerialNumber;
            techReports.ControlNumber = technicianReportDto.ControlNumber;
            techReports.Remarks = technicianReportDto.Remarks;
            techReports.Status = "Resolved";
            techReports.DateStarted = technicianReportDto.DateStarted;
            techReports.DateEnded = technicianReportDto.DateEnded;


            _db.HardwareTasksLists.Add(new HardwareTasksList()
            {
                HardwareTaskId = 3,
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                HardwareUserRequestId = technicianReportDto.HardwareUserRequestId,
                TechnicianReportId = techReports.Id
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Reported A Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });


            _db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/verified/save")]
        public IHttpActionResult VerifiedSave(HardwareVerifyDto hardwareVerifyDto)
        {
            var verified = Mapper.Map<HardwareVerifyDto, HardwareVerify>(hardwareVerifyDto);

            HardwareVerify verify = new HardwareVerify();
            hardwareVerifyDto.Id = verified.Id;
            verify.Name = User.Identity.GetFullName();
            verify.Email = User.Identity.GetLogEmail();
            verify.DateVerified = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            verify.HardwareUserRequestId = hardwareVerifyDto.HardwareUserRequestId;
            verify.TechnicianReportId = hardwareVerifyDto.TechnicianReportId;
            _db.HardwareVerifies.Add(verify);

            _db.HardwareTasksLists.Add(new HardwareTasksList()
            {
                HardwareTaskId = 4,
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                HardwareUserRequestId = hardwareVerifyDto.HardwareUserRequestId,
                TechnicianReportId = hardwareVerifyDto.TechnicianReportId,
                HardwareVerifyId = verified.Id
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Verified a Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + verified.Id), hardwareVerifyDto);
        }
        //From Admin
        [HttpPost]
        [Route("api/v2/AssignTome/save")]
        public IHttpActionResult Assigned(TechnicianReportDto technicianReportDto)
        {
            var assigned = Mapper.Map<TechnicianReportDto, TechnicianReport>(technicianReportDto);

            TechnicianReport technician = new TechnicianReport();
            technicianReportDto.Id = assigned.Id;
            technician.DateAssigned = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            technician.HardwareUserRequestId = technicianReportDto.HardwareUserRequestId;
            technician.HardwareTechnicianId = technicianReportDto.HardwareTechnicianId;
            technician.TechEmail = _db.HardwareTechnician.SingleOrDefault(x => x.Id == technicianReportDto.HardwareTechnicianId).Email;
            technician.TechnicianName = _db.HardwareTechnician.SingleOrDefault(x => x.Id == technicianReportDto.HardwareTechnicianId).Name;
            technician.DateCreated = DateTime.Now;
            technician.Status = "In Progress";

            _db.TechnicianReports.Add(technician);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Assigned A Technician",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + assigned.Id), technicianReportDto);
        }



        [HttpPut]
        [Route("api/accept/UserRequest/{id}")]
        public IHttpActionResult aceeptUserRequest(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var requestList = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);
            requestList.Id = id;
            hardwareUserRequestDto.Id = id;
            requestList.Status = "Pending Assigned Request";
            requestList.IsNew = false;
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/started/UserRequest/{id}")]
        public IHttpActionResult DateStart(int id, TechnicianReportDto technicianReportDto)
        {

            var start = _db.TechnicianReports.SingleOrDefault(h => h.Id == id);
            start.Id = id;
            technicianReportDto.Id = id;
            start.DateStarted = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            _db.HardwareTasksLists.Add(new HardwareTasksList()
            {
                HardwareTaskId = 2,
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                HardwareUserRequestId = technicianReportDto.HardwareUserRequestId,
                TechnicianReportId = start.Id,

            });
            _db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Route("api/verified/UserRequest/{id}")]
        public IHttpActionResult GetVerifiedName(int id, TechnicianReportDto technicianReportDto)
        {
            var adminName = User.Identity.GetFullName();

            var verifyname = _db.TechnicianReports.SingleOrDefault(h => h.Id == id);
            verifyname.Id = id;
            technicianReportDto.Id = id;
            verifyname.AdminName = "JOSE PEPITO BONETE JR.";
            verifyname.SuperAdminId = 1;
            verifyname.SuperName = _db.SuperAdmins.SingleOrDefault(x => x.Id == 1).Name;
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/v2/IsNew/UserRequest/{id}")]
        public IHttpActionResult updateIsNew2(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var requestList = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);
            requestList.Id = id;
            hardwareUserRequestDto.Id = id;
            requestList.Status = "In Progress";
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Accepts a Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/v2/status/UserRequest/{id}")]
        public IHttpActionResult GetStatus(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var requestList = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);
            requestList.Id = id;
            hardwareUserRequestDto.Id = id;
            requestList.Status = "Verified";
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/Tech/task/{id}")]
        public IHttpActionResult GetTaskbyId(int id)
        {
            //var report = _db.HardwareTaskLists.Include(x => x.HardwareUserRequest).Include(x => x.TechnicianReport).SingleOrDefault(x => x.HardwareUserRequestId == id);
            //if (report == null)
            //{
            //    return NotFound();
            //}
            //return Ok(Mapper.Map<HardwareTasksList, HardwareTasksListDto>(report));
            var report = _db.HardwareTasksLists.Include(x => x.HardwareUserRequest).Include(x => x.TechnicianReport).SingleOrDefault(x => x.Id == id);
            if (report == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<HardwareTasksList, HardwareTasksListDto>(report));
        }

        [HttpGet]
        [Route("api/v4/Verified/getbyId/{id}")]
        public IHttpActionResult GetVerified5(int id)
        {
            var verify = _db.HardwareVerifies.Include(x => x.HardwareUserRequest).Include(x => x.TechnicianReport).SingleOrDefault(x => x.Id == id);
            if (verify == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<HardwareVerify, HardwareVerifyDto>(verify));
        }

        [HttpGet]
        [Route("api/v4/approved/getbyId/{id}")]
        public IHttpActionResult GetApproved(int id)
        {
            var approved = _db.HardwareApprovals.Include(x => x.HardwareUserRequest).Include(x => x.TechnicianReport).Include(x => x.HardwareVerify).SingleOrDefault(x => x.Id == id);
            if (approved == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<HardwareApproval, HardwareApprovalDto>(approved));
        }

        //Approve
        [HttpPost]
        [Route("api/approve/save")]
        public IHttpActionResult Approved(HardwareApprovalDto hardwareApprovalDto)
        {
            var approved = Mapper.Map<HardwareApprovalDto, HardwareApproval>(hardwareApprovalDto);

            HardwareApproval approval = new HardwareApproval();
            hardwareApprovalDto.Id = approved.Id;
            approval.DateApprove = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            approval.HardwareUserRequestId = hardwareApprovalDto.HardwareUserRequestId;
            approval.TechnicianReportId = hardwareApprovalDto.TechnicianReportId;
            approval.HardwareVerifyId = hardwareApprovalDto.HardwareVerifyId;
            approval.EmailApproval = User.Identity.GetLogEmail();
            approval.NameApproval = User.Identity.GetFullName();
            approval.RemarksApproval = hardwareApprovalDto.RemarksApproval;
            _db.HardwareApprovals.Add(approval);

            _db.HardwareTasksLists.Add(new HardwareTasksList()
            {
                HardwareTaskId = 5,
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                HardwareUserRequestId = hardwareApprovalDto.HardwareUserRequestId,
                TechnicianReportId = hardwareApprovalDto.TechnicianReportId,
                HardwareVerifyId = hardwareApprovalDto.HardwareVerifyId,
                HardwareApprovalId = approved.Id

            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Approved Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + approved.Id), hardwareApprovalDto);
        }

        [HttpPut]
        [Route("api/update/approved/{id}")]
        public IHttpActionResult UpdateApproved(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var requestList = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);
            requestList.Id = id;
            hardwareUserRequestDto.Id = id;
            requestList.Status = "Approved";
            _db.SaveChanges();
            return Ok();
        }

        //Post //hardwareUserRequest v2
        [HttpPost]
        [Route("api/Manual/v2/SaveRequest")]
        public async Task<string> SaveManual4()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/HardwareImage/");

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            //var hardwareTech = _db.HardwareTechnician.SingleOrDefault(d => d.Id == hardwareRequestDto.HardwareTechnicianId).Email;

            var depName = _db.Departments.SingleOrDefault(d => d.Id == depId).Name;
            var divName = _db.Divisions.SingleOrDefault(d => d.Id == divId).Name;

            var userEmail = User.Identity.GetLogEmail();
            var techId = _db.HardwareTechnician.SingleOrDefault(x => x.Email == userEmail).Id;

            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);

            var provider = new MultipartFormDataStreamProvider(root);

            var ftpAddress = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpHost;
            var ftpUserName = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpUsername;
            var ftpPassword = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpPassword;

            FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
            ftpClient.Port = 21;
            ftpClient.Connect();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                HardwareUserRequest hardwareR = new HardwareUserRequest();

                if (hardwareR.Id == 0)
                {
                    hardwareR.DocumentLabel = provider.FormData["FileName"];
                    hardwareR.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    hardwareR.DateCreated = DateTime.Now;
                    hardwareR.FirstName = provider.FormData["firstName"];
                    hardwareR.MiddleName = provider.FormData["middleName"];
                    hardwareR.LastName = provider.FormData["LastName"];
                    hardwareR.FullName = hardwareR.FirstName + " " + hardwareR.MiddleName + " " + hardwareR.LastName;
                    hardwareR.Email = provider.FormData["email"];
                    hardwareR.MobileNumber = provider.FormData["mobileNumber"];
                    hardwareR.DepartmentsId = Convert.ToInt32(provider.FormData["departmentsId"]);
                    hardwareR.DepartmentName = _db.Departments.SingleOrDefault(x => x.Id == hardwareR.DepartmentsId).Name;
                    hardwareR.DivisionsId = Convert.ToInt32(provider.FormData["divisionsId"]);
                    hardwareR.DivisionName = _db.Divisions.SingleOrDefault(x => x.Id == hardwareR.DivisionsId).Name;
                    hardwareR.Description = provider.FormData["description"];
                    hardwareR.UnitTypeId = Convert.ToInt32(provider.FormData["unitTypeId"]);
                    hardwareR.UniTypes = _db.UnitType.SingleOrDefault(x => x.Id == hardwareR.UnitTypeId).Name;
                    hardwareR.BrandName = provider.FormData["brandName"];
                    hardwareR.ModelName = provider.FormData["modelName"];
                    hardwareR.HardwareId = Convert.ToInt32(provider.FormData["hardwareId"]);
                    hardwareR.HardwareName = _db.Hardware.SingleOrDefault(x => x.Id == hardwareR.HardwareId).Name;
                    hardwareR.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;
                    hardwareR.Status = "Manual";
                    hardwareR.IsManual = true;
                    _db.HardwareUserRequests.Add(hardwareR);


                    _db.HardwareTasksLists.Add(new HardwareTasksList()
                    {
                        HardwareTaskId = 1,
                        DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        HardwareUserRequestId = hardwareR.Id
                    });
                    _db.TechnicianReports.Add(new TechnicianReport()
                    {
                        DateAssigned = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        DateStarted = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        DateEnded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        TechEmail = User.Identity.GetLogEmail(),
                        TechnicianName = User.Identity.GetFullName(),
                        DateCreated = DateTime.Now,
                        HardwareTechnicianId = _db.HardwareTechnician.SingleOrDefault(x => x.Email == userEmail).Id,
                        HardwareUserRequestId = hardwareR.Id
                    });
                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a Manual Hardware Request",
                        ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetLogEmail(),
                        DepartmentName = User.Identity.GetDepartmentName(),
                        DivisionName = User.Identity.GetDivisionName(),
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
                                var codeNumber = Guid.NewGuid().ToString();
                                name = name.Trim('"');

                                var localFileName = file.LocalFileName;
                                var filePath = Path.Combine(root, codeNumber + name);

                                var folderName = _db.Departments.SingleOrDefault(x => x.Id == depId).Ftp.FolderName;

                                ftpClient.UploadFile(localFileName, folderName + codeNumber + name);

                                File.Delete(localFileName);



                                HardwareUserUploads hardwareuserUps = new HardwareUserUploads();
                                hardwareuserUps.FileName = codeNumber + name;
                                hardwareuserUps.ImagePath = _db.Departments.SingleOrDefault(x => x.Id == depId).Ftp.FolderName;
                                hardwareuserUps.HardwareUserRequestId = hardwareR.Id;
                                hardwareuserUps.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                hardwareuserUps.FileExtension = Path.GetExtension(name);
                                hardwareuserUps.FtpId = _db.Departments.SingleOrDefault(x => x.Id == depId).FtpId;
                                _db.HardwareUserUploads.Add(hardwareuserUps);
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
        [Route("api/edit/manual/techReport/{id}")]
        public IHttpActionResult TechUpdate(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var userInfoInDb = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);


            userInfoInDb.Id = id;
            hardwareUserRequestDto.Id = id;

            userInfoInDb.FirstName = hardwareUserRequestDto.FirstName;
            userInfoInDb.MiddleName = hardwareUserRequestDto.MiddleName;
            userInfoInDb.LastName = hardwareUserRequestDto.LastName;
            userInfoInDb.Email = hardwareUserRequestDto.Email;
            userInfoInDb.MobileNumber = hardwareUserRequestDto.MobileNumber;
            userInfoInDb.DepartmentsId = hardwareUserRequestDto.DepartmentsId;
            userInfoInDb.DepartmentName = _db.Departments.SingleOrDefault(x => x.Id == hardwareUserRequestDto.DepartmentsId).Name;
            userInfoInDb.DivisionsId = hardwareUserRequestDto.DivisionsId;
            userInfoInDb.DivisionName = _db.Divisions.SingleOrDefault(x => x.Id == hardwareUserRequestDto.DivisionsId).Name;
            userInfoInDb.HardwareId = hardwareUserRequestDto.HardwareId;
            userInfoInDb.HardwareName = _db.Hardware.SingleOrDefault(x => x.Id == hardwareUserRequestDto.HardwareId).Name;
            userInfoInDb.UnitTypeId = hardwareUserRequestDto.UnitTypeId;
            userInfoInDb.UniTypes = _db.UnitType.SingleOrDefault(x => x.Id == hardwareUserRequestDto.UnitTypeId).Name;
            userInfoInDb.ModelName = hardwareUserRequestDto.ModelName;
            userInfoInDb.BrandName = hardwareUserRequestDto.BrandName;
            userInfoInDb.DocumentLabel = hardwareUserRequestDto.DocumentLabel;
            userInfoInDb.Description = hardwareUserRequestDto.Description;

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edited a Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/v2/hardwareRequest/techReport/{id}")]
        public IHttpActionResult TechReport2(int id, TechnicianReportDto technicianReportDto)
        {

            var techReports = _db.TechnicianReports.SingleOrDefault(h => h.Id == id);


            technicianReportDto.Id = techReports.Id;
            techReports.HardwareId = technicianReportDto.HardwareId;
            techReports.HardwareName = _db.Hardware.SingleOrDefault(x => x.Id == techReports.HardwareId).Name;
            techReports.UnitTypeId = technicianReportDto.UnitTypeId;
            techReports.UniTypes = _db.UnitType.SingleOrDefault(x => x.Id == techReports.UnitTypeId).Name;
            techReports.BrandName = technicianReportDto.BrandName;
            techReports.HardwareUserRequestId = technicianReportDto.HardwareUserRequestId;
            techReports.ModelName = technicianReportDto.ModelName;
            techReports.FindingId = technicianReportDto.FindingId;
            techReports.FindingName = _db.Finding.SingleOrDefault(x => x.Id == techReports.FindingId).Name;
            techReports.PossibleCause = technicianReportDto.PossibleCause;
            techReports.SerialNumber = technicianReportDto.SerialNumber;
            techReports.ControlNumber = technicianReportDto.ControlNumber;
            techReports.Remarks = technicianReportDto.Remarks;
            techReports.Status = "Verified";
            techReports.DateEnded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            techReports.AdminName = User.Identity.GetFullName();
            techReports.SuperAdminId = 1;
            techReports.SuperName = _db.SuperAdmins.SingleOrDefault(x => x.Id == 1).Name;

            _db.HardwareVerifies.Add(new HardwareVerify()
            {

                DateVerified = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Name = User.Identity.GetFullName(),
                HardwareUserRequestId = technicianReportDto.HardwareUserRequestId,
                TechnicianReportId = techReports.Id,
                Email = User.Identity.GetLogEmail(),
            });

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Reported a Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            _db.HardwareTasksLists.Add(new HardwareTasksList()
            {
                HardwareTaskId = 3,
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                HardwareUserRequestId = technicianReportDto.HardwareUserRequestId,
                TechnicianReportId = techReports.Id
            });
            _db.HardwareTasksLists.Add(new HardwareTasksList()
            {
                HardwareTaskId = 4,
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                HardwareUserRequestId = technicianReportDto.HardwareUserRequestId,
                TechnicianReportId = techReports.Id
            });

            _db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Route("api/v3/hardwareRequest/techReport/{id}")]
        public IHttpActionResult Status(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var statusVerify = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);


            hardwareUserRequestDto.Id = statusVerify.Id;
            statusVerify.Status = "Verified";

            _db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Route("api/v4/hardwareRequest/techReport/{id}")]
        public IHttpActionResult Status2(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var returnStatus = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);


            hardwareUserRequestDto.Id = returnStatus.Id;
            returnStatus.Status = "In Progress";

            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/v5/hardwareRequest/techReport/{id}")]
        public IHttpActionResult TechReport3(int id, TechnicianReportDto technicianReportDto)
        {

            var techReports = _db.TechnicianReports.SingleOrDefault(h => h.Id == id);


            technicianReportDto.Id = techReports.Id;
            techReports.HardwareUserRequestId = technicianReportDto.HardwareUserRequestId;

            _db.HardwareTasksLists.Add(new HardwareTasksList()
            {
                HardwareTaskId = 2,
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                HardwareUserRequestId = technicianReportDto.HardwareUserRequestId,
                TechnicianReportId = techReports.Id

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/Harware/Delete/{id}")]
        public IHttpActionResult Hardwaresoft(int id)
        {
            var hardwareS = _db.TechnicianReports.SingleOrDefault(h => h.Id == id);
            if (hardwareS == null)
            {
                return NotFound();
            }

            _db.TechnicianReports.Remove(hardwareS);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Developer Delete A Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }
        [HttpDelete]
        [Route("api/Harware/DeleteUps/{id}")]
        public IHttpActionResult hardwareUps(int id)
        {
            var hardwareS = _db.TechnicianUploads.SingleOrDefault(h => h.Id == id);
            if (hardwareS == null)
            {
                return NotFound();
            }

            _db.TechnicianUploads.Remove(hardwareS);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Developer Delete A Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/Harware/Deleteverified/{id}")]
        public IHttpActionResult HardwareVerifiy(int id)
        {
            var hardwareS = _db.HardwareVerifies.SingleOrDefault(h => h.Id == id);
            if (hardwareS == null)
            {
                return NotFound();
            }

            _db.HardwareVerifies.Remove(hardwareS);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Developer Delete A Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/Harware/DeleteApproved/{id}")]
        public IHttpActionResult HardwareApproved(int id)
        {
            var hardwareS = _db.HardwareApprovals.SingleOrDefault(h => h.Id == id);
            if (hardwareS == null)
            {
                return NotFound();
            }

            _db.HardwareApprovals.Remove(hardwareS);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Developer Delete A Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }


        [HttpDelete]
        [Route("api/Harware/DeleteTask/{id}")]
        public IHttpActionResult HardwareTask(int id)
        {
            var hardwareS = _db.HardwareTasksLists.SingleOrDefault(h => h.Id == id);
            if (hardwareS == null)
            {
                return NotFound();
            }

            _db.HardwareTasksLists.Remove(hardwareS);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Developer Delete A Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }



        [HttpDelete]
        [Route("api/Harware/DeleteApprove/{id}")]
        public IHttpActionResult HardwareApprove(int id)
        {
            var hardwareS = _db.HardwareApprovals.SingleOrDefault(h => h.Id == id);
            if (hardwareS == null)
            {
                return NotFound();
            }

            _db.HardwareApprovals.Remove(hardwareS);

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Developer Delete A Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }


        [HttpPost]
        [Route("api/accepts/hr/save")]
        public IHttpActionResult AccepstsHardware2(HardwareAcceptsRequestDto hardwareAcceptsRequestDto)
        {
            var acceptshardware = Mapper.Map<HardwareAcceptsRequestDto, HardwareAcceptsRequest>(hardwareAcceptsRequestDto);
            HardwareAcceptsRequest approved = new HardwareAcceptsRequest();

            hardwareAcceptsRequestDto.Id = acceptshardware.Id;
            approved.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            approved.Email = User.Identity.GetUserName();
            approved.FullName = User.Identity.GetFullName();
            approved.DivisionsId = Convert.ToInt32(User.Identity.GetUserDivision());
            approved.DepartmentsId = Convert.ToInt32(User.Identity.GetUserDepartment());
            approved.DivisionName = _db.Divisions.SingleOrDefault(x => x.Id == approved.DivisionsId).Name;
            approved.DepartmentName = _db.Departments.SingleOrDefault(x => x.Id == approved.DepartmentsId).Name;
            approved.HardwareUserRequestId = hardwareAcceptsRequestDto.HardwareUserRequestId;
            approved.IsAccept = "seen";


            _db.HardwareAcceptsRequests.Add(approved);

            _db.HardwareTasksLists.Add(new HardwareTasksList()
            {
                HardwareTaskId = 6,
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                HardwareAcceptsRequestId = approved.Id,
                HardwareUserRequestId = hardwareAcceptsRequestDto.HardwareUserRequestId,
            });




            var devNumber = "09165778160";
            var mich = "09666443665";
            var mark = "09491511839";
            var jess = "09182695626";

            Db.Drafts.Add(new Draft
            {
                Sendto = mich,
                msg = "New Hardware Request" + " Department :" + approved.DepartmentName + " Division : " + approved.DivisionName,
                tag = 0
            });
            Db.Drafts.Add(new Draft
            {
                Sendto = devNumber,
                msg = "New Hardware Request" + " Department :" + approved.DepartmentName + " Division : " + approved.DivisionName,
                tag = 0
            });
            Db.Drafts.Add(new Draft
            {
                Sendto = mark,
                msg = "New Hardware Request" + " Department :" + approved.DepartmentName + " Division : " + approved.DivisionName,
                tag = 0
            });
            Db.Drafts.Add(new Draft
            {
                Sendto = jess,
                msg = "New Hardware Request" + " Department :" + approved.DepartmentName + " Division : " + approved.DivisionName,

                tag = 0
            });
            Db.SaveChanges();

            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + approved.Id), hardwareAcceptsRequestDto);


        }

        [HttpPut]
        [Route("api/hardware/acceptsReturn/{id}")]
        public IHttpActionResult HardwareReturn(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var hardwareUser = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);


            hardwareUserRequestDto.Id = hardwareUser.Id;
            hardwareUser.Status = "Pending Division Approval";
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Hardware Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),

            });
            _db.SaveChanges();
            return Ok();
        }



        [HttpPut]
        [Route("api/v2/sms/adminApprover/{id}")]
        public IHttpActionResult SendSmsAdmin(int id, HardwareUserRequestDto hardwareUserRequestDto)
        {

            var hardwareSms = _db.HardwareUserRequests.SingleOrDefault(h => h.Id == id);


            hardwareUserRequestDto.Id = hardwareSms.Id;

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Send a Message",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
            });

            Db.Drafts.Add(new Draft
            {
                Sendto = hardwareUserRequestDto.MobileNumber,
                msg = hardwareUserRequestDto.SmsMessage + "This message is system-generated. No need to reply. Thank you. From: Office of the City Management Information System",
                tag = 0
            });
            Db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }

    }
}
