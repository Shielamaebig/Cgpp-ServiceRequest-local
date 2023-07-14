using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class HardwareUserUploadsDto
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public HardwareUserRequest HardwareUserRequest { get; set; }
        public int HardwareUserRequestId { get; set; }
        public string DateAdded { get; set; }
        public string FileExtension { get; set; }
        public byte[] DocumentBlob { get; set; }

    }
}