using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class HardwareTaskList
    {
        public int Id { get; set; }
        public HardwareTask HardwareTask { get; set; }
        public int HardwareTaskId { get; set; }
        public string DateAdded { get; set; }
        public HardwareUserRequest HardwareUserRequest { get; set; }
        public int? HardwareUserRequestId { get; set; }
        public TechnicianReport TechnicianReport { get; set; }
        public int? TechnicianReportId { get; set; }
        public HardwareVerify HardwareVerify { get; set; }
        public int? HardwareVerifyId { get; set; }
        public HardwareApproval HardwareApproval { get; set; }
        public int? HardwareApprovalId { get;set; }

    }
}