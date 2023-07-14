using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class SoftwareTechnicianDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TechEmail { get; set; }
        public string DateAdded { get; set; }
        public string PhoneNumber { get; set; }

    }
}