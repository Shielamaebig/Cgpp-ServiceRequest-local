using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class HardwareUserUploads
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public HardwareUserRequest HardwareUserRequest { get; set; }
        public int HardwareUserRequestId { get; set; }
        public string DateAdded { get; set; }
        public string FileExtension { get; set; }
        public Ftp Ftp { get; set; }
        public int? FtpId { get; set; }

    }
}