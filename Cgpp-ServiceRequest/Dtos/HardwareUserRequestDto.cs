using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class HardwareUserRequestDto
    {
        public int Id { get; set; }
        public string Ticket { get; set; }
        public DateTime DateCreated { get; set; }
        public string DateAdded { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int DepartmentsId { get; set; }
        public Departments Departments { get; set; }
        public string DepartmentName { get; set; }
        public int? DivisionsId { get; set; }
        public Divisions Divisions { get; set; }
        public string DivisionName { get; set; }
        public string MobileNumber { get; set; }
        public int HardwareId { get; set; }
        public Hardware Hardware { get; set; }
        public string HardwareName { get; set; }
        [Required]
        public int UnitTypeId { get; set; }
        public UnitType UnitType { get; set; }
        public string UniTypes { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        [Required]
        public string DocumentLabel { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsNew { get; set; }
        public string Status { get; set; }
        public bool IsManual { get; set; }
    }
}