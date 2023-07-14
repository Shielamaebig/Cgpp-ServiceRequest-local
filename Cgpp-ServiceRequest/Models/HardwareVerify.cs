using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class HardwareVerify
    {
        public int Id { get; set; }
        public string DateVerified { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public HardwareUserRequest HardwareUserRequest { get; set; }
        public int HardwareUserRequestId { get; set; }
        public TechnicianReport TechnicianReport { get; set; }
        public int? TechnicianReportId { get; set; }

    }
}