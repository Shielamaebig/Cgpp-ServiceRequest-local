using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class SoftwareTaskListDto
    {
        public int Id { get; set; }
        public SoftwareTask SoftwareTask { get; set; }
        public int SoftwareTaskId { get; set; }
        public string DateAdded { get; set; }
        public string State { get; set; }
        public SoftwareUserRequest SoftwareUserRequest { get; set; }
        public int SoftwareUserRequestId { get; set; }
        public SoftwareAcceptsRequest SoftwareAcceptsRequest { get; set; }
        public int SoftwareAcceptsRequestId { get; set; }
        public ProgrammerReport ProgrammerReport { get; set; }
        public int ProgrammerReportId { get; set; }
        public SoftwareVerification SoftwareVerification { get; set; }
        public int SoftwareVerificationId { get; set; }
        public SoftwareApproved SoftwareApproved { get; set; }
        public int SoftwareApprovedId { get; set; }
    }
}