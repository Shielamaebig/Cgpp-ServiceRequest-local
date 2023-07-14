using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class HardwareVerificationDto
    {
        public int Id { get; set; }
        public string DateVerified { get; set; }
        public string NameVerified { get; set; }
        public string EmailVerified { get; set; }
    }
}