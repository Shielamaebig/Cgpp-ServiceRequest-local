using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class ProgrammerUploadsDto
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public ProgrammerReport ProgrammerReport { get; set; }
        public int ProgrammerReportId { get; set; }
        public string DateAdded { get; set; }
        public byte[] DocumentBlob { get; set; }
        public string RemarksUploads { get; set; }
    }
}