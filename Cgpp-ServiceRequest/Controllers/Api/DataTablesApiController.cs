using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;
using Cgpp_ServiceRequest.Models.Extensions;
using System.Linq.Dynamic;
using Cgpp_ServiceRequest.DataTables;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using System.Net;

namespace Cgpp_ServiceRequest.Controllers.Api
{
    public class DataTablesApiController : ApiController
    {
        private ApplicationDbContext _db;

        public DataTablesApiController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        [Route("api/all/Request")]
        [HttpGet]
        public DataTableResponse GetAllRequest(DataTableRequest request)
        {
            IQueryable<RequestHistory> requestHistoryDto = _db.RequestHistory.Include(x => x.Departments).Include(x => x.Divisions).OrderByDescending(x => x.Id);
            var newFilter = _db.RequestHistory.Count();

            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                requestHistoryDto = requestHistoryDto.Where(x => x.UserName.ToLower().Contains(searchText.ToLower()) || x.Email.ToLower().Contains(searchText.ToLower()) || x.Departments.Name.ToLower().Contains(searchText.ToLower()) || x.Departments.Id.ToString().Contains(searchText.ToLower()) || x.Divisions.Name.ToLower().Contains(searchText.ToLower()) || x.Divisions.Id.ToString().Contains(searchText.ToLower()) || x.UploadMessage.ToLower().Contains(searchText.ToLower()) || x.UploadDate.ToLower().Contains(searchText.ToLower()));
            }
            var pagedFiles = requestHistoryDto.Where(u => u.DepartmentsId == null || u.DepartmentsId != null);

            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.RequestHistory.Where(u => u.DepartmentsId == null || u.DepartmentsId != null).Count(),
                recordsFiltered = requestHistoryDto.Where(u => u.DepartmentsId == null || u.DepartmentsId != null).Count(),
                data = pagedFiles.Select(Mapper.Map<RequestHistory, RequestHistoryDto>).ToArray(),
                error = ""
            };

        }

        [Route("api/all/byRole")]
        [HttpGet]
        public DataTableResponse GetAllByRole(DataTableRequest request)
        {
            IQueryable<LoginActivity> activityDto = _db.LoginActivity.OrderByDescending(x => x.Id);
            var newFilter = _db.LoginActivity.Count();

            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                activityDto = activityDto.Where(x => x.UserName.ToLower().Contains(searchText.ToLower()) || x.Email.ToLower().Contains(searchText.ToLower()));
            }

            var usedName = User.Identity.GetUserEmail();
            var UsedEmail = User.Identity.GetLogEmail();
            var pagedFiles = activityDto.Where(u => u.Email == usedName || u.Email == UsedEmail).OrderByDescending(x => x.Id);

            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.LoginActivity.Where(u => u.Email == usedName || u.Email == UsedEmail).Count(),
                recordsFiltered = activityDto.Where(u => u.Email == usedName || u.Email == UsedEmail).Count(),
                data = pagedFiles.Select(Mapper.Map<LoginActivity, LoginActivityDto>).ToArray(),
                error = ""
            };

        }


        [Route("api/all/activity")]
        [HttpGet]
        public DataTableResponse GetAllactivity(DataTableRequest request)
        {
            IQueryable<LoginActivity> loginActivityDto = _db.LoginActivity.OrderByDescending(x => x.Id);
            var newFilter = _db.LoginActivity.Count();

            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                loginActivityDto = _db.LoginActivity.Where(x => x.UserName.ToLower().Contains(searchText.ToLower()) || x.Email.ToLower().Contains(searchText.ToLower()) || x.ActivityMessage.ToLower().Contains(searchText.ToLower()) || x.ActivityDate.ToLower().Contains(searchText.ToLower()));
            }

            var pagedFiles = loginActivityDto.Where(u => u.Email == null || u.Email != null);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }

            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.LoginActivity.Where(u => u.Email == null || u.Email != null).Count(),
                recordsFiltered = loginActivityDto.Where(u => u.Email == null || u.Email != null).Count(),
                data = pagedFiles.Select(Mapper.Map<LoginActivity, LoginActivityDto>).ToArray(),
                error = ""
            };
        }










        [Route("api/sr/user")]
        [HttpGet]
        public DataTableResponse GetSrUser(DataTableRequest request)
        {
            IQueryable<SoftwareUserRequest> softwareRequestDto = _db.SoftwareUserRequests.Include(x => x.Software).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareUserRequests.Count();
            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                softwareRequestDto = softwareRequestDto.Where(x => x.Ticket.ToLower().Contains(searchText.ToLower()) || x.Software.Id.ToString().Contains(searchText.ToLower()) || x.Software.Name.ToLower().Contains(searchText.ToLower()) || x.FullName.ToLower().Contains(searchText.ToLower()));
            }
            var userEmail = User.Identity.GetLogEmail();
            var usedEmail = User.Identity.GetUserEmail();

            var pagedFiles = softwareRequestDto.Where(u => u.Email == userEmail || u.Email == usedEmail);
            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareUserRequests.Where(u => u.Email == userEmail).Count(),
                recordsFiltered = softwareRequestDto.Where(u => u.Email == userEmail).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareUserRequest, SoftwareUserRequestDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/sr/all/user")]
        [HttpGet]
        public DataTableResponse GetSrAll(DataTableRequest request)
        {
            IQueryable<SoftwareUserRequest> softwareRequestDto = _db.SoftwareUserRequests.Include(x => x.Software).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareUserRequests.Count();
            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                softwareRequestDto = softwareRequestDto.Where(x => x.Ticket.ToLower().Contains(searchText.ToLower()) || x.Software.Id.ToString().Contains(searchText.ToLower()) || x.Software.Name.ToLower().Contains(searchText.ToLower()) || x.FullName.ToLower().Contains(searchText.ToLower()));
            }
            var pagedFiles = softwareRequestDto.Where(u => u.SoftwareName == null || u.SoftwareName != null);
            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareUserRequests.Where(u => u.SoftwareName == null || u.SoftwareName != null).Count(),
                recordsFiltered = softwareRequestDto.Where(u => u.SoftwareName == null || u.SoftwareName != null).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareUserRequest, SoftwareUserRequestDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/v2/hr/user")]
        [HttpGet]
        public DataTableResponse GetUserRequestList(DataTableRequest request)
        {
            IQueryable<HardwareUserRequest> hardwareRequests = _db.HardwareUserRequests.OrderByDescending(x => x.Id);

            var newFilter = _db.HardwareUserRequests.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                hardwareRequests = hardwareRequests.Where(x => x.Ticket.ToLower().Contains(searchTect.ToLower()) || x.Status.ToLower().Contains(searchTect.ToLower()) || x.HardwareName.ToLower().Contains(searchTect.ToLower()) || x.DateAdded.ToLower().Contains(searchTect.ToLower()));
            }

            var userEmail = User.Identity.GetLogEmail();
            var usedEmail = User.Identity.GetUserEmail();
            var pagedFiles = hardwareRequests.Where(u => u.Email == userEmail || u.Email == usedEmail);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.HardwareUserRequests.Where(u => u.Email == userEmail).Count(),
                recordsFiltered = hardwareRequests.Where(u => u.Email == userEmail).Count(),
                data = pagedFiles.Select(Mapper.Map<HardwareUserRequest, HardwareUserRequestDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/v2/hr/assigned2")]
        [HttpGet]
        public DataTableResponse GetAssignedHr(DataTableRequest request)
        {
            IQueryable<TechnicianReport> hardwareAssignedDto = _db.TechnicianReports.Include(x => x.HardwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.TechnicianReports.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                hardwareAssignedDto = hardwareAssignedDto.Where(x => x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.HardwareName.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var technicianemail = User.Identity.GetLogEmail();
            var pagedFiles = hardwareAssignedDto.Where(u => u.TechEmail == technicianemail);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.TechnicianReports.Where(u => u.TechEmail == technicianemail).Count(),
                recordsFiltered = hardwareAssignedDto.Where(u => u.TechEmail == technicianemail).Count(),
                data = pagedFiles.Select(Mapper.Map<TechnicianReport, TechnicianReportDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/v2/hr/assigned")]
        [HttpGet]
        public DataTableResponse GetAssignedHr2(DataTableRequest request)
        {
            IQueryable<TechnicianReport> hardwareAssignedDto = _db.TechnicianReports.Include(x => x.HardwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.TechnicianReports.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                hardwareAssignedDto = hardwareAssignedDto.Where(x => x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.HardwareName.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var technicianemail = User.Identity.GetLogEmail();
            var pagedFiles = hardwareAssignedDto.Where(u => u.TechEmail == technicianemail);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.TechnicianReports.Where(u => u.TechEmail == technicianemail).Count(),
                recordsFiltered = hardwareAssignedDto.Where(u => u.TechEmail == technicianemail).Count(),
                data = pagedFiles.Select(Mapper.Map<TechnicianReport, TechnicianReportDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/v2/hr/developer")]
        [HttpGet]
        public DataTableResponse GetAssignedDev(DataTableRequest request)
        {
            IQueryable<TechnicianReport> hardwareAssignedDto = _db.TechnicianReports.Include(x => x.HardwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.TechnicianReports.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                hardwareAssignedDto = hardwareAssignedDto.Where(x => x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.HardwareName.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var pagedFiles = hardwareAssignedDto.Where(u => u.HardwareName != null || u.HardwareName == null);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.TechnicianReports.Where(u => u.HardwareName != null || u.HardwareName == null).Count(),
                recordsFiltered = hardwareAssignedDto.Where(u => u.HardwareName != null || u.HardwareName == null).Count(),
                data = pagedFiles.Select(Mapper.Map<TechnicianReport, TechnicianReportDto>).ToArray(),
                error = ""
            };
        }



        [Route("api/v3/all/hardReq")]
        [HttpGet]
        public DataTableResponse GetAllHardware3(DataTableRequest request)
        {
            IQueryable<TechnicianReport> hardwareRequestDto = _db.TechnicianReports.Include(x => x.HardwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.TechnicianReports.Count();

            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                hardwareRequestDto = hardwareRequestDto.Where(x => x.HardwareUserRequest.Ticket.ToLower().Contains(searchText.ToLower()) || x.HardwareUserRequest.DateAdded.ToLower().Contains(searchText.ToLower()) || x.HardwareUserRequest.FullName.ToLower().Contains(searchText.ToLower()) || x.Hardware.Id.ToString().Contains(searchText.ToLower()) || x.Hardware.Name.ToLower().Contains(searchText.ToLower()));
            }

            var pagedFiles = hardwareRequestDto.Where(u => u.HardwareName == null || u.HardwareName != null);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.TechnicianReports.Where(u => u.HardwareName == null || u.HardwareName != null).Count(),
                recordsFiltered = hardwareRequestDto.Where(u => u.HardwareName == null || u.HardwareName != null).Count(),
                data = pagedFiles.Select(Mapper.Map<TechnicianReport, TechnicianReport>).ToArray(),
                error = ""
            };
        }

        [Route("api/v2/all/hardReq")]
        [HttpGet]
        public DataTableResponse GetAllHardware2(DataTableRequest request)
        {
            IQueryable<HardwareUserRequest> hardwareRequestDto = _db.HardwareUserRequests.Include(x => x.Hardware).OrderByDescending(x => x.DateAdded);

            var newFilter = _db.HardwareUserRequests.Count();

            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                hardwareRequestDto = hardwareRequestDto.Where(x => x.Ticket.ToLower().Contains(searchText.ToLower()) || x.DateAdded.ToLower().Contains(searchText.ToLower()) || x.FullName.ToLower().Contains(searchText.ToLower()) || x.Hardware.Id.ToString().Contains(searchText.ToLower()) || x.Hardware.Name.ToLower().Contains(searchText.ToLower()) || x.Status.ToLower().Contains(searchText.ToLower()));
            }

            var open = "Open";
            var pagedFiles = hardwareRequestDto.Where(u => u.Status == open);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.DateAdded);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.HardwareUserRequests.Where(u => u.Status == open).Count(),
                recordsFiltered = hardwareRequestDto.Where(u => u.Status == open).Count(),
                data = pagedFiles.Select(Mapper.Map<HardwareUserRequest, HardwareUserRequest>).ToArray(),
                error = ""
            };
        }
        [Route("api/v5/all/hardReq")]
        [HttpGet]
        public DataTableResponse GetHardwareRequest3(DataTableRequest request)
        {
            IQueryable<TechnicianReport> techniciandb = _db.TechnicianReports.Include(x => x.Hardware).Include(x => x.HardwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.HardwareUserRequests.Count();

            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                techniciandb = techniciandb.Where(x => x.HardwareUserRequest.Id.ToString().Contains(searchText.ToLower()) || x.HardwareUserRequest.Ticket.ToLower().Contains(searchText.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchText.ToLower()) || x.HardwareUserRequest.HardwareName.ToLower().Contains(searchText.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchText.ToLower()) || x.HardwareUserRequest.DateAdded.ToLower().Contains(searchText.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchText.ToLower()) || x.HardwareUserRequest.FullName.ToLower().Contains(searchText.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchText.ToLower()) || x.Status.ToLower().Contains(searchText.ToLower()));
            }

            var progress = "In Progress";
            var accepts = "Accepts Request";
            var manual = "Manual";
            var pagedFiles = techniciandb.Where(u => u.HardwareUserRequest.Status == progress || u.HardwareUserRequest.Status == accepts || u.HardwareUserRequest.Status == manual);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.TechnicianReports.Where(u => u.Status == progress).Count(),
                recordsFiltered = techniciandb.Where(u => u.Status == progress).Count(),
                data = pagedFiles.Select(Mapper.Map<TechnicianReport, TechnicianReport>).ToArray(),
                error = ""
            };
        }
        [Route("api/v4/all/hardReq")]
        [HttpGet]
        public DataTableResponse GetHardwareRequest2(DataTableRequest request)
        {
            IQueryable<HardwareUserRequest> hardwareRequestDto = _db.HardwareUserRequests.Include(x => x.Hardware).OrderByDescending(x => x.Id);

            var newFilter = _db.HardwareUserRequests.Count();

            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                hardwareRequestDto = hardwareRequestDto.Where(x => x.Ticket.ToLower().Contains(searchText.ToLower()) || x.Status.ToLower().Contains(searchText.ToLower()) || x.FullName.ToLower().Contains(searchText.ToLower()) || x.Hardware.Id.ToString().Contains(searchText.ToLower()) || x.Hardware.Name.ToLower().Contains(searchText.ToLower()));
            }


            var pagedFiles = hardwareRequestDto.Where(u => u.Status != null || u.Status == null);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.HardwareUserRequests.Where(u => u.Status != null || u.Status == null).Count(),
                recordsFiltered = hardwareRequestDto.Where(u => u.Status != null || u.Status == null).Count(),
                data = pagedFiles.Select(Mapper.Map<HardwareUserRequest, HardwareUserRequest>).ToArray(),
                error = ""
            };
        }

        [Route("api/v2/resolved")]
        [HttpGet]
        public DataTableResponse GetHardwareResolved(DataTableRequest request)
        {
            IQueryable<TechnicianReport> hardwareAssignedDto = _db.TechnicianReports.Include(x => x.HardwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.TechnicianReports.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                hardwareAssignedDto = hardwareAssignedDto.Where(x => x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.HardwareName.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var resolved = "Resolved";
            var pagedFiles = hardwareAssignedDto.Where(u => u.HardwareUserRequest.Status == resolved);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.TechnicianReports.Where(u => u.HardwareUserRequest.Status == resolved).Count(),
                recordsFiltered = hardwareAssignedDto.Where(u => u.HardwareUserRequest.Status == resolved).Count(),
                data = pagedFiles.Select(Mapper.Map<TechnicianReport, TechnicianReportDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/v2/verified")]
        [HttpGet]
        public DataTableResponse GetVerified(DataTableRequest request)
        {
            IQueryable<TechnicianReport> hardwareAssignedDto = _db.TechnicianReports.Include(x => x.HardwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.TechnicianReports.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                hardwareAssignedDto = hardwareAssignedDto.Where(x => x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.HardwareName.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var verified = "Verified";
            var pagedFiles = hardwareAssignedDto.Where(u => u.HardwareUserRequest.Status == verified);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.TechnicianReports.Where(u => u.HardwareUserRequest.Status == verified).Count(),
                recordsFiltered = hardwareAssignedDto.Where(u => u.HardwareUserRequest.Status == verified).Count(),
                data = pagedFiles.Select(Mapper.Map<TechnicianReport, TechnicianReportDto>).ToArray(),
                error = ""
            };
        }


        [Route("api/v3/verified")]
        [HttpGet]
        public DataTableResponse GetVerified2(DataTableRequest request)
        {
            IQueryable<HardwareVerify> verifiesDb = _db.HardwareVerifies.Include(x => x.HardwareUserRequest).Include(x=>x.TechnicianReport).OrderByDescending(x => x.Id);

            var newFilter = _db.HardwareVerifies.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                verifiesDb = verifiesDb.Where(x => x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.HardwareName.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var verified = "Verified";
            var pagedFiles = verifiesDb.Where(u => u.HardwareUserRequest.Status == verified );

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.HardwareVerifies.Where(u => u.HardwareUserRequest.Status == verified).Count(),
                recordsFiltered = verifiesDb.Where(u => u.HardwareUserRequest.Status == verified).Count(),
                data = pagedFiles.Select(Mapper.Map<HardwareVerify, HardwareVerifyDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/v3/verifiedDev")]
        [HttpGet]
        public DataTableResponse GetVerified5(DataTableRequest request)
        {
            IQueryable<HardwareVerify> verifiesDb = _db.HardwareVerifies.Include(x => x.HardwareUserRequest).Include(x => x.TechnicianReport).OrderByDescending(x => x.Id);

            var newFilter = _db.HardwareVerifies.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                verifiesDb = verifiesDb.Where(x => x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.HardwareName.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var pagedFiles = verifiesDb.Where(u => u.HardwareUserRequest.HardwareName != null || u.HardwareUserRequest.HardwareName == null);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.HardwareVerifies.Where(u => u.HardwareUserRequest.HardwareName != null || u.HardwareUserRequest.HardwareName == null).Count(),
                recordsFiltered = verifiesDb.Where(u => u.HardwareUserRequest.HardwareName != null || u.HardwareUserRequest.HardwareName == null).Count(),
                data = pagedFiles.Select(Mapper.Map<HardwareVerify, HardwareVerifyDto>).ToArray(),
                error = ""
            };
        }


        [Route("api/softwareRequest/Get")]
        [HttpGet]
        public DataTableResponse GetSoftware(DataTableRequest request)
        {
            IQueryable<SoftwareUserRequest> requestdb = _db.SoftwareUserRequests.Include(x => x.Software).Include(x => x.InformationSystem).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareUserRequests.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.FullName.ToLower().Contains(searchTect.ToLower()));
            }


            var accept = "Accept";
            var update = "Pending Division Approval";
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            var pagedFiles = requestdb.Where(u => u.Status == accept && u.DivisionsId == divId || u.Status == update);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareUserRequests.Where(u => u.Status == accept).Count(),
                recordsFiltered = requestdb.Where(u => u.Status == accept).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareUserRequest, SoftwareUserRequestDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/softwareRequestDev/Get")]
        [HttpGet]
        public DataTableResponse GetSoftwareDev(DataTableRequest request)
        {
            IQueryable<SoftwareUserRequest> requestdb = _db.SoftwareUserRequests.Include(x => x.Software).Include(x => x.InformationSystem).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareUserRequests.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.FullName.ToLower().Contains(searchTect.ToLower()));
            }


            var accept = "Accept";
            var update = "Update Request";
            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            var pagedFiles = requestdb.Where(u => u.Status == accept || u.Status == update);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareUserRequests.Where(u => u.Status == accept).Count(),
                recordsFiltered = requestdb.Where(u => u.Status == accept).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareUserRequest, SoftwareUserRequestDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/Approve/Get")]
        [HttpGet]
        public DataTableResponse GetApproved(DataTableRequest request)
        {
            IQueryable<HardwareApproval> approveDb = _db.HardwareApprovals.Include(x => x.HardwareUserRequest).Include(x => x.TechnicianReport).Include(x=>x.HardwareVerify).OrderByDescending(x => x.Id);

            var newFilter = _db.HardwareVerifies.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                approveDb = approveDb.Where(x => x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.HardwareName.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.HardwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.HardwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var approved = "Approved";
            var pagedFiles = approveDb.Where(u => u.HardwareUserRequest.Status == approved);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.HardwareApprovals.Where(u => u.HardwareUserRequest.Status == approved).Count(),
                recordsFiltered = approveDb.Where(u => u.HardwareUserRequest.Status == approved).Count(),
                data = pagedFiles.Select(Mapper.Map<HardwareApproval, HardwareApprovalDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/v4/softwareRequest/Get")]
        [HttpGet]
        public DataTableResponse GetSoftware2(DataTableRequest request)
        {
            IQueryable<SoftwareUserRequest> requestdb = _db.SoftwareUserRequests.Include(x => x.Software).Include(x => x.InformationSystem).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareUserRequests.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.FullName.ToLower().Contains(searchTect.ToLower()));
            }


            int divId = Convert.ToInt32(User.Identity.GetUserDivision());
            var pagedFiles = requestdb.Where(u => u.IsNew == false && u.DivisionsId == divId);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareUserRequests.Where(u => u.IsNew == false).Count(),
                recordsFiltered = requestdb.Where(u => u.IsNew == false).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareUserRequest, SoftwareUserRequestDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/SoftwareRequest/v2/Get")]
        [HttpGet]
        public DataTableResponse GetAccepted(DataTableRequest request)
        {
            IQueryable<SoftwareAcceptsRequest> requestdb = _db.SoftwareAcceptsRequests.Include(x => x.SoftwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareAcceptsRequests.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var open = "Open";
            var pagedFiles = requestdb.Where(u => u.SoftwareUserRequest.Status == open);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareAcceptsRequests.Where(u => u.SoftwareUserRequest.Status == open).Count(),
                recordsFiltered = requestdb.Where(u => u.SoftwareUserRequest.Status == open).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareAcceptsRequest, SoftwareAcceptsRequestDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/SoftwareAccepts/v2/Get")]
        [HttpGet]
        public DataTableResponse GetallAccepted(DataTableRequest request)
        {
            IQueryable<SoftwareAcceptsRequest> requestdb = _db.SoftwareAcceptsRequests.Include(x => x.SoftwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareAcceptsRequests.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var pagedFiles = requestdb.Where(u => u.DepartmentName != null || u.DepartmentName == null);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareAcceptsRequests.Where(u => u.DepartmentName != null || u.DepartmentName == null).Count(),
                recordsFiltered = requestdb.Where(u => u.DepartmentName != null || u.DepartmentName == null).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareAcceptsRequest, SoftwareAcceptsRequestDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/SoftwareRequest/v4/Get")]
        [HttpGet]
        public DataTableResponse GetAssigned(DataTableRequest request)
        {
            IQueryable<ProgrammerReport> requestdb = _db.ProgrammerReport.Include(x=>x.SoftwareUserRequest).Include(x=>x.SoftwareAcceptsRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.ProgrammerReport.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var email = User.Identity.GetLogEmail();
            var newRequest = "Request New System";
            var pagedFiles = requestdb.Where(u => u.ProgrammerEmail == email && u.SoftwareUserRequest.SoftwareName != newRequest);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.ProgrammerReport.Where(u => u.ProgrammerEmail == email && u.SoftwareUserRequest.SoftwareName != newRequest ).Count(),
                recordsFiltered = requestdb.Where(u => u.ProgrammerEmail == email && u.SoftwareUserRequest.SoftwareName != newRequest ).Count(),
                data = pagedFiles.Select(Mapper.Map<ProgrammerReport, ProgrammerReportDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/uploafs/v4/Get")]
        [HttpGet]
        public DataTableResponse GetAllUploads(DataTableRequest request)
        {
            IQueryable<ProgrammerUploads> requestdb = _db.ProgrammerUploads.Include(x => x.ProgrammerReport).OrderByDescending(x => x.Id);

            var newFilter = _db.ProgrammerReport.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.Id.ToString().Contains(searchTect.ToLower()));
            }

            var pagedFiles = requestdb.Where(u => u.FileName != null || u.FileName == null);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.ProgrammerUploads.Where(u => u.FileName != null || u.FileName == null).Count(),
                recordsFiltered = requestdb.Where(u => u.FileName != null || u.FileName == null).Count(),
                data = pagedFiles.Select(Mapper.Map<ProgrammerUploads, ProgrammerUploads>).ToArray(),
                error = ""
            };
        }


        [Route("api/uploahr/v4/Get")]
        [HttpGet]
        public DataTableResponse GetHardwareUps(DataTableRequest request)
        {
            IQueryable<TechnicianUploads> requestdb = _db.TechnicianUploads.Include(x => x.TechnicianReport).OrderByDescending(x => x.Id);

            var newFilter = _db.TechnicianUploads.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.Id.ToString().Contains(searchTect.ToLower()));
            }

            var pagedFiles = requestdb.Where(u => u.FileName != null || u.FileName == null);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.TechnicianUploads.Where(u => u.FileName != null || u.FileName == null).Count(),
                recordsFiltered = requestdb.Where(u => u.FileName != null || u.FileName == null).Count(),
                data = pagedFiles.Select(Mapper.Map<TechnicianUploads, TechnicianUploads>).ToArray(),
                error = ""
            };
        }

        [Route("api/Developer/Deletion/Get")]
        [HttpGet]
        public DataTableResponse GetRports3(DataTableRequest request)
        {
            IQueryable<ProgrammerReport> requestdb = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).Include(x => x.SoftwareAcceptsRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.ProgrammerReport.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var pagedFiles = requestdb.Where(u => u.SoftwareName != null || u.SoftwareName == null );


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.ProgrammerReport.Where(u => u.SoftwareName != null || u.SoftwareName == null).Count(),
                recordsFiltered = requestdb.Where(u => u.SoftwareName != null || u.SoftwareName == null).Count(),
                data = pagedFiles.Select(Mapper.Map<ProgrammerReport, ProgrammerReportDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/SoftwareRequest/admin/Get")]
        [HttpGet]
        public DataTableResponse GetAssignedAdmin(DataTableRequest request)
        {
            IQueryable<ProgrammerReport> requestdb = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.ProgrammerReport.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var email = User.Identity.GetLogEmail();
            var pagedFiles = requestdb.Where(u => u.ProgrammerEmail == email);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.ProgrammerReport.Where(u => u.ProgrammerEmail == email).Count(),
                recordsFiltered = requestdb.Where(u => u.ProgrammerEmail == email ).Count(),
                data = pagedFiles.Select(Mapper.Map<ProgrammerReport, ProgrammerReportDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/SoftwareRequest/newSystem/Get")]
        [HttpGet]
        public DataTableResponse GetNewSystem(DataTableRequest request)
        {
            IQueryable<ProgrammerReport> requestdb = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.ProgrammerReport.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var newRequest = "Request New System";
            var email = User.Identity.GetLogEmail();

            var pagedFiles = requestdb.Where(u => u.ProgrammerEmail == email && u.SoftwareUserRequest.SoftwareName == newRequest );


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.ProgrammerReport.Where(u => u.ProgrammerEmail == email && u.SoftwareUserRequest.SoftwareName == newRequest && u.SoftwareName == newRequest).Count(),
                recordsFiltered = requestdb.Where(u => u.ProgrammerEmail == email && u.SoftwareUserRequest.SoftwareName == newRequest && u.SoftwareName == newRequest).Count(),
                data = pagedFiles.Select(Mapper.Map<ProgrammerReport, ProgrammerReportDto>).ToArray(),
                error = ""
            };
        }


        [Route("api/SoftwareRequest/progress/Get")]
        [HttpGet]
        public DataTableResponse GetProgess(DataTableRequest request)
        {
            IQueryable<ProgrammerReport> requestdb = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.ProgrammerReport.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var progress = "In Progress";
            var manual = "Manual";
            var newRequest = "Request New System";
            var pagedFiles = requestdb.Where(u => u.SoftwareUserRequest.Status == progress || u.SoftwareUserRequest.SoftwareName != newRequest || u.SoftwareUserRequest.Status == manual);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.ProgrammerReport.Where(u => u.SoftwareUserRequest.Status == progress).Count(),
                recordsFiltered = requestdb.Where(u => u.SoftwareUserRequest.Status == progress ).Count(),
                data = pagedFiles.Select(Mapper.Map<ProgrammerReport, ProgrammerReportDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/SoftwareRequest/Resolved/v2/Get")]
        [HttpGet]
        public DataTableResponse GetResolved(DataTableRequest request)
        {
            IQueryable<ProgrammerReport> requestdb = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).Include(x=>x.SoftwareAcceptsRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.ProgrammerReport.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var resloved = "Resolved";
            var pagedFiles = requestdb.Where(u => u.SoftwareUserRequest.Status == resloved);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.ProgrammerReport.Where(u => u.SoftwareUserRequest.Status == resloved).Count(),
                recordsFiltered = requestdb.Where(u => u.SoftwareUserRequest.Status == resloved).Count(),
                data = pagedFiles.Select(Mapper.Map<ProgrammerReport, ProgrammerReportDto>).ToArray(),
                error = ""
            };
        }
        [Route("api/SoftwareRequest/developer/v2/Get")]
        [HttpGet]
        public DataTableResponse GetDEveloper(DataTableRequest request)
        {
            IQueryable<ProgrammerReport> requestdb = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.ProgrammerReport.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var pagedFiles = requestdb.Where(u => u.SoftwareName == null || u.SoftwareName != null);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.ProgrammerReport.Where(u => u.SoftwareName == null || u.SoftwareName != null).Count(),
                recordsFiltered = requestdb.Where(u => u.SoftwareName == null || u.SoftwareName != null).Count(),
                data = pagedFiles.Select(Mapper.Map<ProgrammerReport, ProgrammerReportDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/SoftwareRequest/Verified/v2/Get")]
        [HttpGet]
        public DataTableResponse GetVerifiedsr(DataTableRequest request)
        {
            IQueryable<SoftwareVerification> requestdb = _db.SoftwareVerification.Include(x => x.SoftwareUserRequest).Include(x=>x.ProgrammerReport).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareVerification.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }
            var verified = "Verified";
            var pagedFiles = requestdb.Where(u => u.SoftwareUserRequest.Status == verified);

            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareVerification.Where(u => u.SoftwareUserRequest.Status == verified).Count(),
                recordsFiltered = requestdb.Where(u => u.SoftwareUserRequest.Status == verified).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareVerification, SoftwareVerificationDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/SoftwareRequest/Remove/v2/Get")]
        [HttpGet]
        public DataTableResponse GetAllVerifiedsr(DataTableRequest request)
        {
            IQueryable<SoftwareVerification> requestdb = _db.SoftwareVerification.Include(x => x.SoftwareUserRequest).Include(x => x.ProgrammerReport).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareVerification.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }
            var pagedFiles = requestdb.Where(u => u.NameVerified != null || u.NameVerified == null);

            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareVerification.Where(u => u.NameVerified != null || u.NameVerified == null).Count(),
                recordsFiltered = requestdb.Where(u => u.NameVerified != null || u.NameVerified == null).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareVerification, SoftwareVerificationDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/SoftwareRequest/newSystem2/Get")]
        [HttpGet]
        public DataTableResponse GetNewSystem2(DataTableRequest request)
        {
            IQueryable<ProgrammerReport> requestdb = _db.ProgrammerReport.Include(x => x.SoftwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.ProgrammerReport.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var newRequest = "Request New System";
            var pagedFiles = requestdb.Where(u => u.SoftwareUserRequest.SoftwareName == newRequest);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.ProgrammerReport.Where(u => u.SoftwareUserRequest.SoftwareName == newRequest && u.SoftwareName == newRequest).Count(),
                recordsFiltered = requestdb.Where(u => u.SoftwareUserRequest.SoftwareName == newRequest && u.SoftwareName == newRequest).Count(),
                data = pagedFiles.Select(Mapper.Map<ProgrammerReport, ProgrammerReportDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/SoftwareRequest/Approved/Get")]
        [HttpGet]
        public DataTableResponse GetApproved2(DataTableRequest request)
        {
            IQueryable<SoftwareApproved> requestdb = _db.SoftwareApproveds.Include(x => x.SoftwareUserRequest).Include(x=>x.ProgrammerReport).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareApproveds.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var approved = "Approved";
            var pagedFiles = requestdb.Where(u => u.SoftwareUserRequest.Status == approved);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareApproveds.Where(u => u.SoftwareUserRequest.Status == approved).Count(),
                recordsFiltered = requestdb.Where(u => u.SoftwareUserRequest.Status ==approved).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareApproved, SoftwareApprovedDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/SoftwareRequest/allapproved/Get")]
        [HttpGet]
        public DataTableResponse GetAllApproved2(DataTableRequest request)
        {
            IQueryable<SoftwareApproved> requestdb = _db.SoftwareApproveds.Include(x => x.SoftwareUserRequest).Include(x => x.ProgrammerReport).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareApproveds.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Id.ToString().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.Ticket.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.DateAdded.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.FullName.ToLower().Contains(searchTect.ToLower()));
            }

            var pagedFiles = requestdb.Where(u => u.EmailApproval != null || u.EmailApproval == null);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareApproveds.Where(u => u.EmailApproval != null || u.EmailApproval == null).Count(),
                recordsFiltered = requestdb.Where(u => u.EmailApproval != null || u.EmailApproval == null).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareApproved, SoftwareApprovedDto>).ToArray(),
                error = ""
            };
        }




        [Route("api/SoftwareRequest/logs/Get")]
        [HttpGet]
        public DataTableResponse GetSoftwareRequestLogs(DataTableRequest request)
        {
            IQueryable<RequestHistory> requestdb = _db.RequestHistory.Include(x => x.Departments).Include(x => x.Divisions).Include(x=>x.SoftwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.RequestHistory.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.Departments.Id.ToString().Contains(searchTect.ToLower()) || x.Departments.Name.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.UserName.ToLower().Contains(searchTect.ToLower()) || x.UploadDate.ToLower().Contains(searchTect.ToLower()));
            }

            var pagedFiles = requestdb.Where(u => u.Category == true);


            //if (request.Order.Any())
            //{
            //    foreach (var order in request.Order)
            //    {
            //        pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
            //    }
            //}
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.RequestHistory.Where(u => u.Category == true).Count(),
                recordsFiltered = requestdb.Where(u => u.Category == true).Count(),
                data = pagedFiles.Select(Mapper.Map<RequestHistory, RequestHistory>).ToArray(),
                error = ""
            };
        }



        [Route("api/HardwareRequest/logs/Get")]
        [HttpGet]
        public DataTableResponse GetHardwareequestLogs(DataTableRequest request)
        {
            IQueryable<RequestHistory> requestdb = _db.RequestHistory.Include(x => x.Departments).Include(x => x.Divisions).Include(x => x.SoftwareUserRequest).OrderByDescending(x => x.Id);

            var newFilter = _db.RequestHistory.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                requestdb = requestdb.Where(x => x.Id.ToString().Contains(searchTect.ToLower()) || x.Departments.Id.ToString().Contains(searchTect.ToLower()) || x.Departments.Name.ToLower().Contains(searchTect.ToLower()) || x.SoftwareUserRequest.SoftwareName.ToLower().Contains(searchTect.ToLower()) || x.UserName.ToLower().Contains(searchTect.ToLower()) || x.UploadDate.ToLower().Contains(searchTect.ToLower()));
            }

            var pagedFiles = requestdb.Where(u => u.Category == false);


            //if (request.Order.Any())
            //{
            //    foreach (var order in request.Order)
            //    {
            //        pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
            //    }
            //}
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x => x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.RequestHistory.Where(u => u.Category == false).Count(),
                recordsFiltered = requestdb.Where(u => u.Category == false).Count(),
                data = pagedFiles.Select(Mapper.Map<RequestHistory, RequestHistory>).ToArray(),
                error = ""
            };
        }
    }

}

