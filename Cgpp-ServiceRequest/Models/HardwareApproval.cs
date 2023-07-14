using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class HardwareApproval
    {
        public int Id { get; set; }
        public string NameApproval { get; set; }
        public string EmailApproval { get; set; }
        public string RemarksApproval { get; set; }
        public string DateApprove { get; set; }
        public HardwareUserRequest HardwareUserRequest { get; set; }
        public int? HardwareUserRequestId { get; set; }
        public TechnicianReport TechnicianReport { get; set; }
        public int? TechnicianReportId { get; set; }
        public HardwareVerify HardwareVerify { get; set; }
        public int? HardwareVerifyId { get; set; }
    }
}