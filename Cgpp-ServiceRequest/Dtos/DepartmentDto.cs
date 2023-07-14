﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Dtos
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DateAdded { get; set; }
        public FtpDto ftpDto { get; set; }
        public int FtpId { get; set; }

    }
}