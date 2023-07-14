using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Cgpp_ServiceRequest.Models.Extensions
{
    public static class IdentityExtension
    {
        public static string GetAspUserId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Id");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetUserEmail(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Email");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetFullName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FullName");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetUserDepartment(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("DepartmentsId");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetUserDivision(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("DivisionsId");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetUserMobileNumber(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("MobileNumber");
            return (claim !=null) ? claim.Value : string.Empty;
        }
        public static string GetImgFile(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ImagePath");
            return (claim != null) ? claim.Value : string.Empty;

        }
        public static string GetDepartmentName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("DepartmentName");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetDivisionName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("DivisionName");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetLogEmail(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("LogEmail");
            return (claim != null) ? claim.Value: string.Empty;
        }
        public static string GetFirstName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FirstName");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetMiddleName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("MiddleName");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetLastName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("LastName");
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}