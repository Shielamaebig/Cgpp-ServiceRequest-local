using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cgpp_ServiceRequest.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cgpp_ServiceRequest.Models
{
    public class HardwareUploads
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public HardwareRequest HardwareRequest { get; set; }
        public int HardwareRequestId { get; set; }
        public string DateAdded { get; set; }

    }
}