using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class SoftwareTechnician
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string TechEmail { get; set; }
        public string DateAdded { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}