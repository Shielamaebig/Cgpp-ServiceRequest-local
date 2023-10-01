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
            IQueryable<RequestHistory> requestHistoryDto = _db.RequestHistory.Include(x=>x.Departments).Include(x=>x.Divisions).OrderByDescending(x => x.Id);
            var newFilter = _db.RequestHistory.Count();

            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                requestHistoryDto = requestHistoryDto.Where(x => x.UserName.ToLower().Contains(searchText.ToLower()) || x.Email.ToLower().Contains(searchText.ToLower()) || x.Departments.Name.ToLower().Contains(searchText.ToLower()) || x.Departments.Id.ToString().Contains(searchText.ToLower()) || x.Divisions.Name.ToLower().Contains(searchText.ToLower()) || x.Divisions.Id.ToString().Contains(searchText.ToLower()) || x.UploadMessage.ToLower().Contains(searchText.ToLower()) || x.UploadDate.ToLower().Contains(searchText.ToLower()));
            }
            var pagedFiles = requestHistoryDto.Where(u => u.DepartmentsId == null || u.DepartmentsId != null);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }

            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x=>x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.RequestHistory.Where(u => u.DepartmentsId == null || u.DepartmentsId != null).Count(),
                recordsFiltered = requestHistoryDto.Where(u => u.DepartmentsId == null || u.DepartmentsId != null).Count(),
                data = pagedFiles.Select(Mapper.Map<RequestHistory, RequestHistoryDto>).ToArray(),
                error = ""
            };

        }




        [Route("api/all/activity")]
        [HttpGet]
        public DataTableResponse GetAllactivity(DataTableRequest request)
        {
            IQueryable<LoginActivity> loginActivityDto = _db.LoginActivity.OrderByDescending(x=>x.Id);
            var newFilter = _db.LoginActivity.Count();

            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                loginActivityDto = _db.LoginActivity.Where(x=>x.UserName.ToLower().Contains(searchText.ToLower()) || x.Email.ToLower().Contains(searchText.ToLower()) || x.ActivityMessage.ToLower().Contains(searchText.ToLower()) || x.ActivityDate.ToLower().Contains(searchText.ToLower()));
            }

            var pagedFiles = loginActivityDto.Where(u => u.Email == null || u.Email != null);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }

            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x=>x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.LoginActivity.Where(u => u.Email == null || u.Email != null).Count(),
                recordsFiltered = loginActivityDto.Where(u => u.Email == null || u.Email != null).Count(),
                data = pagedFiles.Select(Mapper.Map<LoginActivity, LoginActivityDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/all/hardReq")]
        [HttpGet]
        public DataTableResponse GetAllHardware(DataTableRequest request)
        {
            IQueryable<HardwareRequest> hardwareRequestDto = _db.HardwareRequest.Include(x => x.Hardware).Include(x => x.Status).Include(x => x.HardwareTechnician).OrderByDescending(x => x.Id);

            var newFilter = _db.HardwareRequest.Count();

            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                hardwareRequestDto = hardwareRequestDto.Where( x=>x.Ticket.ToLower().Contains(searchText.ToLower()) || x.DateAdded.ToLower().Contains(searchText.ToLower()) || x.FullName.ToLower().Contains(searchText.ToLower()) || x.Hardware.Id.ToString().Contains(searchText.ToLower()) || x.Hardware.Name.ToLower().Contains(searchText.ToLower()) || x.HardwareTechnician.Id.ToString().Contains(searchText.ToLower())|| x.HardwareTechnician.Name.ToLower().Contains(searchText.ToLower()));
            }

            var pagedFiles = hardwareRequestDto.Where(u => u.DepartmentsId == null || u.DepartmentsId != null);


            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x=>x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.HardwareRequest.Where(u => u.DepartmentsId == null || u.DepartmentsId != null).Count(),
                recordsFiltered = hardwareRequestDto.Where(u => u.DepartmentsId == null || u.DepartmentsId != null).Count(),
                data = pagedFiles.Select(Mapper.Map<HardwareRequest, HardwareRequest>).ToArray(),
                error = ""
            };
        }

        [Route("api/hr/assigned")]
        [HttpGet]
        public DataTableResponse GetAllhrAssigned(DataTableRequest request)
        {
            IQueryable<HardwareRequest> hardwareAssignedDto = _db.HardwareRequest.Include(x => x.Hardware).Include(x => x.Status).Include(x=>x.HardwareTechnician).OrderByDescending(x => x.Id);

            var newFilter = _db.HardwareRequest.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                hardwareAssignedDto = hardwareAssignedDto.Where(x=>x.Ticket.ToLower().Contains(searchTect.ToLower()) || x.Hardware.Id.ToString().Contains(searchTect.ToLower()) || x.Hardware.Name.ToLower().Contains(searchTect.ToLower()) || x.Status.Id.ToString().Contains(searchTect.ToLower()) || x.Status.Name.ToLower().Contains(searchTect.ToLower()) || x.FullName.ToLower().Contains(searchTect.ToLower()) );
            }

            var ueamil = User.Identity.GetUserEmail();
            var pagedFiles = hardwareAssignedDto.Where(u => u.TechEmail == ueamil);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x=>x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.HardwareRequest.Where(u => u.TechEmail == ueamil).Count(),
                recordsFiltered = hardwareAssignedDto.Where(u => u.TechEmail == ueamil).Count(),
                data = pagedFiles.Select(Mapper.Map<HardwareRequest, HardwareRequestSpecDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/hr/user")]
        [HttpGet]
        public DataTableResponse GetHrUserRequest(DataTableRequest request)
        {
            IQueryable<HardwareRequest> hardwareRequestUserDto = _db.HardwareRequest.Include(x => x.Hardware).Include(x => x.Status).OrderByDescending(x => x.Id);

            var newFilter = _db.HardwareRequest.Count();

            if (request.Search.Value != "")
            {
                var searchTect = request.Search.Value.Trim();
                hardwareRequestUserDto = hardwareRequestUserDto.Where(x => x.Ticket.ToLower().Contains(searchTect.ToLower()) || x.Hardware.Id.ToString().Contains(searchTect.ToLower()) || x.Hardware.Name.ToLower().Contains(searchTect.ToLower()) || x.Status.Id.ToString().Contains(searchTect.ToLower()) || x.Status.Name.ToLower().Contains(searchTect.ToLower()) || x.FullName.ToLower().Contains(searchTect.ToLower()));
            }
            var useEmail = User.Identity.GetUserEmail();
            var pagedFiles = hardwareRequestUserDto.Where(u => u.Email == useEmail);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x=>x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.HardwareRequest.Where(u => u.Email == useEmail).Count(),
                recordsFiltered = hardwareRequestUserDto.Where(u => u.Email == useEmail).Count(),
                data = pagedFiles.Select(Mapper.Map<HardwareRequest, HardwareRequestSpecDto>).ToArray(),
                error = ""
            };
        }

        [Route("api/all/softReq")]
        [HttpGet]
        public DataTableResponse GettAllSoftware(DataTableRequest request)
        {
            IQueryable<SoftwareRequest> softwareRequestDto = _db.SoftwareRequest.Include(x => x.Software).Include(x=>x.SoftwareTechnician).Include(x=>x.Status).OrderByDescending(x=>x.Id);

            var newFilter = _db.SoftwareRequest.Count();
            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                softwareRequestDto = softwareRequestDto.Where(x => x.Ticket.ToLower().Contains(searchText.ToLower()) || x.Software.Id.ToString().Contains(searchText.ToLower()) || x.Software.Name.ToLower().Contains(searchText.ToLower()) || x.SoftwareTechnician.Id.ToString().Contains(searchText.ToLower()) || x.SoftwareTechnician.Name.ToString().Contains(searchText.ToLower()) || x.Status.Id.ToString().Contains(searchText.ToLower()) || x.Status.Name.ToLower().Contains(searchText.ToLower()) || x.FullName.ToLower().Contains(searchText.ToLower()));
            }

            var pagedFiles = softwareRequestDto.Where(u => u.DepartmentsId == null || u.DepartmentsId != null);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x=>x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareRequest.Where(u => u.DepartmentsId == null || u.DepartmentsId != null).Count(),
                recordsFiltered = softwareRequestDto.Where(u => u.DepartmentsId == null || u.DepartmentsId != null).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareRequest, SoftwareRequest>).ToArray(),
                error = ""
            };
        }

        [Route("api/sr/assigned")]
        [HttpGet]
        public DataTableResponse GetAssignedSoftware(DataTableRequest request)
        {
            IQueryable<SoftwareRequest> softwareRequestDto = _db.SoftwareRequest.Include(x => x.Software).Include(x => x.Status).Include(x => x.SoftwareTechnician).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareRequest.Count();

            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                softwareRequestDto = softwareRequestDto.Where(x => x.Ticket.ToLower().Contains(searchText.ToLower()) || x.Software.Id.ToString().Contains(searchText.ToLower()) || x.Software.Name.ToLower().Contains(searchText.ToLower()) || x.SoftwareTechnician.Id.ToString().Contains(searchText.ToLower()) || x.SoftwareTechnician.Name.ToString().Contains(searchText.ToLower()) || x.Status.Id.ToString().Contains(searchText.ToLower()) || x.Status.Name.ToLower().Contains(searchText.ToLower()) || x.FullName.ToLower().Contains(searchText.ToLower()));
            }

            var urEmail = User.Identity.GetUserEmail();
            var pagedFiles = softwareRequestDto.Where(u => u.ProEmail == urEmail);

            if (request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x=>x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareRequest.Where(u => u.ProEmail == urEmail).Count(),
                recordsFiltered = softwareRequestDto.Where(u => u.ProEmail == urEmail).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareRequest, SoftwareRequestSpecDto>).ToArray(),
                error = ""
            };

        }

        [Route("api/sr/user")]
        [HttpGet]
        public DataTableResponse GetSrUser(DataTableRequest request)
        {
            IQueryable<SoftwareRequest> softwareRequestDto = _db.SoftwareRequest.Include(x => x.Software).Include(x => x.Status).OrderByDescending(x => x.Id);

            var newFilter = _db.SoftwareRequest.Count();
            if (request.Search.Value != "")
            {
                var searchText = request.Search.Value.Trim();
                softwareRequestDto = softwareRequestDto.Where(x => x.Ticket.ToLower().Contains(searchText.ToLower()) || x.Software.Id.ToString().Contains(searchText.ToLower()) || x.Software.Name.ToLower().Contains(searchText.ToLower()) || x.SoftwareTechnician.Id.ToString().Contains(searchText.ToLower()) || x.SoftwareTechnician.Name.ToString().Contains(searchText.ToLower()) || x.Status.Id.ToString().Contains(searchText.ToLower()) || x.Status.Name.ToLower().Contains(searchText.ToLower()) || x.FullName.ToLower().Contains(searchText.ToLower()));
            }
            var userEmail = User.Identity.GetUserEmail();
            var pagedFiles = softwareRequestDto.Where(u=>u.Email == userEmail);
            if(request.Order.Any())
            {
                foreach (var order in request.Order)
                {
                    pagedFiles = pagedFiles.OrderBy(string.Format("{0} {1}", request.Columns[order.Column].Data, order.Dir));
                }
            }
            pagedFiles = pagedFiles.Skip(request.Start).Take(request.Length).OrderByDescending(x=>x.Id);

            return new DataTableResponse
            {
                draw = request.Draw,
                recordsTotal = _db.SoftwareRequest.Where(u => u.Email == userEmail).Count(),
                recordsFiltered = softwareRequestDto.Where(u => u.Email == userEmail).Count(),
                data = pagedFiles.Select(Mapper.Map<SoftwareRequest, SoftwareRequestSpecDto>).ToArray(),
                error = ""
            };
        }
    }    

}

