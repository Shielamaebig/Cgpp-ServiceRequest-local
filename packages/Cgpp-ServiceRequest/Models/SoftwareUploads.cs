using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cgpp_ServiceRequest.Models;
using System.Data.Entity;

namespace Cgpp_ServiceRequest.Models
{
    public class SoftwareUploads
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public SoftwareRequest SoftwareRequest { get; set; }
        public int SoftwareRequestId { get; set; }
        public string DateAdded { get; set; }
    }
}