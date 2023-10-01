using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class TechnicianReport
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string TechnicianName { get; set; }
        public string TechEmail { get; set; }
        public string DateAssigned { get; set; }
        public HardwareUserRequest HardwareUserRequest { get; set; }
        public int? HardwareUserRequestId { get; set; }

        public int? HardwareId { get; set; }
        public Hardware Hardware { get; set; }
        public string HardwareName { get; set; }
        public int? UnitTypeId { get; set; }
        public UnitType UnitType { get; set; }
        public string UniTypes { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string Status { get; set; }
        public string DateStarted { get; set; }
        public string DateEnded { get; set; }
        public HardwareTechnician HardwareTechnician { get; set; }
        public int HardwareTechnicianId { get; set; }
        public string Remarks { get; set; }
        public Finding Finding { get; set; }
        public int? FindingId { get; set; }
        public string FindingName { get; set; }
        public string PossibleCause { get; set;}
        public string SerialNumber { get; set; }
        public string ControlNumber { get; set; }
        public string AdminName { get; set; }
        public SuperAdmin SuperAdmin { get; set; }
        public int? SuperAdminId { get; set; }
        public string SuperName { get; set; }
        public string SmsMessage { get; set; }
        public string DateSend { get; set; }
    }
}