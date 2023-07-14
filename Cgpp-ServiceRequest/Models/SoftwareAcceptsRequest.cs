using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class SoftwareAcceptsRequest
    {
        public int Id { get; set; }
        public string DateAdded { get; set; }
        public SoftwareUserRequest SoftwareUserRequest { get; set; }
        public int SoftwareUserRequestId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Departments Departments { get; set; }
        public int DepartmentsId { get; set; }
        public string DepartmentName { get; set; }
        public Divisions Divisions { get; set; }
        public int DivisionsId { get; set; }
        public string DivisionName { get; set; }
        public string IsAccept { get; set; }
    }
}