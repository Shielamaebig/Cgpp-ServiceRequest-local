using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class HardwareSubTask
    {
        public int Id { get; set; }
        public HardwareTask HardwareTask { get; set; }
        public int HardwareTaskId { get; set; }
        public string Name { get; set; }
    }
}