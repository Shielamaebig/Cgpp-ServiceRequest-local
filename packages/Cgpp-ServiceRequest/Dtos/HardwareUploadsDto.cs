using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cgpp_ServiceRequest.Models;
using Cgpp_ServiceRequest.Dtos;

namespace Cgpp_ServiceRequest.Dtos
{
    public class HardwareUploadsDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }

        public string ImagePath { get; set; }
        public HardwareRequestDto HardwareRequest { get; set; }
        public int HardwareRequestId { get; set; }
        public string DateAdded { get; set; }
        public byte[] DocumentBlob { get; set; }
        public bool isSend { get; set; }
    }
}