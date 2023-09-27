using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class MaintenanceMode
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public bool IsActive { get; set; }

        public string Message { get; set; }
    }
}