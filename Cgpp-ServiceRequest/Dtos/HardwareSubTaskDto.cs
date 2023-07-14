using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class HardwareSubTaskDto
    {
        public int Id { get; set; }
        public HardwareTask HardwareTask { get; set; }
        public int HardwareTaskId { get; set; }
        public string Name { get; set; }
    }
}