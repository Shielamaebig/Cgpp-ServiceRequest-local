using Cgpp_ServiceRequest.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class DraftsDto
    {
        public SoftwareTechnician SoftwareTechnician { get; set; }
        public int SoftwareTechnicianId { get; set; }
        public int draftID { get; set; }
        public string Sendto { get; set; }
        public string msg { get; set; }
        public Nullable<int> tag { get; set; }

        public List<string> PhoneNumber { get; set; }

    }
}