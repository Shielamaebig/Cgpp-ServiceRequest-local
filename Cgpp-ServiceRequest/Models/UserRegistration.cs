﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class UserRegistration
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        [Required]
        public string DepartmentName { get; set; }
        [Required]
        public string DivisionName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public bool IsNewAccount { get; set; }
        public string DateCreated { get; set; }
    }
}