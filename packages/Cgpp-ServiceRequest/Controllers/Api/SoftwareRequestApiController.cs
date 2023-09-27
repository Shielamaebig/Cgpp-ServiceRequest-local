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
    public class SoftwareRequestApiController : ApiController
    {
        private ApplicationDbContext _db;


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
        // working
        [HttpGet]
        [Route("api/sr/assign")]
        public IHttpActionResult GetProgrammers()
        {
            var progDto = _db.SoftwareRequest.Include(x => x.Software).ToList().Select(Mapper.Map<SoftwareRequest, SoftwareRequestSpecDto>);
            if (User.IsInRole("Programmers"))
            {
                var progEmail = User.Identity.GetUserEmail();
                progDto = new List<SoftwareRequestSpecDto>(progDto.Where(x => x.ProEmail == progEmail));
            }
            return Ok(progDto.OrderByDescending(x => x.Id));
        }

        [HttpGet]
        [Route("api/v2/sr/assign")]
        public IHttpActionResult GetDashProgrammers()
        {
            var progDto = _db.SoftwareRequest
                .Include(x => x.Software)
                .Include(x => x.Departments)
                .Include(x => x.Divisions)
                .ToList().Select(Mapper.Map<SoftwareRequest, SoftwareRequestSpecDto>);
            if (User.IsInRole("Programmers"))
            {
                var progEmail = User.Identity.GetUserEmail();
                progDto = new List<SoftwareRequestSpecDto>(progDto.Where(x => x.ProEmail == progEmail));
            }
            return Ok(progDto.OrderByDescending(x => x.Id).Take(5));
        }


        [Route("api/v2/sr/get")]
        public IHttpActionResult GetUser()
        {
            var srDto = _db.SoftwareRequest
                .Include(x => x.Software)
                .ToList().Select(Mapper.Map<SoftwareRequest, SoftwareRequestSpecDto>);
            if (User.IsInRole("Users"))
            {
                var userEmail = User.Identity.GetUserEmail();
                srDto = new List<SoftwareRequestSpecDto>(srDto.Where(x => x.Email == userEmail));
            }

            return Ok(srDto.OrderByDescending(u => u.Id));
        }


        [HttpGet]
        [Route("api/info/System")]
        public IHttpActionResult GetInformationSystem()
        {
            var information = _db.InformationSystem.ToList();
            return Ok(information);
        }

        [HttpGet]
        [Route("api/softwareRequest/requestTech")]
        public IHttpActionResult GetRequestTech()
        {
            var sf = _db.SoftwareRequest
                .Include(x => x.Software)
                .Include(x => x.SoftwareTechnician)
                .ToList().Select(Mapper.Map<SoftwareRequest, SoftwareRequestSpecDto>);

            return Ok(sf.OrderByDescending(s => s.Id));
        }

        [HttpGet]
        [Route("api/softwareRequest/getSoftware")]
        public IHttpActionResult GetDashSoftware()
        {
            var sf = _db.SoftwareRequest
                .Include(x => x.Software)
                .Include(x => x.SoftwareTechnician)
                .Include(x => x.Departments)
                .Include(x => x.Status)
                .Include(x => x.Divisions)
                .ToList().Select(Mapper.Map<SoftwareRequest, SoftwareRequestSpecDto>);
            return Ok(sf.OrderByDescending(s => s.Ticket).Take(5));
        }
        [HttpGet]
        [Route("api/softwareRequest/List")]
        public IHttpActionResult GetSoftwareRequest()
        {
            var sf = _db.SoftwareRequest.ToList().Select(Mapper.Map<SoftwareRequest, SoftwareRequestSpecDto>);
            return Ok(sf);
        }

        //POST with File Attachment
        //Post
        [HttpPost]
        [Route("api/Software/SaveRequest")]
        public async Task<string> SaveUpload()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/SoftwareImage/");


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            Random rand = new Random();
            int codeNum = rand.Next(00000, 99999);

            var provider = new MultipartFormDataStreamProvider(root);

            var ftpAddress = "172.16.100.249";
            var ftpUserName = "cgpp-SR";
            var ftpPassword = "T]ZXm84q";
            //var ftpAddress = "192.168.1.171";
            //var ftpUserName = "Shielamaebig";
            //var ftpPassword = "";

            FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
            ftpClient.Port = 21;
            ftpClient.Connect();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                SoftwareRequest softwareRequest = new SoftwareRequest();

                if (softwareRequest.Id == 0)
                {
                    softwareRequest.DocumentLabel = provider.FormData["FileName"];
                    softwareRequest.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    softwareRequest.FullName = User.Identity.GetFullName();
                    softwareRequest.Email = User.Identity.GetUserEmail();
                    softwareRequest.MobileNumber = User.Identity.GetUserMobileNumber();
                    softwareRequest.DepartmentsId = depId;
                    softwareRequest.DateCreated = DateTime.Now;
                    softwareRequest.DivisionsId = divId;
                    softwareRequest.Description = provider.FormData["description"];
                    softwareRequest.InformationSystemId = Convert.ToInt32(provider.FormData["informationSystemId"]);
                    softwareRequest.RequestFor = provider.FormData["requestFor"];
                    softwareRequest.SoftwareId = Convert.ToInt32(provider.FormData["softwareId"]);
                    softwareRequest.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;
                    softwareRequest.StatusId = 1;
                    softwareRequest.IsNew = true;
                    _db.SoftwareRequest.Add(softwareRequest);
                    _db.RequestHistory.Add(new RequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        UploadMessage = "Added a new Software Request",
                        UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.SoftwareRequestHistory.Add(new SoftwareRequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        RequetMessage = "Added a new Software Request",
                        RequetDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });
                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a new Software Request",
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



                                SoftwareUploads softwareUploads = new SoftwareUploads();
                                softwareUploads.FileName = name;
                                softwareUploads.ImagePath = "/CGPP-SR/" + dateNew + name;
                                softwareUploads.SoftwareRequestId = softwareRequest.Id;
                                softwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");


                                _db.SoftwareUploads.Add(softwareUploads);
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
        [Route("api/softwareRequest/GetbyId/{id}")]
        public IHttpActionResult GetRequestbyId(int id)
        {
            var softwareList = _db.SoftwareRequest.SingleOrDefault(s => s.Id == id);
            if (softwareList == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<SoftwareRequest, SoftwareRequestDto>(softwareList));
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
        //Status
        [Route("api/softwareRequest/Status")]
        [HttpGet]
        public IHttpActionResult GetStatus()
        {
            var status = _db.Status.ToList();
            return Ok(status);
        }
        //Technician
        [Route("api/softwareRequest/Technician")]
        [HttpGet]

        public IHttpActionResult GetTechnician()
        {
            var softTech = _db.SoftwareTechnician.ToList();
            return Ok(softTech);
        }

        //updateRequest User
        [HttpPut]
        [Route("api/softwareRequest/updateRequest/{id}")]
        public IHttpActionResult UpdateRequestUsers(int id, SoftwareRequestDto softwareRequestDto)
        {
            var softwareRequestInDb = _db.SoftwareRequest.SingleOrDefault(s => s.Id == id);

            softwareRequestInDb.Id = id;
            softwareRequestDto.Id = id;
            softwareRequestInDb.SoftwareId = softwareRequestDto.SoftwareId;
            softwareRequestInDb.InformationSystemId = softwareRequestDto.InformationSystemId;
            softwareRequestInDb.RequestFor = softwareRequestDto.RequestFor;
            softwareRequestInDb.Description = softwareRequestDto.Description;
            softwareRequestInDb.DocumentLabel = softwareRequestDto.DocumentLabel;

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.RequestHistory.Add(new RequestHistory()
            {
                UserName = User.Identity.GetFullName(),
                UploadMessage = "Updated A Software Request",
                UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentsId = depId,
                DivisionsId = divId,
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Updated A Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/softwareRequest/updateStatus/{id}")]
        public IHttpActionResult UpdateStatus(int id, SoftwareRequestDto softwareRequestDto)
        {
            var softwareRequestInDb = _db.SoftwareRequest.SingleOrDefault(s => s.Id == id);
            softwareRequestDto.Id = softwareRequestInDb.Id;
            softwareRequestInDb.StatusId = softwareRequestDto.StatusId;
            softwareRequestInDb.IsNew = false;

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Update a Software Status",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/softwareRequest/Cancel/{id}")]
        public IHttpActionResult UpdateCancel(int id, SoftwareRequestDto softwareRequestDto)
        {
            var softwareRequestInDb = _db.SoftwareRequest.SingleOrDefault(s => s.Id == id);

            softwareRequestDto.Id = softwareRequestInDb.Id;
            softwareRequestInDb.StatusId = 5;
            softwareRequestInDb.SoftwareTechnicianId = null;
            softwareRequestInDb.IsNew = false;

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.RequestHistory.Add(new RequestHistory()
            {
                UserName = User.Identity.GetFullName(),
                UploadMessage = "Cancelled A Request",
                UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentsId = depId,
                DivisionsId = divId,
            });
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Cancelled A Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/softwareRequest/UpdateTechnician/{id}")]
        public IHttpActionResult UpdateTech(int id, SoftwareRequestDto softwareRequestDto)
        {
            var softwareRequestInDb = _db.SoftwareRequest.SingleOrDefault(s => s.Id == id);

            var techEmail = _db.SoftwareTechnician.SingleOrDefault(d => d.Id == softwareRequestDto.SoftwareTechnicianId).TechEmail;

            softwareRequestDto.Id = softwareRequestInDb.Id;
            softwareRequestInDb.IsAssigned = true;
            softwareRequestInDb.SoftwareTechnicianId = softwareRequestDto.SoftwareTechnicianId;
            softwareRequestInDb.ProEmail = techEmail;
            softwareRequestInDb.IsNew = false;

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Assigned a Software Technician",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/softwareRequest/Assigntomer/{id}")]
        public IHttpActionResult AssignTomer(int id, SoftwareRequestDto softwareRequestDto)
        {
            //var techEmail = _db.SoftwareTechnician.SingleOrDefault(d => d.Id == softwareRequestDto.SoftwareTechnicianId).Id;
            var email = User.Identity.GetUserEmail();
            var techId = _db.SoftwareTechnician.SingleOrDefault(x => x.TechEmail == email).Id;


            var softwareRequestInDb = _db.SoftwareRequest.SingleOrDefault(s => s.Id == id);
            softwareRequestDto.Id = softwareRequestInDb.Id;
            softwareRequestInDb.SoftwareTechnicianId = techId;
            softwareRequestInDb.StatusId = 4;
            softwareRequestInDb.ProEmail = User.Identity.GetUserEmail();
            softwareRequestInDb.IsNew = false;

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Get Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }


        [HttpPut]
        [Route("api/softwareRequest/TechnicianForm/{id}")]
        public IHttpActionResult UpdateTechnicianReport(int id, SoftwareRequestDto softwareRequestDto)
        {
            var softwareRequestInDb = _db.SoftwareRequest.SingleOrDefault(s => s.Id == id);
            softwareRequestInDb.Id = id;
            softwareRequestDto.Id = id;
            softwareRequestInDb.DateStarted = softwareRequestDto.DateStarted;
            softwareRequestInDb.TimeStarted = softwareRequestDto.TimeStarted;
            softwareRequestInDb.DateEnded = softwareRequestDto.DateEnded;
            softwareRequestInDb.TimeEnded = softwareRequestDto.TimeEnded;
            softwareRequestInDb.Remarks = softwareRequestDto.Remarks;
            softwareRequestInDb.SoftwareId = softwareRequestDto.SoftwareId;
            softwareRequestInDb.IsReported = true;

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added a Software Analysis Report",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();

        }

        [HttpPut]
        [Route("api/softwareRequest/verify/{id}")]
        public IHttpActionResult UpdateVerify(int id, SoftwareRequestDto softwareRequestDto)
        {
            var softwareRequestInDb = _db.SoftwareRequest.SingleOrDefault(s => s.Id == id);

            softwareRequestInDb.Id = id;
            softwareRequestDto.Id = id;
            softwareRequestInDb.Viewer = User.Identity.GetFullName();
            softwareRequestInDb.IsViewed = true;
            softwareRequestInDb.DateView = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Verified  A Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("api/softwareRequest/Viewed/{id}")]
        public IHttpActionResult UpdateViewed(int id, SoftwareRequestDto softwareRequestDto)
        {
            var softwareRequestInDb = _db.SoftwareRequest.SingleOrDefault(s => s.Id == id);

            softwareRequestDto.Id = id;
            softwareRequestInDb.Id = id;
            softwareRequestInDb.Approver = User.Identity.GetFullName();
            softwareRequestInDb.IsApproved = true;
            softwareRequestInDb.DateApproved = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");

            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = " Approved a Software Request",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),

            });
            _db.SaveChanges();

            return Ok();
        }


        [HttpGet]
        [Route("api/software/countReq")]
        public IHttpActionResult GetCountsoftware()
        {
            var softwareReqDto = _db.SoftwareRequest.ToList();
            return Ok(softwareReqDto.Count);
        }
        [HttpGet]
        [Route("api/software/requestbyUser")]
        public IHttpActionResult getsoftwareCount()
        {
            var srDto = _db.SoftwareRequest
                .ToList();
            if (User.IsInRole("Users"))
            {
                var rEmail = User.Identity.GetUserEmail();
                srDto = new List<SoftwareRequest>(srDto.Where(x => x.Email == rEmail));
            }
            return Ok(srDto.Count);
        }

        [HttpGet]
        [Route("api/software/requestbyUserList")]
        public IHttpActionResult getsoftwareList()
        {
            var srDto = _db.SoftwareRequest
                .Include(x => x.Software)
                .ToList();
            if (User.IsInRole("Users"))
            {
                var rEmail = User.Identity.GetUserEmail();
                srDto = new List<SoftwareRequest>(srDto.Where(x => x.Email == rEmail));
            }
            //srDto.OrderByDescending(u => u.Id)
            return Ok(srDto.OrderByDescending(x => x.Id).Take(10));
        }

        [HttpDelete]
        [Route("api/Software/Delete/{id}")]
        public IHttpActionResult DeleteHardwareTech(int id)
        {
            var softwareReq = _db.SoftwareRequest.SingleOrDefault(h => h.Id == id);
            if (softwareReq == null)
            {
                return NotFound();
            }

            _db.SoftwareRequest.Remove(softwareReq);

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

        [HttpGet]
        [Route("api/sf/bymonth")]
        public IHttpActionResult RequestbyMonth()
        {
            var result = _db.SoftwareRequest.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.SoftwareTechnician.Name })
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

            var result = _db.SoftwareRequest.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.Software.Name })
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


            var result = _db.SoftwareRequest.GroupBy(r => new { r.DateCreated.Year, r.DateCreated.Month, r.InformationSystem.Name })
                .Where(grp => grp.Count() > 0)
                .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Name, Count = g.Count() })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("api/sftStatus/count")]
        public IHttpActionResult StastusCount()
        {
            var byName = from s in _db.SoftwareRequest
                         group s by s.Status.Name into g
                         select new { Name = g.Key, Count = g.Count() };
            return Ok(byName);
        }

        [HttpGet]
        [Route("api/v1/uploadDisplay")]
        public IHttpActionResult UploadDisplayList()
        {
            var uploadDisplay = _db.SoftwareUploads.Include(x => x.SoftwareRequest).ToList().Select(Mapper.Map<SoftwareUploads, SoftwareUploadsDto>);

            return Ok(uploadDisplay.OrderByDescending(u => u.Id));
        }
        [HttpGet]
        [Route("api/v1/uploads/{id}")]
        public IHttpActionResult Uploads(int id)
        {
            var uploadDisplay = _db.SoftwareUploads.Include(x => x.SoftwareRequest).ToList().Select(Mapper.Map<SoftwareUploads, SoftwareUploadsDto>);
            uploadDisplay = new List<SoftwareUploadsDto>(uploadDisplay.Where(x => x.SoftwareRequestId == id));
            var ftpAddress = "172.16.100.249";
            var ftpUserName = "cgpp-SR";
            var ftpPassword = "T]ZXm84q";


            var softwareblob = new List<SoftwareUploadsDto>();
            using (var ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword))
            {
                ftpClient.Port = 21;
                ftpClient.Connect();

                foreach (var s in uploadDisplay)
                {
                    byte[] file;
                    ftpClient.DownloadBytes(out file, s.ImagePath);
                    s.DocumentBlob = file;
                    softwareblob.Add(s);
                }
                ftpClient.Disconnect();
            }
            return Ok(uploadDisplay);
        }

        [Route("api/v1/deteleImage/{id}")]
        public IHttpActionResult DeleteImage(int id)
        {
            var file = _db.SoftwareUploads.SingleOrDefault(x => x.Id == id);
            if (file == null)
            {
                return NotFound();
            }
            _db.SoftwareUploads.Remove(file);
            _db.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("api/v2/update/saveFile")]
        public async Task<string> SaveUpload2()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/SoftwareImage/");


            int depId = Convert.ToInt32(User.Identity.GetUserDepartment());
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());



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
                            if (key == "softwareRequestId")
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

                                SoftwareUploads softwareUploads = new SoftwareUploads();
                                softwareUploads.FileName = name;
                                softwareUploads.ImagePath = "/CGPP-SR/" + dateNew + name;
                                softwareUploads.SoftwareRequestId = Convert.ToInt32(val);
                                softwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                                _db.SoftwareUploads.Add(softwareUploads);

                                _db.RequestHistory.Add(new RequestHistory()
                                {
                                    UserName = User.Identity.GetFullName(),
                                    UploadMessage = "Added Image in Software Request",
                                    UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                                    Email = User.Identity.GetUserName(),
                                    DepartmentsId = depId,
                                    DivisionsId = divId,
                                });

                                _db.SoftwareRequestHistory.Add(new SoftwareRequestHistory()
                                {
                                    UserName = User.Identity.GetFullName(),
                                    RequetMessage = "Added Image in Software Request",
                                    RequetDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                                    Email = User.Identity.GetUserName(),
                                    DepartmentsId = depId,
                                    DivisionsId = divId,
                                }); 
                                _db.LoginActivity.Add(new LoginActivity()
                                {
                                    UserName = User.Identity.GetFullName(),
                                    ActivityMessage = "Added Image in Software Request",
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

            var ftpAddress = "172.16.100.249";
            var ftpUserName = "cgpp-SR";
            var ftpPassword = "T]ZXm84q";


            FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
            ftpClient.Port = 21;
            ftpClient.Connect();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                SoftwareRequest softwareRequest = new SoftwareRequest();

                if (softwareRequest.Id == 0)
                {
                    softwareRequest.FullName = provider.FormData["fullName"];
                    softwareRequest.Email = provider.FormData["email"];
                    softwareRequest.MobileNumber = provider.FormData["mobileNumber"];
                    softwareRequest.DepartmentsId = Convert.ToInt32(provider.FormData["departmentsId"]);
                    softwareRequest.DivisionsId = Convert.ToInt32(provider.FormData["divisionsId"]);

                    softwareRequest.SoftwareId = Convert.ToInt32(provider.FormData["softwareId"]);
                    softwareRequest.InformationSystemId = Convert.ToInt32(provider.FormData["informationSystemId"]);
                    softwareRequest.RequestFor = provider.FormData["requestFor"];
                    softwareRequest.DocumentLabel = provider.FormData["FileName"];
                    softwareRequest.Description = provider.FormData["description"];




                    softwareRequest.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    softwareRequest.DateCreated = DateTime.Now;
                    softwareRequest.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;
                    softwareRequest.StatusId = 2;
                    softwareRequest.IsReported = true;
                    softwareRequest.SoftwareTechnicianId = programmerId;
                    softwareRequest.ProEmail = User.Identity.GetUserName();

                    softwareRequest.DateStarted = provider.FormData["dateStarted"];
                    softwareRequest.TimeStarted = provider.FormData["timeStarted"];
                    softwareRequest.DateEnded = provider.FormData["dateEnded"];
                    softwareRequest.TimeEnded = provider.FormData["timeEnded"];
                    softwareRequest.Remarks = provider.FormData["remarks"];

                    _db.SoftwareRequest.Add(softwareRequest);

                    _db.RequestHistory.Add(new RequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        UploadMessage = "Added a Manual Software Request",
                        UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.SoftwareRequestHistory.Add(new SoftwareRequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        RequetMessage = "Added a Manual Software Request",
                        RequetDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a Manual Software Request",
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



                                SoftwareUploads softwareUploads = new SoftwareUploads();
                                softwareUploads.FileName = name;
                                softwareUploads.ImagePath = "/CGPP-SR/" + dateNew + name;
                                softwareUploads.SoftwareRequestId = softwareRequest.Id;
                                softwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");


                                _db.SoftwareUploads.Add(softwareUploads);
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


        //SoftwareAdmin
        [HttpPost]
        [Route("api/manual/SaveRequestAdmin")]
        public async Task<string> SaveRequestAdmin()
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

            var ftpAddress = "172.16.100.249";
            var ftpUserName = "cgpp-SR";
            var ftpPassword = "T]ZXm84q";


            FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
            ftpClient.Port = 21;
            ftpClient.Connect();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                SoftwareRequest softwareRequest = new SoftwareRequest();

                if (softwareRequest.Id == 0)
                {
                    softwareRequest.FullName = provider.FormData["fullName"];
                    softwareRequest.Email = provider.FormData["email"];
                    softwareRequest.MobileNumber = provider.FormData["mobileNumber"];
                    softwareRequest.DepartmentsId = Convert.ToInt32(provider.FormData["departmentsId"]);
                    softwareRequest.DivisionsId = Convert.ToInt32(provider.FormData["divisionsId"]);

                    softwareRequest.SoftwareId = Convert.ToInt32(provider.FormData["softwareId"]);
                    softwareRequest.InformationSystemId = Convert.ToInt32(provider.FormData["informationSystemId"]);
                    softwareRequest.RequestFor = provider.FormData["requestFor"];
                    softwareRequest.DocumentLabel = provider.FormData["FileName"];
                    softwareRequest.Description = provider.FormData["description"];




                    softwareRequest.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    softwareRequest.DateCreated = DateTime.Now;
                    softwareRequest.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;
                    softwareRequest.StatusId = 2;
                    softwareRequest.IsReported = true;
                    softwareRequest.IsViewed = true;
                    softwareRequest.DateView = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    softwareRequest.Viewer = User.Identity.GetFullName();
                    softwareRequest.SoftwareTechnicianId = programmerId;
                    softwareRequest.ProEmail = User.Identity.GetUserName();

                    softwareRequest.DateStarted = provider.FormData["dateStarted"];
                    softwareRequest.TimeStarted = provider.FormData["timeStarted"];
                    softwareRequest.DateEnded = provider.FormData["dateEnded"];
                    softwareRequest.TimeEnded = provider.FormData["timeEnded"];
                    softwareRequest.Remarks = provider.FormData["remarks"];

                    _db.SoftwareRequest.Add(softwareRequest);

                    _db.RequestHistory.Add(new RequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        UploadMessage = "Added a Manual Software Request",
                        UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.SoftwareRequestHistory.Add(new SoftwareRequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        RequetMessage = "Added a Manual Software Request",
                        RequetDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a Manual Software Request",
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



                                SoftwareUploads softwareUploads = new SoftwareUploads();
                                softwareUploads.FileName = name;
                                softwareUploads.ImagePath = "/CGPP-SR/" + dateNew + name;
                                softwareUploads.SoftwareRequestId = softwareRequest.Id;
                                softwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");


                                _db.SoftwareUploads.Add(softwareUploads);
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

        //SuperAdmin
        [HttpPost]
        [Route("api/manual/SaveRequest2")]
        public async Task<string> SaveRequest2()
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

            var ftpAddress = "172.16.100.249";
            var ftpUserName = "cgpp-SR";
            var ftpPassword = "T]ZXm84q";


            FtpClient ftpClient = new FtpClient(ftpAddress, ftpUserName, ftpPassword);
            ftpClient.Port = 21;
            ftpClient.Connect();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                SoftwareRequest softwareRequest = new SoftwareRequest();

                if (softwareRequest.Id == 0)
                {
                    softwareRequest.FullName = provider.FormData["fullName"];
                    softwareRequest.Email = provider.FormData["email"];
                    softwareRequest.MobileNumber = provider.FormData["mobileNumber"];
                    softwareRequest.DepartmentsId = Convert.ToInt32(provider.FormData["departmentsId"]);
                    softwareRequest.DivisionsId = Convert.ToInt32(provider.FormData["divisionsId"]);

                    softwareRequest.SoftwareId = Convert.ToInt32(provider.FormData["softwareId"]);
                    softwareRequest.InformationSystemId = Convert.ToInt32(provider.FormData["informationSystemId"]);
                    softwareRequest.RequestFor = provider.FormData["requestFor"];
                    softwareRequest.DocumentLabel = provider.FormData["FileName"];
                    softwareRequest.Description = provider.FormData["description"];




                    softwareRequest.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    softwareRequest.DateCreated = DateTime.Now;
                    softwareRequest.Ticket = DateTime.Now.ToString("Mddyyyy") + codeNum;
                    softwareRequest.StatusId = 2;
                    softwareRequest.IsReported = true;
                    softwareRequest.IsApproved = true;
                    softwareRequest.IsViewed = true;
                    softwareRequest.DateApproved = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    softwareRequest.DateView = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");
                    softwareRequest.Viewer = "Luwe Songcayaon";
                    softwareRequest.Approver = User.Identity.GetFullName();
                    softwareRequest.SoftwareTechnicianId = programmerId;
                    softwareRequest.ProEmail = User.Identity.GetUserName();

                    softwareRequest.DateStarted = provider.FormData["dateStarted"];
                    softwareRequest.TimeStarted = provider.FormData["timeStarted"];
                    softwareRequest.DateEnded = provider.FormData["dateEnded"];
                    softwareRequest.TimeEnded = provider.FormData["timeEnded"];
                    softwareRequest.Remarks = provider.FormData["remarks"];

                    _db.SoftwareRequest.Add(softwareRequest);

                    _db.RequestHistory.Add(new RequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        UploadMessage = "Added a Manual Software Request",
                        UploadDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.SoftwareRequestHistory.Add(new SoftwareRequestHistory()
                    {
                        UserName = User.Identity.GetFullName(),
                        RequetMessage = "Added a Manual Software Request",
                        RequetDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                        Email = User.Identity.GetUserName(),
                        DepartmentsId = depId,
                        DivisionsId = divId,
                    });

                    _db.LoginActivity.Add(new LoginActivity()
                    {
                        UserName = User.Identity.GetFullName(),
                        ActivityMessage = "Added a Manual Software Request",
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



                                SoftwareUploads softwareUploads = new SoftwareUploads();
                                softwareUploads.FileName = name;
                                softwareUploads.ImagePath = "/CGPP-SR/" + dateNew + name;
                                softwareUploads.SoftwareRequestId = softwareRequest.Id;
                                softwareUploads.DateAdded = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt");


                                _db.SoftwareUploads.Add(softwareUploads);
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
