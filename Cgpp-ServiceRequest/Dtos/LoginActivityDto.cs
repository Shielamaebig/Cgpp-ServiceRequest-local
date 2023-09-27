using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class LoginActivityDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ActivityMessage { get; set; }
        public string ActivityDate { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionName { get; set; }
    }
}