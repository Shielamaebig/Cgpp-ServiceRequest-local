using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class DivisionSpecDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DateAdded { get; set; }

        public DepartmentDto Departments { get; set; }
        public int DepartmentsId { get; set; }
    }
}