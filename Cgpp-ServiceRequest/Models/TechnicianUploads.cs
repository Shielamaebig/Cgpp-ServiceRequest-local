using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class TechnicianUploads
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public TechnicianReport TechnicianReport { get; set; }
        public int TechnicianReportId { get; set; }
        public string FileExtension { get; set; }
        public Ftp Ftp { get; set; }
        public int? FtpId { get; set; }
        public string DateAdded { get; set; }
    }
}