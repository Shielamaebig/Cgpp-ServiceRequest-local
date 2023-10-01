using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class SoftwareVerification
    {
        public int Id { get; set; }
        public string DateVerified { get; set; }
        public string NameVerified { get; set; }
        public string EmailVerified { get; set; }
        public SoftwareUserRequest SoftwareUserRequest { get; set; }
        public int? SoftwareUserRequestId { get; set; }
        public ProgrammerReport ProgrammerReport { get; set; }
        public int ProgrammerReportId { get; set; }

    }
}