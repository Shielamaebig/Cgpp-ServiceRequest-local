using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class ProgrammerUploads
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public ProgrammerReport ProgrammerReport { get; set; }
        public int ProgrammerReportId { get; set; }
        public string DateAdded { get; set; }
        public string RemarksUploads { get; set; }
    }
}