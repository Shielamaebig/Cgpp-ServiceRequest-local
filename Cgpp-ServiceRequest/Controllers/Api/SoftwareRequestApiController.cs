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
using System.Runtime.Remoting.Metadata.W3cXsd2001;
//using Cgpp_ServiceRequest.Migrations;
using System.Web.Helpers;
using System.Threading;
using System.Collections;
using System.Linq.Dynamic;
using Microsoft.Owin;
using System.Security.Cryptography;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class SoftwareRequestApiController : ApiController
    {
        private ApplicationDbContext _db;

        IlsContext Db = new IlsContext();
        public SoftwareRequestApiController()
        {
            _db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }


        [Route("api/getuseremail")]
        public IHttpActionResult GetUserEmail()
        {

            var progEmail = User.Identity.GetUserEmail();
            return Ok(progEmail);

        }
        [Route("api/list")]
        public IHttpActionResult GetList()
        {
            var result = _db.SoftwareAcceptsRequests.ToList().Select(Mapper.Map<SoftwareAcceptsRequest, SoftwareAcceptsRequestDto>);
            return Ok(result);
        }
        // working

        [Route("api/softwareReques/list")]
        public IHttpActionResult GetAllList()
        {
            var result = _db.SoftwareUserRequests.ToList();
            return Ok(result.OrderByDescending(x => x.Id).Take(8));
        }
        // working
        [HttpGet]
        [Route("api/v2/sr/assign")]
        public IHttpActionResult GetDashProgrammers()
        {
            var progDto = _db.ProgrammerReport
                .Include(x => x.Software)
                .Include(x => x.SoftwareUserRequest)
                .ToList().Select(Mapper.Map<ProgrammerReport, ProgrammerReportDto>);
            if (User.IsInRole("Programmers") || User.IsInRole("Technicians/Programmer"))
            {
                var progEmail = User.Identity.GetUserEmail();
                progDto = new List<ProgrammerReportDto>(progDto.Where(x => x.ProgrammerEmail == progEmail));
            }
            return Ok(progDto.OrderByDescending(x => x.Id).Take(5));
        }




        [HttpGet]
        [Route("api/info/System")]
        public IHttpActionResult GetInformationSystem()
        {
            var information = _db.InformationSystem.ToList();
            return Ok(information);
        }





        //POST with File Attachment
        //Post
        [HttpPost]
        [Route("api/Software/SaveRequest")]
        public async Task<string> SaveUpload()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/SoftwareImage/");
            var devNumber = "09165778160";
            var SAdminNumber = "09178450776";
            var AdminNumber = "09158814555";
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

                SoftwareUserRequest softwareRequest = new SoftwareUserRequest();

                if (softwareRequest.Id == 0)
                {
                    softwareRequest.DocumentLabel = provider.FormData["FileName"];
                    softwareRequest.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    softwareRequest.FullName = User.Identity.GetFirstName() + " " + User.Identity.GetMiddleName() + " " + User.Identity.GetLastName();
                    softwareRequest.FirstName = User.Identity.GetFirstName();
                    softwareRequest.LastName = User.Identity.GetLastName();
                    softwareRequest.MiddleName = User.Identity.GetMiddleName();
                    softwareRequest.Email = User.Identity.GetUserEmail();
                    softwareRequest.MobileNumber = User.Identity.GetUserMobileNumber();
                    softwareRequest.DepartmentsId = depId;
                    softwareRequest.DepartmentName = _db.Departments.SingleOrDefault(x => x.Id == depId).Name;
                    softwareRequest.DateCreated = DateTime.Now;
                    softwareRequest.DivisionsId = divId;
                    softwareRequest.DivisionName = _db.Divisions.SingleOrDefault(x => x.Id == divId).Name;
                    softwareRequest.Description = provider.FormData["description"];
                    softwareRequest.InformationSystemId = Convert.ToInt32(provider.FormData["informationSystemId"]);
                    softwareRequest.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == softwareRequest.InformationSystemId).Name;
                    softwareRequest.RequestFor = provider.FormData["requestFor"];
                    softwareRequest.SoftwareId = Convert.ToInt32(provider.FormData["softwareId"]);
                    softwareRequest.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == softwareRequest.SoftwareId).Name;
                    softwareRequest.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;
                    softwareRequest.IsNew = true;
                    softwareRequest.Status = "Pending Division Approval";
                    _db.SoftwareUserRequests.Add(softwareRequest);

                    _db.SoftwareTaskLists.Add(new SoftwareTaskList()
                    {
                        SoftwareTaskId = 1,
                        DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        SoftwareUserRequestId = softwareRequest.Id,
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
                        SoftwareUserRequestId = softwareRequest.Id,
                    });

                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a new Software Request",
                        ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetLogEmail(),
                        DepartmentName = User.Identity.GetDepartmentName(),
                        DivisionName = User.Identity.GetDivisionName(),
                    });

                    var isApprover = _db.Divisions.SingleOrDefault(x => x.Id == divId).IsDivisionApprover;
                    var deptApprover = _db.Departments.SingleOrDefault(x => x.Id == depId).IsDepartmentApprover;

                    if (deptApprover == true)
                    {
                        softwareRequest.Status = "Pending Department Approval";

                        var roles = _db.Roles.Where(r => r.Name == "DepartmentApprover");

                        var approvers = new List<ApplicationUser>();

                        if (roles.Any())
                        {
                            var roleId = roles.First().Id;
                            approvers = _db.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == roleId) && u.DepartmentsId == softwareRequest.DepartmentsId).ToList();


                        }

                        foreach (var approver in approvers)
                        {

                            Db.Drafts.Add(new Draft
                            {
                                Sendto = approver.MobileNumber,
                                msg = " New Hardware Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Department : " + " " + softwareRequest.DepartmentName,
                                tag = 0
                            });
                        }



                        Db.SaveChanges();
                    }
                    else if (isApprover == true)
                    {
                        softwareRequest.Status = "Pending Division Approval";

                        var roles = _db.Roles.Where(r => r.Name == "DivisionApprover");
                        var approvers = new List<ApplicationUser>();
                        if (roles.Any())
                        {
                            var rolesId = roles.First().Id;
                            approvers = _db.Users.Include(x => x.Roles).Where(u => u.Roles.Any(r => r.RoleId == rolesId) && u.DivisionsId == softwareRequest.DivisionsId).ToList();
                        }

                        foreach (var divA in approvers)
                        {
                            Db.Drafts.Add(new Draft
                            {
                                Sendto = divA.MobileNumber,
                                msg = "ok",
                                tag = 0
                            });
                        }
                        Db.SaveChanges();
                    }

                    else
                    {
                        softwareRequest.Status = "Open";

                        Db.Drafts.Add(new Draft
                        {
                            Sendto = SAdminNumber,
                            msg = " New Hardware Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Department : " + " " + softwareRequest.DepartmentName,
                            tag = 0
                        });
                        Db.Drafts.Add(new Draft
                        {
                            Sendto = devNumber,
                            msg = " New Hardware Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Department : " + " " + softwareRequest.DepartmentName,
                            tag = 0
                        });
                        Db.Drafts.Add(new Draft
                        {
                            Sendto = AdminNumber,
                            msg = " New Hardware Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Department : " + " " + softwareRequest.DepartmentName,
                            tag = 0
                        });
                        Db.SaveChanges();
                    }
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

                                SoftwareUserUploads softwareUploads = new SoftwareUserUploads();
                                softwareUploads.FileName = codeNumber + name;
                                softwareUploads.ImagePath = folderName;
                                softwareUploads.SoftwareUserRequestId = softwareRequest.Id;
                                softwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                softwareUploads.FileExtension = Path.GetExtension(name);
                                softwareUploads.FtpId = _db.Departments.SingleOrDefault(x => x.Id == depId).FtpId;

                                _db.SoftwareUserUploads.Add(softwareUploads);
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
            //ftpClient.Disconnect();
            return "File uploaded!";

        }



        [HttpGet]
        [Route("api/usertask/sf/getbyId/{id}")]
        public IHttpActionResult GetTask(int id)
        {
            var sftask = _db.SoftwareTaskLists.Include(x => x.SoftwareUserRequest).Include(x => x.SoftwareAcceptsRequest).Include(x => x.SoftwareTask).Include(x => x.ProgrammerReport).Include(x => x.SoftwareVerification).Include(x => x.SoftwareApproved).ToList().Select(Mapper.Map<SoftwareTaskList, SoftwareTaskListDto>);
            sftask = new List<SoftwareTaskListDto>(sftask.Where(x => x.SoftwareUserRequestId == id));

            return Ok(sftask);
        }

        //Get Department
        [Route("api/softwareRequest/department")]
        [HttpGet]
        public IHttpActionResult GetDepartment()
        {
            var department = _db.Departments.ToList().OrderByDescending(x => x.Id); ;
            return Ok(department);
        }
        //Get Division
        [Route("api/softwareRequest/division")]
        [HttpGet]
        public IHttpActionResult GetDivision()
        {
            var division = _db.Divisions.ToList();
            return Ok(division);
        }
        //Software
        [Route("api/softwareRequest/software")]
        [HttpGet]
        public IHttpActionResult GetSoftware()
        {
            var software = _db.Software.ToList();
            return Ok(software);
        }
        //SYstem
        [Route("api/softwareRequest/infoSystem")]
        [HttpGet]
        public IHttpActionResult GetSystem()
        {
            var inforSystem = _db.InformationSystem.ToList();
            return Ok(inforSystem);
        }

        //Technician
        [Route("api/softwareRequest/Technician")]
        [HttpGet]

        public IHttpActionResult GetTechnician()
        {
            var softTech = _db.SoftwareTechnician.ToList();
            return Ok(softTech);
        }




        [HttpGet]
        [Route("api/software/countReq")]
        public IHttpActionResult GetCountsoftware()
        {
            var softwareReqDto = _db.SoftwareUserRequests.ToList();
            return Ok(softwareReqDto.Count);
        }
        [HttpGet]
        [Route("api/software/requestbyUser")]
        public IHttpActionResult getsoftwareCount()
        {
            var srDto = _db.SoftwareUserRequests
                .ToList();
            if (User.IsInRole("Users"))
            {
                var rEmail = User.Identity.GetUserEmail();
                srDto = new List<SoftwareUserRequest>(srDto.Where(x => x.Email == rEmail));
            }
            return Ok(srDto.Count);
        }

        [HttpGet]
        [Route("api/software/requestbyUserList")]
        public IHttpActionResult getsoftwareList()
        {
            var srDto = _db.SoftwareUserRequests
                .Include(x => x.Software)
                .ToList();
            if (User.IsInRole("Users"))
            {
                var rEmail = User.Identity.GetUserEmail();
                srDto = new List<SoftwareUserRequest>(srDto.Where(x => x.Email == rEmail));
            }
            //srDto.OrderByDescending(u => u.Id)
            return Ok(srDto.OrderByDescending(x => x.Id).Take(10));
        }

        [HttpDelete]
        [Route("api/Software/Delete/{id}")]
        public IHttpActionResult DeleteSoftwareh(int id)
        {
            var softwareReq = _db.ProgrammerReport.SingleOrDefault(h => h.Id == id);
            if (softwareReq == null)
            {
                return NotFound();
            }

            _db.ProgrammerReport.Remove(softwareReq);

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
        [Route("api/softwareUsers/Remove/{id}")]
        public IHttpActionResult RemoveUsersRequest(int id)
        {
            var softwareReq = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);
            if (softwareReq == null)
            {
                return NotFound();
            }

            _db.SoftwareUserRequests.Remove(softwareReq);

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
        [Route("api/softwareAccepts/Remove/{id}")]
        public IHttpActionResult RemoveAccepts(int id)
        {
            var softwareReq = _db.SoftwareAcceptsRequests.SingleOrDefault(h => h.Id == id);
            if (softwareReq == null)
            {
                return NotFound();
            }

            _db.SoftwareAcceptsRequests.Remove(softwareReq);

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
        [Route("api/Uploads/Remove/{id}")]
        public IHttpActionResult RemoveUploafs(int id)
        {
            var softwareReq = _db.ProgrammerUploads.SingleOrDefault(h => h.Id == id);
            if (softwareReq == null)
            {
                return NotFound();
            }

            _db.ProgrammerUploads.Remove(softwareReq);

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
        [Route("api/taskList/Delete/{id}")]
        public IHttpActionResult DeleteTaskList(int id)
        {
            var taskList = _db.SoftwareTaskLists.SingleOrDefault(h => h.Id == id);
            if (taskList == null)
            {
                return NotFound();
            }

            _db.SoftwareTaskLists.Remove(taskList);

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
        [Route("api/approve/Delete/{id}")]
        public IHttpActionResult ApproveRemove(int id)
        {
            var softwareA = _db.SoftwareApproveds.SingleOrDefault(h => h.Id == id);
            if (softwareA == null)
            {
                return NotFound();
            }

            _db.SoftwareApproveds.Remove(softwareA);

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
        [Route("api/softwareVerified/Delete/{id}")]
        public IHttpActionResult DeleteVerfied(int id)
        {
            var sv = _db.SoftwareVerification.SingleOrDefault(h => h.Id == id);
            if (sv == null)
            {
                return NotFound();
            }

            _db.SoftwareVerification.Remove(sv);

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
        //Count Report

        [HttpGet]
        [Route("api/sf/bymonth")]
        public IHttpActionResult RequestbyMonth()
        {
            var result = _db.ProgrammerReport.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.SoftwareTechnician.Name })
                            .Where(grp => grp.Count() > 0)
                            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                            .OrderBy(x => x.Year).ThenBy(x => x.Month)
                            .ToList();
            return Ok(result);

        }

        [HttpGet]
        [Route("api/sftService/count")]
        public IHttpActionResult SoftwareCategoryCount()
        {

            var result = _db.SoftwareUserRequests.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.Software.Name })
                    .Where(grp => grp.Count() > 0)
                    .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                    .OrderBy(x => x.Year).ThenBy(x => x.Month)
                    .ToList();
            return Ok(result);
        }
        [HttpGet]
        [Route("api/sftSystem/count")]
        public IHttpActionResult SystemCountList()
        {
            var result = _db.SoftwareUserRequests.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.InformationSystem.Name })
                .Where(grp => grp.Count() > 0)
                .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();
            return Ok(result);
        }









        [HttpGet]
        [Route("api/v2/Approver/getbyId/{id}")]
        public IHttpActionResult GetTech(int id)
        {
            var accepts = _db.SoftwareUserRequests.SingleOrDefault(x => x.Id == id);
            if (accepts == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<SoftwareUserRequest, SoftwareUserRequestDto>(accepts));
        }

        [HttpGet]
        [Route("api/v2/Programmer/getbyId/{id}")]
        public IHttpActionResult GetProg4(int id)
        {
            var accepts = _db.SoftwareAcceptsRequests.SingleOrDefault(x => x.Id == id);
            if (accepts == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<SoftwareAcceptsRequest, SoftwareAcceptsRequestDto>(accepts));
        }


        [HttpGet]
        [Route("api/v2/uploads/{id}")]
        public IHttpActionResult Uploads2(int id)
        {
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            var uploadDisplay = _db.SoftwareUserUploads.Include(x => x.SoftwareUserRequest).ToList().Select(Mapper.Map<SoftwareUserUploads, SoftwareUserUploadsDto>);
            uploadDisplay = new List<SoftwareUserUploadsDto>(uploadDisplay.Where(x => x.SoftwareUserRequestId == id));

            var ftpAddress = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpHost;
            var ftpUserName = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpUsername;
            var ftpPassword = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpPassword;


            var softwareblob = new List<SoftwareUserUploadsDto>();
            using (var ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword))
            {
                ftpClient.Port = 21;
                ftpClient.Connect();

                foreach (var s in uploadDisplay)
                {
                    byte[] file;
                    ftpClient.DownloadBytes(out file, s.ImagePath + s.FileName);
                    s.DocumentBlob = file;
                    softwareblob.Add(s);
                }
                ftpClient.Disconnect();
            }
            return Ok(uploadDisplay);
        }

        [HttpPut]
        [Route("api/sofrware/approve/{id}")]
        public IHttpActionResult UpdateApprover2(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            var softwareInDb = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);

            softwareInDb.Id = id;
            softwareUserRequestDto.Id = id;
            softwareInDb.Status = "Open";
            softwareInDb.IsNew = false;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Accepts Request of Division Personnel",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        //[HttpPost]
        //[Route("api/accepts/save")]
        //public IHttpActionResult Accepsts2(SoftwareAcceptsRequestDto softwareAcceptsRequestDto)
        //{
        //    int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
        //    int divId = Convert.ToInt32(User.Identity.GetUserDivision());
        //    var useEmail = User.Identity.GetUserEmail();
        //    var accept = Mapper.Map<SoftwareAcceptsRequestDto, SoftwareAcceptsRequest>(softwareAcceptsRequestDto);

        //    SoftwareAcceptsRequest accepted = new SoftwareAcceptsRequest();

        //    softwareAcceptsRequestDto.Id = accept.Id;
        //    accepted.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
        //    accepted.Email = User.Identity.GetUserEmail();
        //    accepted.FullName = User.Identity.GetFullName();
        //    accepted.DepartmentsId = depId;
        //    accepted.DivisionsId = divId;
        //    accepted.DivisionName = User.Identity.GetDepartmentName();
        //    accepted.DepartmentName = User.Identity.GetDivisionName();
        //    accepted.SoftwareUserRequestId = softwareAcceptsRequestDto.SoftwareUserRequestId;
        //    accepted.IsAccept = "seen";
        //    _db.SoftwareAcceptsRequests.Add(accepted);
        //    _db.SoftwareTaskLists.Add(new SoftwareTaskList()
        //    {
        //        DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
        //        SoftwareTaskId = 2,
        //        SoftwareUserRequestId = softwareAcceptsRequestDto.SoftwareUserRequestId,
        //        SoftwareAcceptsRequestId = accept.Id,
        //    });
        //    _db.SaveChanges();
        //    return Created(new Uri(Request.RequestUri + "/" + accept.Id), softwareAcceptsRequestDto);
        //}
        [HttpPost]
        [Route("api/accepts/save")]
        public IHttpActionResult Accepsts2(SoftwareAcceptsRequestDto softwareAcceptsRequestDto)
        {
            var accepts = Mapper.Map<SoftwareAcceptsRequestDto, SoftwareAcceptsRequest>(softwareAcceptsRequestDto);
            SoftwareAcceptsRequest acceptsRequest = new SoftwareAcceptsRequest();

            softwareAcceptsRequestDto.Id = accepts.Id;
            acceptsRequest.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            acceptsRequest.Email = User.Identity.GetUserName();
            acceptsRequest.FullName = User.Identity.GetFullName();
            acceptsRequest.DivisionsId = Convert.ToInt32(User.Identity.GetUserDivision());
            acceptsRequest.DepartmentsId = Convert.ToInt32(User.Identity.GetUserDepartment());
            acceptsRequest.DepartmentName = _db.Departments.SingleOrDefault(x => x.Id == acceptsRequest.DepartmentsId).Name;
            acceptsRequest.DivisionName = _db.Divisions.SingleOrDefault(x => x.Id == acceptsRequest.DivisionsId).Name;
            acceptsRequest.SoftwareUserRequestId = softwareAcceptsRequestDto.SoftwareUserRequestId;
            acceptsRequest.IsAccept = "seen";
            _db.SoftwareAcceptsRequests.Add(acceptsRequest);
            _db.SoftwareTaskLists.Add(new SoftwareTaskList()
            {
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                SoftwareTaskId = 2,
                SoftwareUserRequestId = softwareAcceptsRequestDto.SoftwareUserRequestId,
                SoftwareAcceptsRequestId = accepts.Id,
            });
            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + accepts.Id), softwareAcceptsRequestDto);
        }
        [HttpPut]
        [Route("api/sofrware/ReturnApproved/{id}")]
        public IHttpActionResult ReturnAps(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            var softwareInDb = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);

            softwareInDb.Id = id;
            softwareUserRequestDto.Id = id;
            softwareInDb.Status = "Open";
            softwareInDb.IsNew = false;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            _db.SaveChanges();
            return Ok();
        }
        [Route("api/sf/v4/getnewRequesth")]
        public IHttpActionResult GetHardwareNewRequest4()
        {

            var open = "Open";
            var hrDto = _db.SoftwareUserRequests.ToList().Select(Mapper.Map<SoftwareUserRequest, SoftwareUserRequestDto>);
            return Ok(hrDto.Where(x => x.Status == open).OrderByDescending(x => x.DateAdded));
        }
        [HttpGet]
        [Route("api/v2/Prog/getbyId/{id}")]
        public IHttpActionResult GetProg(int id)
        {
            var prog = _db.SoftwareAcceptsRequests.Include(x => x.SoftwareUserRequest).SingleOrDefault(x => x.Id == id);
            if (prog == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<SoftwareAcceptsRequest, SoftwareAcceptsRequestDto>(prog));
        }
        [HttpGet]
        [Route("api/v3/Prog/getbyId/{id}")]
        public IHttpActionResult GetProgmmer(int id)
        {
            var prog = _db.SoftwareUserRequests.Include(x => x.Departments).Include(x=>x.Divisions).Include(x=>x.Software).SingleOrDefault(x => x.Id == id);
            if (prog == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<SoftwareUserRequest, SoftwareUserRequestDto>(prog));
        }
        [HttpGet]
        [Route("api/v2/user/getbyId/{id}")]
        public IHttpActionResult GetsfUser(int id)
        {
            var userRequest = _db.SoftwareUserRequests.SingleOrDefault(x => x.Id == id);
            if (userRequest == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<SoftwareUserRequest, SoftwareUserRequestDto>(userRequest));
        }

        [HttpPut]
        [Route("api/software/editRequest/{id}")]
        public IHttpActionResult SOftwareUserUpdate(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);


            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.SoftwareId = softwareUserRequestDto.SoftwareId;
            softwareRequest.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == softwareUserRequestDto.SoftwareId).Name;
            softwareRequest.InformationSystemId = softwareUserRequestDto.InformationSystemId;
            softwareRequest.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == softwareUserRequestDto.InformationSystemId).Name;
            softwareRequest.RequestFor = softwareUserRequestDto.RequestFor;
            softwareRequest.DocumentLabel = softwareUserRequestDto.DocumentLabel;
            softwareRequest.Description = softwareUserRequestDto.Description;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edit a Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }


        [HttpPut]
        [Route("api/software/editRequesttech/{id}")]
        public IHttpActionResult SOftwareUserUpdatetech(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);


            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.SoftwareId = softwareUserRequestDto.SoftwareId;
            softwareRequest.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == softwareUserRequestDto.SoftwareId).Name;
            softwareRequest.InformationSystemId = softwareUserRequestDto.InformationSystemId;
            softwareRequest.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == softwareUserRequestDto.InformationSystemId).Name;
            softwareRequest.RequestFor = softwareUserRequestDto.RequestFor;
            softwareRequest.DocumentLabel = softwareUserRequestDto.DocumentLabel;
            softwareRequest.Description = softwareUserRequestDto.Description;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edit a Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            var devNumber = "09165778160";
            var SAdminNumber = "09178450776";
            var AdminNumber = "09158814555";
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            var isApprover = _db.Divisions.SingleOrDefault(x => x.Id == divId).IsDivisionApprover;
            var deptApprover = _db.Departments.SingleOrDefault(x => x.Id == depId).IsDepartmentApprover;

            if (deptApprover == true)
            {
                softwareRequest.Status = "Pending Department Approval";

                var roles = _db.Roles.Where(r => r.Name == "DepartmentApprover");

                var approvers = new List<ApplicationUser>();

                if (roles.Any())
                {
                    var roleId = roles.First().Id;
                    approvers = _db.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == roleId) && u.DepartmentsId == softwareRequest.DepartmentsId).ToList();


                }

                foreach (var approver in approvers)
                {

                    Db.Drafts.Add(new Draft
                    {
                        Sendto = approver.MobileNumber,
                        msg = " Updated Software Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Department : " + " " + softwareRequest.DepartmentName,
                        tag = 0
                    });
                }



                Db.SaveChanges();
            }
            else if (isApprover == true)
            {
                softwareRequest.Status = "Pending Division Approval";

                var roles = _db.Roles.Where(r => r.Name == "DivisionApprover");
                var approvers = new List<ApplicationUser>();
                if (roles.Any())
                {
                    var rolesId = roles.First().Id;
                    approvers = _db.Users.Include(x => x.Roles).Where(u => u.Roles.Any(r => r.RoleId == rolesId) && u.DivisionsId == softwareRequest.DivisionsId).ToList();
                }

                foreach (var divA in approvers)
                {
                    Db.Drafts.Add(new Draft
                    {
                        Sendto = divA.MobileNumber,
                        msg = " Updated Software Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Department : " + " " + softwareRequest.DepartmentName,
                        tag = 0
                    });
                }
                Db.SaveChanges();
            }

            else
            {
                softwareRequest.Status = "Open";

                Db.Drafts.Add(new Draft
                {
                    Sendto = SAdminNumber,
                    msg = " Updated Software Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Ticket No. : " + " " + softwareRequest.Ticket,
                    tag = 0
                });
                Db.Drafts.Add(new Draft
                {
                    Sendto = devNumber,
                    msg = " Updated Software Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Ticket No. : " + " " + softwareRequest.Ticket,
                    tag = 0
                });
                Db.Drafts.Add(new Draft
                {
                    Sendto = AdminNumber,
                    msg = " Updated Software Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Ticket No. : " + " " + softwareRequest.Ticket,
                    tag = 0
                });
                Db.SaveChanges();
            }
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/returnRequest/{id}")]
        public IHttpActionResult SoftwareReturn(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);


            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.SoftwareId = softwareUserRequestDto.SoftwareId;
            softwareRequest.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == softwareUserRequestDto.SoftwareId).Name;
            softwareRequest.InformationSystemId = softwareUserRequestDto.InformationSystemId;
            softwareRequest.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == softwareUserRequestDto.InformationSystemId).Name;
            softwareRequest.RequestFor = softwareUserRequestDto.RequestFor;
            softwareRequest.DocumentLabel = softwareUserRequestDto.DocumentLabel;
            softwareRequest.Description = softwareUserRequestDto.Description;
            softwareRequest.Status = "Update Request";
            softwareRequest.IsNew = true;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edit a Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/cancelRequest/{id}")]
        public IHttpActionResult SOftwareUserCancel(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);


            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.IsNew = false;
            softwareRequest.Status = "Cancel";
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Cancelled a Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/status/{id}")]
        public IHttpActionResult statusChange(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);


            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.Status = softwareUserRequestDto.Status;

            _db.SaveChanges();
            return Ok();
        }

        [Route("api/sur/deleteImage/{id}")]
        public IHttpActionResult DeleteImage2(int id)
        {
            var file = _db.SoftwareUserUploads.SingleOrDefault(x => x.Id == id);
            if (file == null)
            {
                return NotFound();
            }
            _db.SoftwareUserUploads.Remove(file);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted a Software Request Image file",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            _db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/v4/saveFile")]
        public async Task<string> saveAddImage()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/SoftwareImage/");


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);

            var provider = new MultipartFormDataStreamProvider(root);
            //var ftpAddress = "172.16.100.249";
            //var ftpUserName = "cgpp-SR";
            //var ftpPassword = "T]ZXm84q";

            //var ftpAddress = "192.168.1.2";
            //var ftpUserName = "shielamaemalaque2022@outlook.com";
            //var ftpPassword = "Malaque@22+08";

            //var ftpAddress = "192.168.71.148";
            //var ftpAddress = "172.16.1.225";
            var ftpAddress = "192.168.1.2";
            var ftpUserName = "shielamaemalaque2022@outlook.com";
            var ftpPassword = "Malaque@22+08";
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
                            if (key == "softwareUserRequestId")
                            {
                                var name = file.Headers
                                    .ContentDisposition
                                    .FileName;

                                // remove double quotes from string.
                                name = name.Trim('"');
                                var dateNew = Convert.ToString(DateTime.Now.Ticks) + "-";

                                var localFileName = file.LocalFileName;
                                var filePath = Path.Combine(root, dateNew + name);

                                ftpClient.UploadFile(localFileName, "/softwareimage/" + dateNew + name);

                                File.Move(localFileName, filePath);

                                SoftwareUserUploads softwareUps = new SoftwareUserUploads();
                                softwareUps.FileName = name;
                                softwareUps.ImagePath = "/softwareimage/" + dateNew + name;
                                softwareUps.SoftwareUserRequestId = Convert.ToInt32(val);
                                softwareUps.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                _db.SoftwareUserUploads.Add(softwareUps);

                                _db.LoginActivity.Add(new LoginActivity()
                                {
                                    UserName = User.Identity.GetFullName(),
                                    ActivityMessage = "Added a Image",
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
        [Route("api/software/assignedRequest/{id}")]
        public IHttpActionResult SoftwareAssigned(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {
            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);
            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.Status = "In Progress";
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Assigned Software Request to me ",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/Accepts/{id}")]
        public IHttpActionResult Software(int id, SoftwareAcceptsRequestDto softwareAcceptsRequestDto)
        {
            var softwareAccepts = _db.SoftwareAcceptsRequests.SingleOrDefault(h => h.Id == id);
            softwareAcceptsRequestDto.Id = softwareAccepts.Id;
            softwareAccepts.IsAccept = "seen";
            _db.SaveChanges();
            return Ok();
        }

        //Programmer
        [HttpPost]
        [Route("api/assign/save")]
        public IHttpActionResult Approved(ProgrammerReportDto programmerReportDto)
        {
            var useEmail = User.Identity.GetUserEmail();
            var assign = Mapper.Map<ProgrammerReportDto, ProgrammerReport>(programmerReportDto);

            ProgrammerReport assigned = new ProgrammerReport();

            programmerReportDto.Id = assign.Id;
            assigned.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            assigned.DateCreated = DateTime.Now;
            assigned.DateAssigned = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            assigned.ProgrammerName = User.Identity.GetFullName();
            assigned.ProgrammerEmail = User.Identity.GetUserEmail();
            assigned.SoftwareTechnicianName = User.Identity.GetFullName();
            assigned.SoftwareTechnicianId = _db.SoftwareTechnician.SingleOrDefault(x => x.TechEmail == useEmail).Id;
            assigned.SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId;
            _db.ProgrammerReport.Add(assigned);

            _db.SoftwareTaskLists.Add(new SoftwareTaskList()
            {
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                SoftwareTaskId = 3,
                SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId,
                ProgrammerReportId = assign.Id,
            });

            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + assign.Id), programmerReportDto);
        }

        //Programmer
        [HttpPost]
        [Route("api/admin/assign/save")]
        public IHttpActionResult ApprovedbyAdmin(ProgrammerReportDto programmerReportDto)
        {
            var useEmail = User.Identity.GetUserEmail();
            var assign = Mapper.Map<ProgrammerReportDto, ProgrammerReport>(programmerReportDto);

            ProgrammerReport assigned = new ProgrammerReport();

            programmerReportDto.Id = assign.Id;
            assigned.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            assigned.DateCreated = DateTime.Now;
            assigned.ProgrammerName = _db.SoftwareTechnician.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareTechnicianId).Name;
            assigned.ProgrammerEmail = _db.SoftwareTechnician.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareTechnicianId).TechEmail;
            assigned.SoftwareTechnicianName = _db.SoftwareTechnician.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareTechnicianId).Name;
            assigned.SoftwareTechnicianId = programmerReportDto.SoftwareTechnicianId;
            assigned.SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId;
            _db.ProgrammerReport.Add(assigned);

            _db.SoftwareTaskLists.Add(new SoftwareTaskList()
            {
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                SoftwareTaskId = 3,
                SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId,
                ProgrammerReportId = assign.Id,
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + assign.Id), programmerReportDto);
        }
        [HttpGet]
        [Route("api/v2/Assigned/getbyId/{id}")]
        public IHttpActionResult GetAssigned(int id)
        {
            var assigned = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).Include(x => x.SoftwareAcceptsRequest).SingleOrDefault(x => x.Id == id);
            if (assigned == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<ProgrammerReport, ProgrammerReportDto>(assigned));
        }

        [HttpPut]
        [Route("api/software/v3/developer/{id}")]
        public IHttpActionResult Developer(int id, ProgrammerReportDto programmerReportDto)
        {
            var progReported = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).SingleOrDefault(h => h.Id == id);
            programmerReportDto.Id = progReported.Id;
            progReported.SoftwareId = programmerReportDto.SoftwareId;
            progReported.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareId).Name;
            progReported.InformationSystemId = programmerReportDto.InformationSystemId;
            progReported.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == programmerReportDto.InformationSystemId).Name;
            progReported.RequestFor = programmerReportDto.RequestFor;
            progReported.DateAdded = programmerReportDto.DateAdded;
            progReported.DateEnded = programmerReportDto.DateEnded;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edit Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/v3/assigned/{id}")]
        public IHttpActionResult Reported(int id, ProgrammerReportDto programmerReportDto)
        {
            var progReported = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).SingleOrDefault(h => h.Id == id);
            programmerReportDto.Id = progReported.Id;
            progReported.SoftwareId = programmerReportDto.SoftwareId;
            progReported.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareId).Name;
            progReported.InformationSystemId = programmerReportDto.InformationSystemId;
            progReported.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == programmerReportDto.InformationSystemId).Name;
            progReported.RequestFor = programmerReportDto.RequestFor;
            progReported.Description = programmerReportDto.Description;
            progReported.DocumentLabel = programmerReportDto.DocumentLabel;
            progReported.Resulotion = programmerReportDto.Resulotion;
            progReported.Remarks = programmerReportDto.Remarks;
            progReported.ProgressStatus = "100";
            progReported.DateEnded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            _db.SoftwareTaskLists.Add(new SoftwareTaskList()
            {
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                SoftwareTaskId = 4,
                SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId,
                ProgrammerReportId = progReported.Id,
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Reported Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            Db.Drafts.Add(new Draft
            {
                Sendto = programmerReportDto.MobileNumber,
                msg = "Your Service Request is Resolved" + " " + programmerReportDto.Remarks + " " + "This message is system-generated. No need to reply. Thank you. From: Office of the City Management Information System",
                tag = 0
            });
            Db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/manual/assigned/{id}")]
        public IHttpActionResult ReportedManual(int id, ProgrammerReportDto programmerReportDto)
        {
            var progReported = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).SingleOrDefault(h => h.Id == id);
            programmerReportDto.Id = progReported.Id;
            progReported.SoftwareId = programmerReportDto.SoftwareId;
            progReported.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareId).Name;
            progReported.InformationSystemId = programmerReportDto.InformationSystemId;
            progReported.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == programmerReportDto.InformationSystemId).Name;
            progReported.RequestFor = programmerReportDto.RequestFor;
            progReported.Description = programmerReportDto.Description;
            progReported.DocumentLabel = programmerReportDto.DocumentLabel;
            progReported.Resulotion = programmerReportDto.Resulotion;
            progReported.Remarks = programmerReportDto.Remarks;
            progReported.ProgressStatus = "100";
            progReported.DateEnded = programmerReportDto.DateEnded;
            progReported.DateAdded = programmerReportDto.DateAdded;


            _db.SoftwareTaskLists.Add(new SoftwareTaskList()
            {
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                SoftwareTaskId = 4,
                SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId,
                ProgrammerReportId = progReported.Id,
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Reported Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            _db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Route("api/software/Reported/{id}")]
        public IHttpActionResult SoftwareReported(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {
            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);
            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.Status = "Resolved-Manual";
            _db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/prog/saveFile")]
        public async Task<string> SaveUpload3()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/SoftwareImage/");


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);

            var provider = new MultipartFormDataStreamProvider(root);

            //var ftpAddress = "172.16.100.249";
            //var ftpUserName = "cgpp-SR";
            //var ftpPassword = "T]ZXm84q";

            //var ftpAddress = "192.168.1.2";
            //var ftpUserName = "shielamaemalaque2022@outlook.com";
            //var ftpPassword = "Malaque@22+08";
            //var ftpAddress = "192.168.71.148";
            //var ftpAddress = "172.16.1.225";
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
                            if (key == "programmerReportId")
                            {
                                var name = file.Headers
                                    .ContentDisposition
                                    .FileName;

                                // remove double quotes from string.
                                var codeNumber = Guid.NewGuid().ToString();
                                name = name.Trim('"');

                                var localFileName = file.LocalFileName;
                                var filePath = Path.Combine(root, codeNumber + name);
                                var folderName = _db.Departments.SingleOrDefault(x => x.Id == depId).Ftp.FolderName;
                                ftpClient.UploadFile(localFileName, folderName + codeNumber + name);

                                File.Move(localFileName, filePath);

                                ProgrammerUploads progUps = new ProgrammerUploads();
                                progUps.FileName = codeNumber + name;
                                progUps.ImagePath = folderName;
                                progUps.FileExtension = Path.GetExtension(name);
                                progUps.ProgrammerReportId = Convert.ToInt32(val);
                                progUps.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                progUps.RemarksUploads = provider.FormData["remarksUploads"];
                                progUps.FtpId = _db.Departments.SingleOrDefault(x => x.Id == depId).FtpId;
                                _db.ProgrammerUploads.Add(progUps);
                                _db.LoginActivity.Add(new LoginActivity()
                                {
                                    UserName = User.Identity.GetFullName(),
                                    ActivityMessage = "Added Image in Reported Software Request",
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

        [HttpPost]
        [Route("api/prog/saveFile2")]
        public async Task<string> SaveUpload4()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/SoftwareImage/");


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);

            var provider = new MultipartFormDataStreamProvider(root);

            //var ftpAddress = "172.16.100.249";
            //var ftpUserName = "cgpp-SR";
            //var ftpPassword = "T]ZXm84q";

            //var ftpAddress = "192.168.1.2";
            //var ftpUserName = "shielamaemalaque2022@outlook.com";
            //var ftpPassword = "Malaque@22+08";

            //var ftpAddress = "192.168.71.148";
            //var ftpAddress = "172.16.1.225";
            var ftpAddress = "192.168.1.2";
            var ftpUserName = "shielamaemalaque2022@outlook.com";
            var ftpPassword = "Malaque@22+08";
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
                            if (key == "programmerReportId")
                            {
                                var name = file.Headers
                                    .ContentDisposition
                                    .FileName;

                                // remove double quotes from string.
                                name = name.Trim('"');
                                var dateNew = Convert.ToString(DateTime.Now.Ticks) + "-";

                                var localFileName = file.LocalFileName;
                                var filePath = Path.Combine(root, dateNew + name);

                                ftpClient.UploadFile(localFileName, "/softwareimage/" + dateNew + name);

                                File.Move(localFileName, filePath);

                                ProgrammerUploads progUps = new ProgrammerUploads();
                                progUps.FileName = name;
                                progUps.ImagePath = "/softwareimage/" + dateNew + name;
                                progUps.ProgrammerReportId = Convert.ToInt32(val);
                                progUps.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                progUps.RemarksUploads = provider.FormData["remarksUploads"];
                                _db.ProgrammerUploads.Add(progUps);
                                _db.LoginActivity.Add(new LoginActivity()
                                {
                                    UserName = User.Identity.GetFullName(),
                                    ActivityMessage = "Added Progress in New System Request",
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
        [Route("api/progUps/get/{id}")]
        public IHttpActionResult GettechUploads(int id)
        {
            var uploadsoftware = _db.ProgrammerUploads.Include(x => x.ProgrammerReport).ToList().Select(Mapper.Map<ProgrammerUploads, ProgrammerUploadsDto>);
            uploadsoftware = new List<ProgrammerUploadsDto>(uploadsoftware.Where(x => x.ProgrammerReportId == id));
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            var ftpAddress = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpHost;
            var ftpUserName = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpUsername;
            var ftpPassword = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpPassword;
            var softwareblob = new List<ProgrammerUploadsDto>();
            using (var ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword))
            {
                ftpClient.Port = 21;
                ftpClient.Connect();

                foreach (var s in uploadsoftware)
                {
                    byte[] file;
                    ftpClient.DownloadBytes(out file, s.ImagePath + s.FileName);
                    s.DocumentBlob = file;
                    softwareblob.Add(s);
                }
                ftpClient.Disconnect();
            }
            return Ok(uploadsoftware);
        }
        //delete image
        [Route("api/v2/sf/deleteImage/{id}")]
        public IHttpActionResult DeleteImage5(int id)
        {
            var file = _db.ProgrammerUploads.SingleOrDefault(x => x.Id == id);
            if (file == null)
            {
                return NotFound();
            }
            _db.ProgrammerUploads.Remove(file);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Deleted Reported Attached Image",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }


        [HttpPut]
        [Route("api/newSystem/Reported/{id}")]
        public IHttpActionResult NewRequestSystem(int id, ProgrammerReportDto programmerReportDto)
        {
            var programmernewsystem = _db.ProgrammerReport.SingleOrDefault(h => h.Id == id);
            programmerReportDto.Id = programmernewsystem.Id;
            programmernewsystem.ProgressStatus = programmerReportDto.ProgressStatus;
            programmernewsystem.ProgressRemarks = programmerReportDto.ProgressRemarks;
            _db.SaveChanges();
            return Ok();
        }

        //Manuak
        [HttpPost]
        [Route("api/manual/saveRequest")]
        public async Task<string> SaveRequest()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/SoftwareImage/");


            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);

            var provider = new MultipartFormDataStreamProvider(root);
            var email = User.Identity.GetUserEmail();
            var progId = _db.SoftwareTechnician.SingleOrDefault(x => x.TechEmail == email).Id;
            int programmerId = Convert.ToInt32(progId);

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());


            //172.16.1.225
            var ftpAddress = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpHost;
            var ftpUserName = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpUsername;
            var ftpPassword = _db.Departments.Include(x => x.Ftp).SingleOrDefault(x => x.Id == depId).Ftp.FtpPassword;

            FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
            ftpClient.Port = 21;
            ftpClient.Connect();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                SoftwareUserRequest softwareRequest = new SoftwareUserRequest();

                if (softwareRequest.Id == 0)
                {
                    softwareRequest.FirstName = provider.FormData["firstName"];
                    softwareRequest.MiddleName = provider.FormData["middleName"];
                    softwareRequest.LastName = provider.FormData["lastName"];
                    softwareRequest.FullName = softwareRequest.FirstName + " " + softwareRequest.MiddleName + " " + softwareRequest.LastName;
                    softwareRequest.Email = provider.FormData["email"];
                    softwareRequest.MobileNumber = provider.FormData["mobileNumber"];
                    softwareRequest.DepartmentsId = Convert.ToInt32(provider.FormData["departmentsId"]);
                    softwareRequest.DepartmentName = _db.Departments.SingleOrDefault(x => x.Id == softwareRequest.DepartmentsId).Name;
                    softwareRequest.DivisionsId = Convert.ToInt32(provider.FormData["divisionsId"]);
                    softwareRequest.DivisionName = _db.Divisions.SingleOrDefault(x => x.Id == softwareRequest.DivisionsId).Name;
                    softwareRequest.SoftwareId = Convert.ToInt32(provider.FormData["softwareId"]);
                    softwareRequest.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == softwareRequest.SoftwareId).Name;
                    softwareRequest.InformationSystemId = Convert.ToInt32(provider.FormData["informationSystemId"]);
                    softwareRequest.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == softwareRequest.InformationSystemId).Name;
                    softwareRequest.RequestFor = provider.FormData["requestFor"];
                    softwareRequest.DocumentLabel = provider.FormData["FileName"];
                    softwareRequest.Description = provider.FormData["description"];
                    softwareRequest.IsNew = false;
                    softwareRequest.Status = "Manual";
                    softwareRequest.IsManual = true;
                    softwareRequest.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    softwareRequest.DateCreated = DateTime.Now;
                    softwareRequest.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;
                    _db.SoftwareUserRequests.Add(softwareRequest);

                    _db.ProgrammerReport.Add(new ProgrammerReport()
                    {
                        DateAssigned = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        DateCreated = DateTime.Now,
                        ProgrammerName = User.Identity.GetFullName(),
                        ProgrammerEmail = User.Identity.GetUserEmail(),
                        SoftwareTechnicianId = _db.SoftwareTechnician.SingleOrDefault(x => x.TechEmail == email).Id,
                        SoftwareTechnicianName = User.Identity.GetFullName(),
                        SoftwareUserRequestId = softwareRequest.Id,
                    });

                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a Manual Software Request",
                        ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetLogEmail(),
                        DepartmentName = User.Identity.GetDepartmentName(),
                        DivisionName = User.Identity.GetDivisionName(),
                    });
                    _db.SoftwareTaskLists.Add(new SoftwareTaskList()
                    {
                        SoftwareTaskId = 1,
                        DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        SoftwareUserRequestId = softwareRequest.Id,
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



                                SoftwareUserUploads softwareUploads = new SoftwareUserUploads();
                                softwareUploads.FileName = codeNumber + name;
                                softwareUploads.ImagePath = folderName;
                                softwareUploads.SoftwareUserRequestId = softwareRequest.Id;
                                softwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                softwareUploads.FileExtension = Path.GetExtension(name);
                                softwareUploads.FtpId = _db.Departments.SingleOrDefault(x => x.Id == depId).FtpId;



                                _db.SoftwareUserUploads.Add(softwareUploads);
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
        [Route("api/software/acceptsReturn/{id}")]
        public IHttpActionResult SOftwareUserReturn(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);


            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.Status = "Pending Division Approval";
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Returned Software Request to Division Personnel",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
           
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/V2/byUser/getbyId/{id}")]
        public IHttpActionResult GetReportByUser(int id)
        {
            var task = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).ToList().Select(Mapper.Map<ProgrammerReport, ProgrammerReportDto>);
            task = new List<ProgrammerReportDto>(task.Where(x => x.SoftwareUserRequestId == id));

            return Ok(task);
        }

        [HttpPut]
        [Route("api/edit/sf/manual/progReport/{id}")]
        public IHttpActionResult TechUpdate(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var userInfo = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);
            softwareUserRequestDto.Id = userInfo.Id;
            userInfo.FullName = softwareUserRequestDto.FullName;
            userInfo.Email = softwareUserRequestDto.Email;
            userInfo.MobileNumber = softwareUserRequestDto.MobileNumber;
            userInfo.DepartmentsId = softwareUserRequestDto.DepartmentsId;
            userInfo.DepartmentName = _db.Departments.SingleOrDefault(x => x.Id == softwareUserRequestDto.DepartmentsId).Name;
            userInfo.DivisionsId = softwareUserRequestDto.DivisionsId;
            userInfo.DivisionName = _db.Divisions.SingleOrDefault(x => x.Id == softwareUserRequestDto.DivisionsId).Name;
            userInfo.SoftwareId = softwareUserRequestDto.SoftwareId;
            userInfo.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == softwareUserRequestDto.SoftwareId).Name;
            userInfo.InformationSystemId = softwareUserRequestDto.InformationSystemId;
            userInfo.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == softwareUserRequestDto.InformationSystemId).Name;
            userInfo.RequestFor = softwareUserRequestDto.RequestFor;
            userInfo.DocumentLabel = softwareUserRequestDto.DocumentLabel;
            userInfo.Description = softwareUserRequestDto.Description;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edit a Manual Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/acceptsAssigned/{id}")]
        public IHttpActionResult SoftwareAcceptsAssigned(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);

            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.Status = "Accepts Request";
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

        [HttpPut]
        [Route("api/software/AssignedAccepted/{id}")]
        public IHttpActionResult SoftwareAccepted(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);

            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.Status = "In Progress";
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Route("api/software/Verified/{id}")]
        public IHttpActionResult SoftwareVerified(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);

            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.Status = "Verified";
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Route("api/software/AssignedAccepted2/{id}")]
        public IHttpActionResult SoftwareAccepted(int id, ProgrammerReportDto programmerReportDto)
        {

            var programmerDto = _db.ProgrammerReport.SingleOrDefault(h => h.Id == id);

            programmerReportDto.Id = programmerDto.Id;
            programmerDto.DateAssigned = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [Route("api/v2/ProgAssigned/getbyId/{id}")]
        public IHttpActionResult GetProgAssigned(int id)
        {
            var progAssigned = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).Include(x => x.SoftwareAcceptsRequest).SingleOrDefault(x => x.Id == id);
            if (progAssigned == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<ProgrammerReport, ProgrammerReportDto>(progAssigned));
        }


        [HttpPut]
        [Route("api/software/AssignedProgress/{id}")]
        public IHttpActionResult ProgressList(int id, ProgrammerReportDto programmerReportDto)
        {

            var programmerDto = _db.ProgrammerReport.SingleOrDefault(h => h.Id == id);

            programmerReportDto.Id = programmerDto.Id;
            programmerDto.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            programmerDto.DateCreated = DateTime.Now;
            programmerDto.ProgrammerName = _db.SoftwareTechnician.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareTechnicianId).Name;
            programmerDto.ProgrammerEmail = _db.SoftwareTechnician.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareTechnicianId).TechEmail;
            programmerDto.SoftwareTechnicianName = _db.SoftwareTechnician.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareTechnicianId).Name;
            programmerDto.SoftwareTechnicianId = programmerReportDto.SoftwareTechnicianId;
            programmerDto.SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/v5/Verifieds/{id}")]
        public IHttpActionResult Resolved(int id, ProgrammerReportDto programmerReportDto)
        {
            var userName = User.Identity.GetFullName();
            var programmerDto = _db.ProgrammerReport.SingleOrDefault(h => h.Id == id);

            programmerReportDto.Id = programmerDto.Id;
            programmerDto.AdminName = User.Identity.GetFullName();
            programmerDto.DateVerified = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            programmerDto.SuperAdminName = _db.SuperAdmins.SingleOrDefault(x => x.Id == 1).Name;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }



        //Programmer
        [HttpPost]
        [Route("api/verified/v2/save")]
        public IHttpActionResult Verified2(SoftwareVerificationDto softwareVerificationDto)
        {
            var verify = Mapper.Map<SoftwareVerificationDto, SoftwareVerification>(softwareVerificationDto);
            SoftwareVerification verification = new SoftwareVerification();
            softwareVerificationDto.Id = verify.Id;
            verify.DateVerified = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            verify.NameVerified = User.Identity.GetFullName();
            verify.EmailVerified = User.Identity.GetUserEmail();
            verify.SoftwareUserRequestId = softwareVerificationDto.SoftwareUserRequestId;
            verify.ProgrammerReportId = softwareVerificationDto.ProgrammerReportId;

            _db.SoftwareVerification.Add(verify);
            _db.SoftwareTaskLists.Add(new SoftwareTaskList()
            {
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                SoftwareTaskId = 5,
                SoftwareUserRequestId = softwareVerificationDto.SoftwareUserRequestId,
                ProgrammerReportId = softwareVerificationDto.ProgrammerReportId,
                SoftwareVerificationId = verify.Id
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + verification.Id), softwareVerificationDto);
        }



        //[HttpPost]
        //[Route("api/verified/checkbox/save")]
        //public IHttpActionResult SelectedVerify(SoftwareVerificationCheckboxDto softwareVerificationCheckboxDto)
        //{
        //    var verify = Mapper.Map<SoftwareVerificationCheckboxDto, SoftwareVerification>(softwareVerificationCheckboxDto);

        //    if (softwareVerificationCheckboxDto.Id == 0 )
        //    {
        //        foreach (var srv in softwareVerificationCheckboxDto.ProgrammerReportId)
        //        {
        //            var newVerified = new SoftwareVerification
        //            {
        //                DateVerified = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
        //                NameVerified = User.Identity.GetFullName(),
        //                EmailVerified = User.Identity.GetUserEmail(),
        //                SoftwareUserRequestId = 3,
        //                ProgrammerReportId = srv
        //            };
        //            _db.SoftwareVerification.Add(newVerified);
        //            _db.SaveChanges();
        //        }
        //    }

        //    return Ok(verify);
        //}


        //Programmer
        [HttpPost]
        [Route("api/verified/v2/save2")]
        public IHttpActionResult Verified3(SoftwareVerificationDto softwareVerificationDto)
        {
            var verify = Mapper.Map<SoftwareVerificationDto, SoftwareVerification>(softwareVerificationDto);
            SoftwareVerification verification = new SoftwareVerification();
            softwareVerificationDto.Id = verify.Id;
            verify.DateVerified = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            verify.NameVerified = User.Identity.GetFullName();
            verify.EmailVerified = User.Identity.GetUserEmail();
            verify.SoftwareUserRequestId = softwareVerificationDto.SoftwareUserRequestId;
            verify.ProgrammerReportId = softwareVerificationDto.ProgrammerReportId;

            _db.SoftwareVerification.Add(verify);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Reported a Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + verification.Id), softwareVerificationDto);
        }

        [HttpGet]
        [Route("api/v2/Verified/getbyId/{id}")]
        public IHttpActionResult GetVerified(int id)
        {
            var verified = _db.SoftwareVerification.Include(x => x.SoftwareUserRequest).Include(x => x.ProgrammerReport).SingleOrDefault(x => x.Id == id);
            if (verified == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<SoftwareVerification, SoftwareVerificationDto>(verified));
        }

        [HttpPost]
        [Route("api/approved/v2/save")]
        public IHttpActionResult Approved2(SoftwareApprovedDto softwareApprovedDto)
        {
            var Approved = Mapper.Map<SoftwareApprovedDto, SoftwareApproved>(softwareApprovedDto);
            SoftwareApproved approve = new SoftwareApproved();

            softwareApprovedDto.Id = Approved.Id;
            approve.NameApproval = User.Identity.GetFullName();
            approve.EmailApproval = User.Identity.GetUserEmail();
            approve.RemarksApproval = softwareApprovedDto.RemarksApproval;
            approve.DateApprove = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            approve.SoftwareUserRequestId = softwareApprovedDto.SoftwareUserRequestId;
            approve.ProgrammerReportId = softwareApprovedDto.ProgrammerReportId;
            approve.SoftwareVerificationId = softwareApprovedDto.SoftwareVerificationId;
            _db.SoftwareApproveds.Add(approve);

            _db.SoftwareTaskLists.Add(new SoftwareTaskList()
            {
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                SoftwareTaskId = 6,
                SoftwareUserRequestId = softwareApprovedDto.SoftwareUserRequestId,
                ProgrammerReportId = softwareApprovedDto.ProgrammerReportId,
                SoftwareVerificationId = softwareApprovedDto.SoftwareVerificationId,
                SoftwareApprovedId = Approved.Id,
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Approved a Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + Approved.Id), softwareApprovedDto);

        }
        [HttpPut]
        [Route("api/software/v5/Approved/{id}")]
        public IHttpActionResult Approved(int id, ProgrammerReportDto programmerReportDto)
        {
            var userName = User.Identity.GetFullName();
            var programmerDto = _db.ProgrammerReport.SingleOrDefault(h => h.Id == id);

            programmerReportDto.Id = programmerDto.Id;
            programmerDto.DateApproved = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            programmerDto.RemarksApproval = programmerReportDto.RemarksApproval;
            _db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Route("api/manualsoftware/Admin/assigned/{id}")]
        public IHttpActionResult ManualAdmin(int id, ProgrammerReportDto programmerReportDto)
        {
            var progReported = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).SingleOrDefault(h => h.Id == id);
            programmerReportDto.Id = progReported.Id;
            progReported.SoftwareId = programmerReportDto.SoftwareId;
            progReported.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareId).Name;
            progReported.InformationSystemId = programmerReportDto.InformationSystemId;
            progReported.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == programmerReportDto.InformationSystemId).Name;
            progReported.RequestFor = programmerReportDto.RequestFor;
            progReported.Description = programmerReportDto.Description;
            progReported.DocumentLabel = programmerReportDto.DocumentLabel;
            progReported.Resulotion = programmerReportDto.Resulotion;
            progReported.Remarks = programmerReportDto.Remarks;
            progReported.DateAdded = programmerReportDto.DateAdded;
            progReported.DateEnded = programmerReportDto.DateEnded;
            progReported.DateVerified = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            progReported.AdminName = User.Identity.GetFullName();
            progReported.SuperAdminName = _db.SuperAdmins.SingleOrDefault(x => x.Id == 1).Name;
            progReported.SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId;

            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/Admin/assigned/{id}")]
        public IHttpActionResult ReportAdmin(int id, ProgrammerReportDto programmerReportDto)
        {
            var progReported = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).SingleOrDefault(h => h.Id == id);
            programmerReportDto.Id = progReported.Id;
            progReported.SoftwareId = programmerReportDto.SoftwareId;
            progReported.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareId).Name;
            progReported.InformationSystemId = programmerReportDto.InformationSystemId;
            progReported.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == programmerReportDto.InformationSystemId).Name;
            progReported.RequestFor = programmerReportDto.RequestFor;
            progReported.Description = programmerReportDto.Description;
            progReported.DocumentLabel = programmerReportDto.DocumentLabel;
            progReported.Resulotion = programmerReportDto.Resulotion;
            progReported.Remarks = programmerReportDto.Remarks;
            //progReported.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            progReported.DateEnded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            progReported.DateVerified = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            progReported.AdminName = User.Identity.GetFullName();
            progReported.SuperAdminName = _db.SuperAdmins.SingleOrDefault(x => x.Id == 1).Name;
            progReported.SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId;
            _db.SoftwareTaskLists.Add(new SoftwareTaskList()
            {
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                SoftwareTaskId = 5,
                SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId,
                ProgrammerReportId = progReported.Id,
            });

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            Db.Drafts.Add(new Draft
            {
                Sendto = programmerReportDto.MobileNumber,
                msg = "Your Service Request is Resolved" + " " + programmerReportDto.Remarks,
                tag = 0
            });
            Db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Route("api/software/Approved/{id}")]
        public IHttpActionResult SoftwareApproved(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);

            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.Status = "Approved";
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/return/NewSystem/{id}")]
        public IHttpActionResult ReturnNewSytem(int id, ProgrammerReportDto programmerReportDto)
        {
            var userName = User.Identity.GetFullName();
            var programmerDto = _db.ProgrammerReport.SingleOrDefault(h => h.Id == id);

            programmerReportDto.Id = programmerDto.Id;
            programmerDto.ProgressStatus = "80";

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Route("api/software/Reported/Verify/{id}")]
        public IHttpActionResult SoftwareReported4(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {
            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);
            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.Status = "Verified";
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Verified a Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });

            _db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/v2/Approved/getbyId/{id}")]
        public IHttpActionResult GetApproved2(int id)
        {
            var approve = _db.SoftwareApproveds.Include(x => x.SoftwareUserRequest).Include(x => x.ProgrammerReport).SingleOrDefault(x => x.Id == id);
            if (approve == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<SoftwareApproved, SoftwareApprovedDto>(approve));
        }

        [HttpPut]
        [Route("api/software/v3/Approved/{id}")]
        public IHttpActionResult ReportedAdmin(int id, ProgrammerReportDto programmerReportDto)
        {
            var progReported = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).SingleOrDefault(h => h.Id == id);
            programmerReportDto.Id = progReported.Id;
            progReported.SoftwareId = programmerReportDto.SoftwareId;
            progReported.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == programmerReportDto.SoftwareId).Name;
            progReported.InformationSystemId = programmerReportDto.InformationSystemId;
            progReported.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == programmerReportDto.InformationSystemId).Name;
            progReported.RequestFor = programmerReportDto.RequestFor;
            progReported.Description = programmerReportDto.Description;
            progReported.DocumentLabel = programmerReportDto.DocumentLabel;
            progReported.Resulotion = programmerReportDto.Resulotion;
            progReported.Remarks = programmerReportDto.Remarks;
            progReported.DateEnded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            progReported.AdminName = User.Identity.GetFullName();
            progReported.SuperAdminName = User.Identity.GetFullName();
            progReported.DateVerified = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            progReported.DateApproved = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            progReported.SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId;

            _db.SoftwareVerification.Add(new SoftwareVerification()
            {
                DateVerified = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                NameVerified = User.Identity.GetFullName(),
                EmailVerified = User.Identity.GetUserEmail(),
                SoftwareUserRequestId = (int)progReported.SoftwareUserRequestId,
                ProgrammerReportId = progReported.Id,

            });

            _db.SoftwareApproveds.Add(new SoftwareApproved()
            {
                DateApprove = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                NameApproval = User.Identity.GetFullName(),
                EmailApproval = User.Identity.GetUserEmail(),
                SoftwareUserRequestId = (int)progReported.SoftwareUserRequestId,
                ProgrammerReportId = progReported.Id,

            });
            _db.SoftwareTaskLists.Add(new SoftwareTaskList()
            {
                DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                SoftwareTaskId = 6,
                SoftwareUserRequestId = programmerReportDto.SoftwareUserRequestId,
                ProgrammerReportId = progReported.Id,
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Reported a Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            Db.Drafts.Add(new Draft
            {
                Sendto = programmerReportDto.MobileNumber,
                msg = "Your Service Request is Resolved" + " " + programmerReportDto.Remarks,
                tag = 0
            });
            Db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/Reported/Approver/{id}")]
        public IHttpActionResult SoftwareReported5(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {
            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);
            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.Status = "Approved";
            _db.SaveChanges();
            return Ok();
        }

        //Programmer
        [HttpPost]
        [Route("api/softwareAccept/Task/save")]
        public IHttpActionResult AccepstsTask(SoftwareTaskListDto softwareTaskListDto)
        {
            var taskList = Mapper.Map<SoftwareTaskListDto, SoftwareTaskList>(softwareTaskListDto);

            SoftwareTaskList task = new SoftwareTaskList();

            softwareTaskListDto.Id = taskList.Id;
            task.SoftwareTaskId = 6;
            task.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            task.SoftwareUserRequestId = softwareTaskListDto.SoftwareUserRequestId;
            task.SoftwareAcceptsRequestId = softwareTaskListDto.SoftwareAcceptsRequestId;
            task.ProgrammerReportId = softwareTaskListDto.ProgrammerReportId;
            _db.SoftwareTaskLists.Add(taskList);
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a new Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),

            });
            _db.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + task.Id), softwareTaskListDto);
        }


        [HttpGet]
        [Route("api/Task/sf/get/{id}")]
        public IHttpActionResult GetRequestList(int id)
        {
            var task = _db.SoftwareTaskLists.Include(x => x.ProgrammerReport).Include(x => x.SoftwareAcceptsRequest).Include(x => x.SoftwareUserRequest).Include(x => x.SoftwareVerification).Include(x => x.SoftwareApproved).SingleOrDefault(x => x.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<SoftwareTaskList, SoftwareTaskListDto>(task));
        }


        [HttpPut]
        [Route("api/software/Return/Approver/{id}")]
        public IHttpActionResult SoftwareReturnApproved(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {
            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);
            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.Status = "Resolved";
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Returned Verified Request To Admin",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/phoneNumber/get")]
        public IHttpActionResult GetphoneBook()
        {
            var pnumber = _db.SoftwareTechnician.ToList().Select(Mapper.Map<SoftwareTechnician, softwarePhoneNumberDto>);
            return Ok(pnumber);
        }

        //POST with File Attachment
        //Post
        [HttpPost]
        [Route("api/Software/div/SaveRequest")]
        public async Task<string> SaveUploaddiv()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/SoftwareImage/");
            var devNumber = "09165778160";
            var SAdminNumber = "09178450776";
            var AdminNumber = "09158814555";
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

                SoftwareUserRequest softwareRequest = new SoftwareUserRequest();

                if (softwareRequest.Id == 0)
                {
                    softwareRequest.DocumentLabel = provider.FormData["FileName"];
                    softwareRequest.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    softwareRequest.FullName = User.Identity.GetFirstName() + " " + User.Identity.GetMiddleName() + " " + User.Identity.GetLastName();
                    softwareRequest.FirstName = User.Identity.GetFirstName();
                    softwareRequest.LastName = User.Identity.GetLastName();
                    softwareRequest.MiddleName = User.Identity.GetMiddleName();
                    softwareRequest.Email = User.Identity.GetUserEmail();
                    softwareRequest.MobileNumber = User.Identity.GetUserMobileNumber();
                    softwareRequest.DepartmentsId = depId;
                    softwareRequest.DepartmentName = _db.Departments.SingleOrDefault(x => x.Id == depId).Name;
                    softwareRequest.DateCreated = DateTime.Now;
                    softwareRequest.DivisionsId = divId;
                    softwareRequest.DivisionName = _db.Divisions.SingleOrDefault(x => x.Id == divId).Name;
                    softwareRequest.Description = provider.FormData["description"];
                    softwareRequest.InformationSystemId = Convert.ToInt32(provider.FormData["informationSystemId"]);
                    softwareRequest.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == softwareRequest.InformationSystemId).Name;
                    softwareRequest.RequestFor = provider.FormData["requestFor"];
                    softwareRequest.SoftwareId = Convert.ToInt32(provider.FormData["softwareId"]);
                    softwareRequest.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == softwareRequest.SoftwareId).Name;
                    softwareRequest.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;
                    softwareRequest.IsNew = false;
                    softwareRequest.Status = "Open";
                    _db.SoftwareUserRequests.Add(softwareRequest);

                    _db.SoftwareAcceptsRequests.Add(new SoftwareAcceptsRequest()
                    {
                        DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        SoftwareUserRequestId = softwareRequest.Id,
                        FullName = User.Identity.GetFullName(),
                        Email = User.Identity.GetUserEmail(),
                        DepartmentsId = Convert.ToInt32(User.Identity.GetUserDepartment()),
                        DepartmentName = User.Identity.GetDepartmentName(),
                        DivisionsId = Convert.ToInt32(User.Identity.GetUserDivision()),
                        DivisionName = User.Identity.GetDivisionName(),
                        IsAccept = "seen",
                    });
                    _db.SoftwareTaskLists.Add(new SoftwareTaskList()
                    {
                        SoftwareTaskId = 1,
                        DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        SoftwareUserRequestId = softwareRequest.Id,
                    });
                    _db.RequestHistory.Add(new RequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        UploadMessage = "Added a new Software Request",
                        UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetLogEmail(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });
                    _db.RequestHistory.Add(new RequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        Email = User.Identity.GetLogEmail(),
                        DivisionsId = divId,
                        DepartmentsId = depId,
                        Category = false,
                        UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        UploadMessage = "Added a new Software Request",
                        SoftwareUserRequestId = softwareRequest.Id,
                    });
                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a new Software Request",
                        ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetLogEmail(),
                        DepartmentName = User.Identity.GetDepartmentName(),
                        DivisionName = User.Identity.GetDivisionName(),
                    });
                    Db.Drafts.Add(new Draft
                    {
                        Sendto = devNumber,
                        msg = "New Software Request" + " " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + " " + softwareRequest.SoftwareName + " " + "Department :" + " " + softwareRequest.DepartmentName,
                        tag = 0
                    });

                    Db.Drafts.Add(new Draft
                    {
                        Sendto = SAdminNumber,
                        msg = "New Software Request" + " " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + " " + softwareRequest.SoftwareName + " " + "Department :" + " " + softwareRequest.DepartmentName,
                        tag = 0
                    });
                    Db.Drafts.Add(new Draft
                    {
                        Sendto = AdminNumber,
                        msg = "New Software Request" + " " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + " " + softwareRequest.SoftwareName + " " + "Department :" + " " + softwareRequest.DepartmentName,
                        tag = 0
                    });
                    Db.SaveChanges();
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

                                SoftwareUserUploads softwareUploads = new SoftwareUserUploads();
                                softwareUploads.FileName = codeNumber + name;
                                softwareUploads.ImagePath = folderName;
                                softwareUploads.SoftwareUserRequestId = softwareRequest.Id;
                                softwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                softwareUploads.FileExtension = Path.GetExtension(name);
                                softwareUploads.FtpId = _db.Departments.SingleOrDefault(x => x.Id == depId).FtpId;

                                _db.SoftwareUserUploads.Add(softwareUploads);
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
            //ftpClient.Disconnect();
            return "File uploaded!";

        }
        [Route("api/sf/divReport")]
        [HttpGet]
        public IHttpActionResult RequestDivByMonth()
        {
            var divName = User.Identity.GetDivisionName();
            var divresult = _db.SoftwareUserRequests.Where(u => u.DivisionName == divName);

            var reports = divresult.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.FullName, r.SoftwareName })
                  .Where(grp => grp.Count() > 0)
                  .Select(g => new { g.Key.Year, g.Key.Month, g.Key.FullName, g.Key.SoftwareName, Count = g.Count() })
                  .ToList();
            //Count Report
            return Ok(reports);
        }



        [HttpGet]
        [Route("api/software/div/requestbyUserList")]
        public IHttpActionResult getsoftwareListDiv()
        {
            var srDto = _db.SoftwareUserRequests
                .Include(x => x.Software)
                .ToList();
            var divName = User.Identity.GetDivisionName();
            srDto = new List<SoftwareUserRequest>(srDto.Where(x => x.DivisionName == divName));
            //srDto.OrderByDescending(u => u.Id)
            return Ok(srDto.OrderByDescending(x => x.Id).Take(10));
        }


        [HttpPut]
        [Route("api/v3/sms/techReport/{id}")]
        public IHttpActionResult SendSms(int id, ProgrammerReportDto programmerReportDto)
        {

            var prog = _db.ProgrammerReport.SingleOrDefault(h => h.Id == id);


            programmerReportDto.Id = prog.Id;
            prog.SmsMessage = programmerReportDto.SmsMessage;
            prog.DateSend = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Send a Message"  ,
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
            });

            Db.Drafts.Add(new Draft
            {
                Sendto = programmerReportDto.MobileNumber,
                msg = prog.SmsMessage + "This message is system-generated. No need to reply. Thank you. From: Office of the City Management Information System",
                tag = 0
            });
            Db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/returned/{id}")]
        public IHttpActionResult UpdateReturn(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {
            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());

            var softwarinDb = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);

            softwarinDb.Id = id;
            softwareUserRequestDto.Id = id;
            softwarinDb.Status = "Return Request";
            softwarinDb.IsNew = false;
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Return Request to User",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),


            });

            Db.Drafts.Add(new Draft
            {
                Sendto = softwarinDb.MobileNumber,
                msg = "Your Request Return"+ " " + "This message is system-generated. No need to reply. Thank you. From: Office of the City Management Information System",
                tag = 0
            });
            Db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software/Return/Request/{id}")]
        public IHttpActionResult UpdateFromReturn(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {
            var softwareRequest = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);
            var devNumber = "09165778160";
            var SAdminNumber = "09178450776";
            var AdminNumber = "09158814555";

            softwareUserRequestDto.Id = softwareRequest.Id;
            softwareRequest.SoftwareId = softwareUserRequestDto.SoftwareId;
            softwareRequest.SoftwareName = _db.Software.SingleOrDefault(x => x.Id == softwareUserRequestDto.SoftwareId).Name;
            softwareRequest.InformationSystemId = softwareUserRequestDto.InformationSystemId;
            softwareRequest.InformationName = _db.InformationSystem.SingleOrDefault(x => x.Id == softwareUserRequestDto.InformationSystemId).Name;
            softwareRequest.RequestFor = softwareUserRequestDto.RequestFor;
            softwareRequest.DocumentLabel = softwareUserRequestDto.DocumentLabel;
            softwareRequest.Description = softwareUserRequestDto.Description;


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            var divApprover = _db.Divisions.SingleOrDefault(x => x.Id == divId).IsDivisionApprover;
            var deptApprover = _db.Departments.SingleOrDefault(x => x.Id == depId).IsDepartmentApprover;

            if (deptApprover == true)
            {
                softwareRequest.Status = "Pending Department Approval";

                var roles = _db.Roles.Where(r => r.Name == "DepartmentApprover");

                var approvers = new List<ApplicationUser>();

                if (roles.Any())
                {
                    var roleId = roles.First().Id;
                    approvers = _db.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == roleId) && u.DepartmentsId == softwareRequest.DepartmentsId).ToList();


                }

                foreach (var approver in approvers)
                {

                    Db.Drafts.Add(new Draft
                    {
                        Sendto = approver.MobileNumber,
                        msg = " New Hardware Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Department : " + " " + softwareRequest.DepartmentName,
                        tag = 0
                    });
                }



                Db.SaveChanges();
            }
            else if (divApprover == true)
            {
                softwareRequest.Status = "Pending Division Approval";

                var roles = _db.Roles.Where(r => r.Name == "DivisionApprover");
                var approvers = new List<ApplicationUser>();
                if (roles.Any())
                {
                    var rolesId = roles.First().Id;
                    approvers = _db.Users.Include(x => x.Roles).Where(u => u.Roles.Any(r => r.RoleId == rolesId) && u.DivisionsId == softwareRequest.DivisionsId).ToList();
                }

                foreach (var divA in approvers)
                {
                    Db.Drafts.Add(new Draft
                    {
                        Sendto = divA.MobileNumber,
                        msg = "ok",
                        tag = 0
                    });
                }
                Db.SaveChanges();
            }

            else
            {
                softwareRequest.Status = "Open";

                Db.Drafts.Add(new Draft
                {
                    Sendto = SAdminNumber,
                    msg = " New Hardware Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Department : " + " " + softwareRequest.DepartmentName,
                    tag = 0
                });
                Db.Drafts.Add(new Draft
                {
                    Sendto = devNumber,
                    msg = " New Hardware Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Department : " + " " + softwareRequest.DepartmentName,
                    tag = 0
                });
                Db.Drafts.Add(new Draft
                {
                    Sendto = AdminNumber,
                    msg = " New Hardware Request" + "  " + "Reported By :" + softwareRequest.FullName + " " + "Issue :" + softwareRequest.SoftwareName + " " + "Department : " + " " + softwareRequest.DepartmentName,
                    tag = 0
                });
                Db.SaveChanges();
            }


            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Edit A Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),
            });
            _db.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [Route("api/sf/mytech/count")]
        public IHttpActionResult MonthlytechList()
        {
            int dateNow = Convert.ToInt32(DateTime.Now.Year.ToString());
            var techName = User.Identity.GetFullName();
            var result = _db.ProgrammerReport.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.SoftwareTechnician.Name })
                            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                            .Where(x => x.Year == dateNow && x.Name == techName)
                            .OrderBy(x => x.Year).ThenBy(x => x.Month)
                            .ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("api/sf/mytech/countyear")]
        public IHttpActionResult MonthlytechListYear()
        {
            var techName = User.Identity.GetFullName();
            var result = _db.ProgrammerReport.GroupBy(r => new { r.DateCreated.Year, r.SoftwareTechnician.Name })
                            .Select(g => new { g.Key.Year, g.Key.Name, Count = g.Count() })
                            .Where(x => x.Name == techName)
                            .OrderBy(x => x.Year)
                            .ToList();

            return Ok(result);
        }


        [HttpGet]
        [Route("api/v2/sftProg/count")]
        public IHttpActionResult TechCountList()
        {
            var result = _db.ProgrammerReport.GroupBy(r => new { r.SoftwareTechnician.Name })
                            .Where(grp => grp.Count() > 0)
                            .Select(g => new { g.Key.Name, Count = g.Count() })
                            .ToList();
            return Ok(result);
        }


        [HttpGet]
        [Route("api/v2/allsft/count")]
        public IHttpActionResult AlltechList()
        {
            int dateNow = Convert.ToInt32(DateTime.Now.Year.ToString());
            var result = _db.ProgrammerReport.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.SoftwareTechnician.Name })
                            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                            .Where(x => x.Year == dateNow)
                            .OrderBy(x => x.Year).ThenBy(x => x.Month)
                            .ToList();

            return Ok(result);
        }
        [HttpGet]
        [Route("api/v2/AllYearlysf/count")]
        public IHttpActionResult YearlyReport()
        {
            var result = _db.ProgrammerReport.GroupBy(r => new { r.DateCreated.Year })
                            .Where(grp => grp.Count() > 0)
                            .Select(g => new { g.Key.Year, Count = g.Count() })
                            .ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("api/v2/dept/sf/count")]
        public IHttpActionResult DepartmentCountList()
        {
            var deptName = User.Identity.GetDepartmentName();

            var hr = _db.SoftwareUserRequests.Where(x => x.DepartmentName == deptName);
            var reports = hr.GroupBy(x => new { x.DateCreated.Year, x.DateCreated.Month, x.FullName, x.SoftwareName })
                .Where(grp => grp.Count() > 0)
                            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.SoftwareName, g.Key.FullName, Count = g.Count() })
                            .ToList();
            return Ok(reports);
        }


        [HttpPut]
        [Route("api/v2/sms/sfApprover/{id}")]
        public IHttpActionResult SendSmsAdminsf(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var SoftwareSms = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);


            softwareUserRequestDto.Id = SoftwareSms.Id;

            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Send a Message",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetLogEmail(),
            });

            Db.Drafts.Add(new Draft
            {
                Sendto = softwareUserRequestDto.MobileNumber,
                msg = softwareUserRequestDto.SmsMessage+ " " + "This message is system-generated. No need to reply. Thank you. From: Office of the City Management Information System",
                tag = 0
            });
            Db.SaveChanges();
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/software4/CancelRequest/{id}")]
        public IHttpActionResult CancelRequestnew(int id, SoftwareUserRequestDto softwareUserRequestDto)
        {

            var softwareInDb = _db.SoftwareUserRequests.SingleOrDefault(h => h.Id == id);

            softwareInDb.Id = id;
            softwareUserRequestDto.Id = id;
            softwareInDb.Status = "Cancel";
            softwareInDb.IsNew = false;


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
    }

}
