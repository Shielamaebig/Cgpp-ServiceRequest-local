using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.Dtos
{
    public class RequestHistoryDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int DepartmentsId { get; set; }
        public int DivisionsId { get; set; }
        public Departments Departments { get; set; }
        public Divisions Divisions { get; set; }
        public string UploadMessage { get; set; }
        public string UploadDate { get; set; }
        public bool Category { get; set; }
        public HardwareUserRequest HardwareUserRequest { get; set; }
        public int HardwareUserRequestId { get; set; }
        public SoftwareUserRequest SoftwareUserRequest { get; set; }
        public int SoftwareUserRequestId { get; set; }
    }
}