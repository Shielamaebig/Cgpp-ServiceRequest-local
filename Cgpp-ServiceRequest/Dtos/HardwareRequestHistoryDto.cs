﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.Dtos
{
    public class HardwareRequestHistoryDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? DepartmentsId { get; set; }
        public int? DivisionsId { get; set; }
        public Departments Departments { get; set; }
        public Divisions Divisions { get; set; }
        public string RequetMessage { get; set; }
        public string RequetDate { get; set; }
    }
}