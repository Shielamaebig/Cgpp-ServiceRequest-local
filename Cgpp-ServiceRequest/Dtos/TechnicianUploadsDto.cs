using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class TechnicianUploadsDto
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public TechnicianReport TechnicianReport { get; set; }
        public int TechnicianReportId { get; set; }
        public string FileExtension { get; set; }
        public Ftp Ftp { get; set; }
        public int FtpId { get; set; }
        public string DateAdded { get; set; }
        public byte[] DocumentBlob { get; set; }

    }
}