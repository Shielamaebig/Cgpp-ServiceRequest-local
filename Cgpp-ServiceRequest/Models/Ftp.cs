using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class Ftp
    {
        public int Id { get; set; }
        public string FtpHost { get; set; }
        public string FtpPort { get; set; }
        public string FtpUsername { get; set; }
        public string FtpPassword { get; set; }
        public string DateAdded { get; set; }
        public string FolderName { get; set; }
        public string Label { get; set; }
    }
}