using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class ProgrammerReport
    {
        public int Id { get; set; }
        public string ProgrammerName { get; set; }
        public string ProgrammerEmail { get; set; }
        public SoftwareTechnician SoftwareTechnician { get; set; }
        public int SoftwareTechnicianId { get; set; }
        public string SoftwareTechnicianName { get; set; }
        public string DateAdded { get; set; }
        public string DateAssigned { get; set; }
        public string DateEnded { get; set; }
        public DateTime DateCreated { get; set; }
        public Software Software { get; set; }
        public int? SoftwareId { get; set; }
        public string SoftwareName { get; set;}
        public InformationSystem InformationSystem { get; set; }
        public int? InformationSystemId { get; set; }
        public string InformationName { get; set; }
        public string RequestFor { get; set; }
        public string Description { get; set; }
        public string DocumentLabel { get; set; }
        public string Remarks { get; set; }
        public string AdminName { get; set; }
        public string SuperAdminName { get; set;}
        public SoftwareUserRequest SoftwareUserRequest { get; set; }
        public int? SoftwareUserRequestId { get; set; }
        public string Resulotion { get; set; }

        [Range(1, 100)]
        public string ProgressStatus { get; set; }
        public string ProgressRemarks { get; set; }
        public string DateVerified { get; set; }
        public string DateApproved { get; set; }
        public SoftwareAcceptsRequest SoftwareAcceptsRequest { get; set; }
        public int? SoftwareAcceptsRequestId { get; set; }
        public string RemarksApproval { get; set; }
        public string SmsMessage { get; set; }
        public string DateSend { get; set; }

    }
}