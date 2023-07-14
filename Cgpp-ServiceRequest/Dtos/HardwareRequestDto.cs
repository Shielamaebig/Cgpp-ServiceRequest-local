using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.Dtos
{
    public class HardwareRequestDto
    {
        public int Id { get; set; }
        public string Ticket { get; set; }
        public string DateAdded { get; set; }
        public string ModelName { get; set; }
        public string MobileNumber { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public int HardwareId { get; set; }
        public int StatusId { get; set; }
        public int HardwareTechnicianId { get; set; }
        public HardwareTechnician HardwareTechnician { get; set; }
        public bool IsAssigned { get; set; }
        public string TechEmail { get; set; }
        public string Email { get; set; }
        public int DepartmentsId { get; set; }
        public int DivisionsId { get; set; }
        public int UnitTypeId { get; set; }
        public string BrandName { get; set; }
        public string PossibleCause { get; set; }
        public int FindingId { get; set; }
        public string DateStarted { get; set; }
        public string DateEnded { get; set; }
        public bool IsReported { get; set; }
        public bool IsVerified { get; set; }
        public string TimeStarted { get; set; }
        public string TimeEnded { get; set; }
        public string Remarks { get; set; }
        public string DateApproved { get; set; }
        public string Approver { get; set; }
        public string ApproverRemarks { get; set; }
        public bool IsApproved { get; set; }
        public string DateView { get; set; }
        public string Viewer { get; set; }
        public string ViewedRemarks { get; set; }
        public string DocumentLabel { get; set; }
        public bool IsNew { get; set; }
        public string SerialNumber { get; set; }
        public string ControlNumber { get; set; }
    }
}