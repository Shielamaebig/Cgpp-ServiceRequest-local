using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.Dtos
{
    public class SoftwareRequestSpecDto
    {

        public int Id { get; set; }
        public string Ticket { get; set; }
        public string DateAdded { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string FullName { get; set; }
        public string RequestFor { get; set; }
        public string Description { get; set; }
        public int? InformationSystemId { get; set; }
        public InformationSystem InformationSystem { get; set; }
        public int DepartmentsId { get; set; }
        public Departments Departments { get; set; }
        public int DivisionsId { get; set; }
        public Divisions Divisions { get; set; }
        public int SoftwareId { get; set; }
        public Software Software { get; set; }
        public int SoftwareTechnicianId { get; set; }
        public SoftwareTechnician SoftwareTechnician { get; set; }
        public string ProEmail { get; set; }
        public bool IsAssigned { get; set; }
        public string PossibleCause { get; set; }
        public string DateStarted { get; set; }
        public string DateEnded { get; set; }
        public string TimeStarted { get; set; }
        public string TimeEnded { get; set; }
        public string Remarks { get; set; }
        public bool IsReported { get; set; }
        public string DateApproved { get; set; }
        public string Approver { get; set; }
        public string ApproverRemarks { get; set; }
        public bool IsApproved { get; set; }
        public string DateView { get; set; }
        public string Viewer { get; set; }
        public bool IsViewed { get; set; }
        public string DocumentLabel { get; set; }
        public bool IsNew { get; set; }
    }
}