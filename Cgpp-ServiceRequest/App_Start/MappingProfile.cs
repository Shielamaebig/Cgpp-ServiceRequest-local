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
            Mapper.CreateMap<Finding, FindingDto>().ReverseMap();
            Mapper.CreateMap<InformationSystem, InformationSystemDto>().ReverseMap();
            Mapper.CreateMap<LoginActivity, LoginActivityDto>().ReverseMap();
            Mapper.CreateMap<RequestHistory, RequestHistoryDto>().ReverseMap();
            Mapper.CreateMap<MaintenanceMode, MaintenanceModeDto>().ReverseMap();
            Mapper.CreateMap<UserRegistration, UserRegistrationDto>().ReverseMap();
            Mapper.CreateMap<UserForgotPassword, UserForgotPasswordsDto>().ReverseMap();
            Mapper.CreateMap<HardwareUserRequest, HardwareUserRequestDto>().ReverseMap();
            Mapper.CreateMap<TechnicianReport, TechnicianReportDto>().ReverseMap();
            Mapper.CreateMap<HardwareApproval, HardwareApprovalDto>().ReverseMap();
            Mapper.CreateMap<HardwareTask, HardwareTaskDto>().ReverseMap();
            Mapper.CreateMap<HardwareUserUploads, HardwareUserUploadsDto>().ReverseMap();
            Mapper.CreateMap<HardwareVerify, HardwareVerifyDto>().ReverseMap();
            Mapper.CreateMap<TechnicianUploads, TechnicianUploadsDto>().ReverseMap();
            Mapper.CreateMap<SoftwareUserRequest, SoftwareUserRequestDto>().ReverseMap();
            Mapper.CreateMap<ProgrammerReport, ProgrammerReportDto>().ReverseMap();
            Mapper.CreateMap<SoftwareUserUploads, SoftwareUserUploadsDto>().ReverseMap();
            Mapper.CreateMap<SoftwareAcceptsRequest, SoftwareAcceptsRequestDto>().ReverseMap();
            Mapper.CreateMap<ProgrammerUploads, ProgrammerUploadsDto>().ReverseMap();  
            Mapper.CreateMap<SoftwareVerification, SoftwareVerificationDto>().ReverseMap();
            Mapper.CreateMap<SoftwareApproved, SoftwareApprovedDto>().ReverseMap();
            Mapper.CreateMap<SoftwareTaskList, SoftwareTaskListDto>().ReverseMap();
            Mapper.CreateMap<Draft, DraftsDto>().ReverseMap();
            Mapper.CreateMap<SoftwareTechnician, softwarePhoneNumberDto>().ReverseMap();
            Mapper.CreateMap<HardwareTasksList, HardwareTasksListDto>().ReverseMap();
            Mapper.CreateMap<SuperAdmin, SuperAdminDto>().ReverseMap();
            Mapper.CreateMap<TechnicianAdmin, TechnicianAdminDto>().ReverseMap();
            Mapper.CreateMap<SoftwareVerificationCheckboxDto, SoftwareVerification>().ReverseMap();
            Mapper.CreateMap<Ftp, FtpDto>().ReverseMap();
            Mapper.CreateMap<HardwareAcceptsRequest, HardwareAcceptsRequestDto>().ReverseMap();

            Mapper.CreateMap<Draft, DraftsDto>().ForMember(d => d.draftID, opt => opt.Ignore());
            Mapper.CreateMap<DivisionSpecDto, Divisions>().ForMember(d => d.Id, opt => opt.Ignore());
            Mapper.CreateMap<HardwareUserRequestDto, HardwareUserRequest>().ForMember(s=>s.Id, opt => opt.Ignore());
            Mapper.CreateMap<TechnicianReportDto, TechnicianReport>().ForMember(s => s.Id, opt => opt.Ignore());
            Mapper.CreateMap<HardwareApprovalDto, HardwareApproval>().ForMember(s => s.Id, opt => opt.Ignore());
            Mapper.CreateMap<TechnicianUploadsDto, TechnicianUploads>().ForMember(s=>s.Id, opt => opt.Ignore());
            Mapper.CreateMap<SoftwareUserRequestDto, SoftwareUserRequest>().ForMember(s => s.Id, opt => opt.Ignore());
            Mapper.CreateMap<ProgrammerReportDto, ProgrammerReport>().ForMember(s => s.Id, opt => opt.Ignore());
            Mapper.CreateMap<SoftwareUserUploadsDto, SoftwareUserUploads>().ForMember(s => s.Id, opt => opt.Ignore());
            Mapper.CreateMap<SoftwareAcceptsRequestDto, SoftwareAcceptsRequest>().ForMember(s => s.Id, opt => opt.Ignore());
            Mapper.CreateMap<ProgrammerUploadsDto, ProgrammerUploads>().ForMember(s => s.Id, opt => opt.Ignore());
            Mapper.CreateMap<SoftwareVerificationDto, SoftwareVerification>().ForMember(s => s.Id, opt => opt.Ignore());
            Mapper.CreateMap<SoftwareApprovedDto, SoftwareApproved>().ForMember(s => s.Id, opt => opt.Ignore());
            Mapper.CreateMap<SoftwareTaskListDto, SoftwareTaskList>().ForMember(s => s.Id, opt => opt.Ignore());
            Mapper.CreateMap<HardwareTasksListDto, HardwareTasksList>().ForMember(s=>s.Id, opt => opt.Ignore());
            Mapper.CreateMap<HardwareAcceptsRequestDto, HardwareAcceptsRequest>().ForMember (s=>s.Id, opt => opt.Ignore());
        }
    }
}