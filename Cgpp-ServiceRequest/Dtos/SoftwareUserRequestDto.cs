using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class SoftwareUserRequestDto
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string DateAdded { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public Departments Departments { get; set; }
        public int DepartmentsId { get; set; }
        public string DepartmentName { get; set; }
        public Divisions Divisions { get; set; }
        public int DivisionsId { get; set; }
        public string DivisionName { get; set; }
        public string Status { get; set; }
        public bool IsNew { get; set; }
        public bool IsManual { get; set; }
        public Software Software { get; set; }
        public int SoftwareId { get; set; }
        public string SoftwareName { get; set; }
        public string Ticket { get; set; }
        public string RequestFor { get; set; }
        public string Description { get; set; }
        public string DocumentLabel { get; set; }
        public InformationSystem InformationSystem { get; set; }
        public int InformationSystemId { get; set; }
        public string InformationName { get; set; }
        public string ProRemarks { get; set; }
        public string DateRemarksProg { get; set; }
        public string NameProg { get; set; }
        public string TelNumber { get; set; }
        public string SmsMessage { get; set; }
    }
}