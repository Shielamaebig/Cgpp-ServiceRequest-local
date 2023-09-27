using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using AutoMapper;
using Cgpp_ServiceRequest.Dtos;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Departments, DepartmentDto>().ReverseMap();
            Mapper.CreateMap<Divisions, DivisionDto>().ReverseMap();
            Mapper.CreateMap<Divisions, DivisionSpecDto>().ReverseMap();
            Mapper.CreateMap<UnitType, UnitTypeDto>().ReverseMap();
            Mapper.CreateMap<SoftwareTechnician, SoftwareTechnicianDto>().ReverseMap();
            Mapper.CreateMap<HardwareTechnician, HardwareTechnicianDto>().ReverseMap();
            Mapper.CreateMap<Software, SoftwareDto>().ReverseMap();
            Mapper.CreateMap<Hardware, HardwareDto>().ReverseMap();
            Mapper.CreateMap<HardwareRequest, HardwareRequestDto>().ReverseMap();
            Mapper.CreateMap<HardwareRequest, HardwareRequestSpecDto>().ReverseMap();
            Mapper.CreateMap<Status, StatusDto>().ReverseMap();
            Mapper.CreateMap<Finding, FindingDto>().ReverseMap();
            Mapper.CreateMap<SoftwareRequest, SoftwareRequestDto>().ReverseMap();
            Mapper.CreateMap<SoftwareRequest, SoftwareRequestSpecDto>().ReverseMap();
            Mapper.CreateMap<InformationSystem, InformationSystemDto>().ReverseMap();
            Mapper.CreateMap<LoginActivity, LoginActivityDto>().ReverseMap();
            Mapper.CreateMap<RequestHistory, RequestHistoryDto>().ReverseMap();
            Mapper.CreateMap<SoftwareRequestHistory, SoftwareRequestHistoryDto>().ReverseMap();
            Mapper.CreateMap<HardwareRequestHistory, HardwareRequestHistoryDto>().ReverseMap();
            Mapper.CreateMap<HardwareUploads, HardwareUploadsDto>().ReverseMap();
            Mapper.CreateMap<SoftwareUploads, SoftwareUploadsDto>().ReverseMap();
            Mapper.CreateMap<MaintenanceMode, MaintenanceModeDto>().ReverseMap();

            Mapper.CreateMap<DivisionSpecDto, Divisions>().ForMember(d => d.Id, opt => opt.Ignore());
            Mapper.CreateMap<HardwareRequestSpecDto, HardwareRequest>().ForMember(h => h.Id, opt => opt.Ignore());
            Mapper.CreateMap<SoftwareRequestSpecDto, SoftwareRequest>().ForMember(s => s.Id, opt => opt.Ignore());

        }
    }
}