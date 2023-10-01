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
    }
}