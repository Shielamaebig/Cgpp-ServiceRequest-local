using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class HardwareAcceptsRequestDto
    {
        public int Id { get; set; }
        public string DateAdded { get; set; }
        public HardwareUserRequestDto HardwareUserRequest { get; set; }
        public int HardwareUserRequestId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DepartmentDto Departments { get; set; }
        public int DepartmentsId { get; set; }
        public string DepartmentName { get; set; }
        public DivisionDto Divisions { get; set; }
        public int DivisionsId { get; set; }
        public string DivisionName { get; set; }
        public string IsAccept { get; set; }
    }
}