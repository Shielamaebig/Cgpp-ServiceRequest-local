using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class SoftwareApproved
    {
    public int Id { get; set; }
        public string NameApproval { get; set; }
        public string EmailApproval { get; set; }
        public string RemarksApproval { get; set; }
        public string DateApprove { get; set; }
        public SoftwareUserRequest SoftwareUserRequest { get; set; }
        public int? SoftwareUserRequestId { get; set; }
        public ProgrammerReport ProgrammerReport { get; set; }
        public int? ProgrammerReportId { get; set; }
        public SoftwareVerification SoftwareVerification { get; set; }
        public int? SoftwareVerificationId { get; set; }
    }
}