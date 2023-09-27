using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class UnitType
    {
        public int Id { get; set; }
        public string DateAdded { get; set; }
        [Required]
        public string Name { get; set; }
    }
}