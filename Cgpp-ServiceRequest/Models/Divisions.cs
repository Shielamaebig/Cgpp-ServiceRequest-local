using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class Divisions
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string DateAdded { get; set; }

        public int DepartmentsId { get; set; }
        public Departments Departments { get; set; }
        public bool IsDivisionApprover { get; set; }
    }
}