using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cgpp_ServiceRequest.Models;
using System.Data.Entity;
using Cgpp_ServiceRequest.ViewModels;
using Cgpp_ServiceRequest.Models.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;

namespace Cgpp_ServiceRequest.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationDbContext _db;

        public RolesController()
        {
            _db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }
        // GET: Roles
        public ActionResult Index()
        {
            var roles = _db.Roles.ToList();
            return View(roles);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            IdentityResult ir = new IdentityResult();
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
            ir = rm.Create(new IdentityRole(collection["RoleName"]));


            _db.LoginActivity.Add(new LoginActivity()
            {
                UserName = User.Identity.GetFullName(),
                ActivityMessage = "Added A new Role",
                ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                Email = User.Identity.GetUserName(),
                DepartmentName = User.Identity.GetDepartmentName(),
                DivisionName = User.Identity.GetDivisionName(),

            });
            _db.SaveChanges();

            if (ir.Succeeded)
            {
                return RedirectToAction("Index");
            }

            else
            {
                ViewBag.ErrorMessage = ir.Errors.ToString();
                return View();
            }
        }
        public ActionResult ListUsers()
        {
            var usersWithRoles = (from user in _db.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      DateCreated = user.DateCreated,
                                      Id = user.Id,
                                      FirstName = user.FirstName,
                                      MiddleName = user.MiddleName,
                                      LastName = user.LastName,
                                      MobileNumber = user.MobileNumber,
                                      Email = user.Email,
                                      Departments = user.Departments,
                                      Divisions = user.Divisions,
                                      ImagePath = user.ImagePath,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in _db.Roles on userRole.RoleId equals role.Id
                                                   select role.Name).ToList(),
                                  }).ToList().Select(p => new UsersInRoleViewModel()
                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      FirstName = p.FirstName,
                                      MiddleName = p.MiddleName,
                                      LastName = p.LastName,
                                      MobileNumber = p.MobileNumber,
                                      Divisions = p.Divisions,
                                      Departments = p.Departments,
                                      ImagePath = p.ImagePath,
                                      Role = string.Join(",", p.RoleNames),
                                      DateCreated = p.DateCreated,

                                  }).OrderByDescending(x=>x.DateCreated);

            return View(usersWithRoles);
        }


        [AllowAnonymous]

        public ActionResult ListUsers3()
        {
            var usersWithRoles = (from user in _db.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      DateCreated = user.DateCreated,
                                      Id = user.Id,
                                      FirstName = user.FirstName,
                                      MiddleName = user.MiddleName,
                                      LastName = user.LastName,
                                      MobileNumber = user.MobileNumber,
                                      Email = user.Email,
                                      Departments = user.Departments,
                                      Divisions = user.Divisions,
                                      ImagePath = user.ImagePath,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in _db.Roles on userRole.RoleId equals role.Id
                                                   select role.Name).ToList(),
                                  }).ToList().Select(p => new UsersInRoleViewModel()
                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      FirstName = p.FirstName,
                                      MiddleName = p.MiddleName,
                                      LastName = p.LastName,
                                      MobileNumber = p.MobileNumber,
                                      Divisions = p.Divisions,
                                      Departments = p.Departments,
                                      ImagePath = p.ImagePath,
                                      Role = string.Join(",", p.RoleNames),
                                      DateCreated = p.DateCreated,

                                  });

            return View(usersWithRoles);
        }
        public ActionResult ListUser2()
        {
            var usersWithRoles2 = (from user in _db.Users
                                   select new
                                   {
                                       UserId = user.Id,
                                       Username = user.UserName,
                                       DateCreated = user.DateCreated,
                                       Id = user.Id,
                                       FirstName = user.FirstName,
                                       MiddleName = user.MiddleName,
                                       LastName = user.LastName,
                                       MobileNumber = user.MobileNumber,
                                       Email = user.Email,
                                       Departments = user.Departments,
                                       Divisions = user.Divisions,
                                       ImagePath = user.ImagePath,
                                       RoleNames = (from userRole in user.Roles
                                                    join role in _db.Roles on userRole.RoleId equals role.Id
                                                    select role.Name).ToList(),
                                   }).ToList().Select(p => new UsersInRoleViewModel()
                                   {
                                       UserId = p.UserId,
                                       Username = p.Username,
                                       Email = p.Email,
                                       FirstName = p.FirstName,
                                       MiddleName = p.MiddleName,
                                       LastName = p.LastName,
                                       MobileNumber = p.MobileNumber,
                                       Divisions = p.Divisions,
                                       Departments = p.Departments,
                                       ImagePath = p.ImagePath,
                                       Role = string.Join(",", p.RoleNames),
                                       DateCreated = p.DateCreated,

                                   });

            return View(usersWithRoles2);
        }

        public ActionResult ManageUserRoles(FormCollection form)
        {
            UserRoleViewModel vm = new UserRoleViewModel();
            var UserName = form["UserName"];

            var user = _db.Users.Where(u => u.UserName == UserName.Trim()).FirstOrDefault();
            if (user != null)
            {
                IEnumerable<IdentityRole> roles = _db.Roles.ToList();

                vm.UserName = UserName;
                vm.UserId = user.Id;

                foreach (var item in roles.ToList())
                {
                    RolesControl roleAssigned = new RolesControl();
                    roleAssigned.Name = item.Name;
                    roleAssigned.Id = item.Id;
                    roleAssigned.IsSelected = false;

                    if (user.Roles != null)
                    {
                        var roleId = user.Roles.Select(r => r.RoleId).ToList();
                        if (roleId.Contains(item.Id))
                        {
                            roleAssigned.IsSelected = true;
                        }
                        else
                        {
                            roleAssigned.IsSelected = false;
                        }
                    }
                    vm.UserRoles.Add(roleAssigned);
                }

                _db.LoginActivity.Add(new LoginActivity()
                {
                    UserName = User.Identity.GetFullName(),
                    ActivityMessage = "Assigned a new Role to a User",
                    ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                    Email = User.Identity.GetUserName(),
                    DepartmentName = User.Identity.GetDepartmentName(),
                    DivisionName = User.Identity.GetDivisionName(),
                });
                _db.SaveChanges();

            }
            else
            {
                vm = null;
            }
            return View(vm);
        }

        public ActionResult UpdateRoles(UserRoleViewModel updateRoles)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            var userId = updateRoles.UserId;

            foreach (var item in updateRoles.UserRoles)
            {
                var role = _db.Roles.FirstOrDefault(r => r.Id == item.Id);
                if (item.IsSelected)
                {
                    if (!userManager.IsInRole(userId, item.Id))
                    {
                        userManager.AddToRole(userId, role.Name);
                    }
                }
                else if (userManager.IsInRole(userId, role.Name))
                {
                    userManager.RemoveFromRole(userId, role.Name);
                }
            }

            return RedirectToAction("ListUsers", "Roles");
        }

        public ActionResult UpdateRoles2(UserRoleViewModel updateRoles)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            var userId = updateRoles.UserId;

            foreach (var item in updateRoles.UserRoles)
            {
                var role = _db.Roles.FirstOrDefault(r => r.Id == item.Id);
                if (item.IsSelected)
                {
                    if (!userManager.IsInRole(userId, item.Id))
                    {
                        userManager.AddToRole(userId, role.Name);
                    }
                }
                else if (userManager.IsInRole(userId, role.Name))
                {
                    userManager.RemoveFromRole(userId, role.Name);
                }
            }

            return RedirectToAction("userAccounts", "Roles");
        }

        public ActionResult UserProfile()
        {
            // var pos = ctx.Users.Include(u => u.Roles).Include(u => u.Departments).Include(u => u.Divisions).ToList();
            var posId = User.Identity.GetAspUserId();
            // pos = new List<ApplicationUser>(pos.Where(u => u.Email == posId));
            // return View(pos);
            var usersWithRoles = (from user in _db.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      DateCreated = user.DateCreated,
                                      DivisionsId = user.DivisionsId,
                                      DepartmentsId = user.DepartmentsId,
                                      Departments = user.Departments,
                                      Divisions = user.Divisions,
                                      Id = user.Id,
                                      FirstName = user.FirstName,
                                      MiddleName = user.MiddleName,
                                      LastName = user.LastName,
                                      MobileNumber = user.MobileNumber,
                                      Email = user.Email,
                                      ImagePath = user.ImagePath,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in _db.Roles on userRole.RoleId
                                                       equals role.Id
                                                   select role.Name).ToList(),

                                  }).ToList().Select(p => new UsersInRoleViewModel()

                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Role = string.Join(",", p.RoleNames),
                                      DateCreated = p.DateCreated,
                                      DivisionsId = p.DivisionsId,
                                      DepartmentsId = p.DepartmentsId,
                                      FirstName = p.FirstName,
                                      MiddleName = p.MiddleName,
                                      LastName = p.LastName,
                                      MobileNumber = p.MobileNumber,
                                      Divisions = p.Divisions,
                                      Departments = p.Departments,
                                      ImagePath = p.ImagePath
                                  });
            usersWithRoles = new List<UsersInRoleViewModel>(usersWithRoles.Where(u => u.UserId == posId));
            if (User.IsInRole("DepartmentHead"))
            {
                var useId = Convert.ToInt32(User.Identity.GetUserDepartment());
                usersWithRoles = new List<UsersInRoleViewModel>(usersWithRoles.Where(u => u.DepartmentsId == useId));
            }
            return View(usersWithRoles);
        }


        public ActionResult UserLogin()
        {
            var userlogin = (from user in _db.Users
                             select new
                             {
                                 UserId = user.Id,
                                 Username = user.UserName,
                                 DateCreated = user.DateCreated,
                                 DivisionsId = user.DivisionsId,
                                 DepartmentsId = user.DepartmentsId,
                                 Departments = user.Departments,
                                 Divisions = user.Divisions,
                                 Id = user.Id,
                                 FulllName = user.FullName,
                                 FirstName = user.FirstName,
                                 MiddleName = user.MiddleName,
                                 LastName = user.LastName,
                                 MobileNumber = user.MobileNumber,
                                 Email = user.Email,
                                 ImagePath = user.ImagePath,
                                 IsLogged = user.IsLogged,
                                 RoleNames = (from userRole in user.Roles
                                              join role in _db.Roles on userRole.RoleId
                                                  equals role.Id
                                              select role.Name).ToList(),

                             }).ToList().Select(p => new UsersInRoleViewModel()
                             {
                                 UserId = p.UserId,
                                 Username = p.Username,
                                 FullName = p.FulllName,
                                 Email = p.Email,
                                 Role = string.Join(",", p.RoleNames),
                                 DateCreated = p.DateCreated,
                                 DivisionsId = p.DivisionsId,
                                 DepartmentsId = p.DepartmentsId,
                                 FirstName = p.FirstName,
                                 MiddleName = p.MiddleName,
                                 LastName = p.LastName,
                                 MobileNumber = p.MobileNumber,
                                 Divisions = p.Divisions,
                                 Departments = p.Departments,
                                 ImagePath = p.ImagePath,
                                 Islogged = p.IsLogged,
                             });
            return View(userlogin);
        }

        public ActionResult userAccounts()
        {
            var usersWithRoles = (from user in _db.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      DateCreated = user.DateCreated,
                                      Id = user.Id,
                                      FirstName = user.FirstName,
                                      MiddleName = user.MiddleName,
                                      LastName = user.LastName,
                                      MobileNumber = user.MobileNumber,
                                      Email = user.Email,
                                      Departments = user.Departments,
                                      Divisions = user.Divisions,
                                      ImagePath = user.ImagePath,
                                      IsUserApproved = user.IsUserApproved,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in _db.Roles on userRole.RoleId equals role.Id
                                                   select role.Name).ToList(),
                                  }).ToList().Select(p => new UsersInRoleViewModel()
                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      FirstName = p.FirstName,
                                      MiddleName = p.MiddleName,
                                      LastName = p.LastName,
                                      MobileNumber = p.MobileNumber,
                                      Divisions = p.Divisions,
                                      Departments = p.Departments,
                                      ImagePath = p.ImagePath,
                                      IsUserApproved = p.IsUserApproved,
                                      Role = string.Join(",", p.RoleNames),
                                      DateCreated = p.DateCreated,

                                  });

            return View(usersWithRoles.OrderByDescending(x => x.DateCreated));
        }
        [AllowAnonymous]
        public ActionResult DefaultRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DefaultRole(FormCollection collection)
        {
            IdentityResult ir = new IdentityResult();
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
            ir = rm.Create(new IdentityRole(collection["RoleName"]));

            _db.SaveChanges();

            if (ir.Succeeded)
            {
                return RedirectToAction("ListUsers3", "Roles");
            }

            else
            {
                ViewBag.ErrorMessage = ir.Errors.ToString();
                return View();
            }
        }

        public ActionResult ManageUserRoles3(FormCollection form)
        {
            UserRoleViewModel vm = new UserRoleViewModel();
            var UserName = form["UserName"];

            var user = _db.Users.Where(u => u.UserName == UserName.Trim()).FirstOrDefault();
            if (user != null)
            {
                IEnumerable<IdentityRole> roles = _db.Roles.ToList();

                vm.UserName = UserName;
                vm.UserId = user.Id;

                foreach (var item in roles.ToList())
                {
                    RolesControl roleAssigned = new RolesControl();
                    roleAssigned.Name = item.Name;
                    roleAssigned.Id = item.Id;
                    roleAssigned.IsSelected = false;

                    if (user.Roles != null)
                    {
                        var roleId = user.Roles.Select(r => r.RoleId).ToList();
                        if (roleId.Contains(item.Id))
                        {
                            roleAssigned.IsSelected = true;
                        }
                        else
                        {
                            roleAssigned.IsSelected = false;
                        }
                    }
                    vm.UserRoles.Add(roleAssigned);
                }

                _db.LoginActivity.Add(new LoginActivity()
                {
                    UserName = User.Identity.GetFullName(),
                    ActivityMessage = "Assigned a new Role to a User",
                    ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                    Email = User.Identity.GetUserName(),
                    DepartmentName = User.Identity.GetDepartmentName(),
                    DivisionName = User.Identity.GetDivisionName(),
                });
                _db.SaveChanges();

            }
            else
            {
                vm = null;
            }
            return View(vm);
        }

        [AllowAnonymous]
        public ActionResult ManageUserRoles2(FormCollection form)
        {
            UserRoleViewModel vm = new UserRoleViewModel();
            var UserName = form["UserName"];

            var user = _db.Users.Where(u => u.UserName == UserName.Trim()).FirstOrDefault();
            if (user != null)
            {
                IEnumerable<IdentityRole> roles = _db.Roles.ToList();

                vm.UserName = UserName;
                vm.UserId = user.Id;

                foreach (var item in roles.ToList())
                {
                    RolesControl roleAssigned = new RolesControl();
                    roleAssigned.Name = item.Name;
                    roleAssigned.Id = item.Id;
                    roleAssigned.IsSelected = false;

                    if (user.Roles != null)
                    {
                        var roleId = user.Roles.Select(r => r.RoleId).ToList();
                        if (roleId.Contains(item.Id))
                        {
                            roleAssigned.IsSelected = true;
                        }
                        else
                        {
                            roleAssigned.IsSelected = false;
                        }
                    }
                    vm.UserRoles.Add(roleAssigned);
                }

                _db.LoginActivity.Add(new LoginActivity()
                {
                    UserName = User.Identity.GetFullName(),
                    ActivityMessage = "Assigned a new Role to a User",
                    ActivityDate = DateTime.Now.ToString("MMMM dd yyyy hh:mm tt"),
                    Email = User.Identity.GetUserName(),
                });
                _db.SaveChanges();

            }
            else
            {
                vm = null;
            }
            return View(vm);
        }

        public ActionResult DivisionUser()
        {
            return View();
        }
        public ActionResult DepartmentUsers()
        {
            return View();
        }
    }

}