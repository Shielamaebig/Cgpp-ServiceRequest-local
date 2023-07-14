using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.ViewModels
{
    public class hardwareUploadDisplayViewModel
    {
        public int Id { get; set; }
        public int HardwareRequestId { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public string DateAdded { get; set; }

    }
}