namespace Cgpp_ServiceRequest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Draft
    {
        public int draftID { get; set; }

        [StringLength(15)]
        public string Sendto { get; set; }

        public string msg { get; set; }

        public int? tag { get; set; }

    }
}
