using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class HardwareVerification
    {
        public int Id { get; set; }
        public string DateVerified { get; set; }
        public string NameVerified { get; set; }
        public string EmailVerified { get; set; }

    }
}