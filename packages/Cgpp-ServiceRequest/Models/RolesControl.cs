using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cgpp_ServiceRequest.Models
{
    public class RolesControl
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
    public class UserRoleViewModel
    {
        public UserRoleViewModel()
        {
            this.UserRoles = new List<RolesControl>();
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RolesControl> UserRoles { get; set; }
    }
}
