using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class SoftwareUploadsDto
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public SoftwareRequestDto SoftwareRequest { get; set; }
        public int SoftwareRequestId { get; set; }
         public string DateAdded { get; set; }
        public string FileExtension { get; set; }
 
        public byte[] DocumentBlob { get; set; }

    }
}