using Cgpp_ServiceRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class SoftwareUserUploadsDto
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
        public SoftwareUserRequest SoftwareUserRequest { get; set; }
        public int SoftwareUserRequestId { get; set; }
        public string DateAdded { get; set; }
       public byte[] DocumentBlob { get; set; }

    }
}