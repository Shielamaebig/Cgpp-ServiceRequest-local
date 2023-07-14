using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Cgpp_ServiceRequest.Models;

namespace Cgpp_ServiceRequest.ViewModels
{
    public class UsersInRoleViewModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Role { get; set; }
        public bool Islogged { get; set; }
        public int? DepartmentsId { get; set; }
        public Departments Departments { get; set; }
        public int? DivisionsId { get; set; }
        public Divisions Divisions { get; set; }
        public string ImagePath { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public bool IsUserApproved { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionName { get; set; }
    }
}