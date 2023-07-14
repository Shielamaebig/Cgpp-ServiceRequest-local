using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Cgpp_ServiceRequest.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]

        public string MiddleName { get; set; }
        [Required]

        public string LastName { get; set; }
        public DateTime? DateCreated { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public int? DepartmentsId { get; set; }
        public Departments Departments { get; set; }
       //public string ImgPath { get; set; }                    
        public string ImagePath { get; set; }
        public int? DivisionsId { get; set; }
        public Divisions Divisions { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionName { get; set; }
        public bool IsLogged { get; set; }
        public bool IsUserApproved { get; set; }
        public string LogEmail { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            userIdentity.AddClaim(new Claim("Id", this.Id));
            userIdentity.AddClaim(new Claim("FullName", this.FullName));
            userIdentity.AddClaim(new Claim("FirstName", this.FirstName));
            userIdentity.AddClaim(new Claim("MiddleName", this.MiddleName));
            userIdentity.AddClaim(new Claim("LastName", this.LastName));
            userIdentity.AddClaim(new Claim("Email", this.Email));
            userIdentity.AddClaim(new Claim("MobileNumber", this.MobileNumber));
            userIdentity.AddClaim(new Claim("ImagePath", this.ImagePath));
            userIdentity.AddClaim(new Claim("DepartmentName", this.DepartmentName));
            userIdentity.AddClaim(new Claim("DivisionName", this.DivisionName));
            userIdentity.AddClaim(new Claim("DepartmentsId", this.DepartmentsId.ToString()));
            userIdentity.AddClaim(new Claim("DivisionsId", this.DivisionsId.ToString()));
            userIdentity.AddClaim(new Claim("LogEmail", this.LogEmail));
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Divisions> Divisions { get; set; }
        public DbSet<LoginActivity> LoginActivity { get; set; }
        public DbSet<UnitType> UnitType { get; set; }
        public DbSet<Software> Software { get; set; }
        public DbSet<Hardware> Hardware { get; set; }
        public DbSet<SoftwareTechnician> SoftwareTechnician { get; set; }
        public DbSet<HardwareTechnician> HardwareTechnician { get; set; }
        public DbSet<Finding> Finding { get; set; }
        public DbSet<InformationSystem> InformationSystem { get; set; }
        public DbSet<RequestHistory> RequestHistory { get; set; }
        public DbSet<FullCalendar> FullCalendars { get; set; }
        public DbSet<MaintenanceMode> MaintenanceMode { get; set; }
        public DbSet<UserRegistration> UserRegistrations { get; set; }
        public DbSet<UserForgotPassword> UserForgotPasswords { get; set; }
        public DbSet<HardwareUserRequest> HardwareUserRequests { get; set; }
        public DbSet<TechnicianReport> TechnicianReports { get; set; }
        public DbSet<HardwareApproval> HardwareApprovals { get; set; }
        public DbSet<HardwareTask> HardwareTasks { get; set; }
        public DbSet<HardwareUserUploads> HardwareUserUploads { get; set; }
        public DbSet<TechnicianUploads> TechnicianUploads { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<HardwareVerify> HardwareVerifies { get; set; }
        public DbSet<SoftwareUserRequest> SoftwareUserRequests { get; set; }
        public DbSet<SoftwareAcceptsRequest> SoftwareAcceptsRequests { get; set; }
        public DbSet<ProgrammerReport> ProgrammerReport { get; set; }
        public DbSet<SoftwareTask> SoftwareTasks { get; set; }
        public DbSet<SoftwareTaskList> SoftwareTaskLists { get; set; }
        public DbSet<SoftwareUserUploads> SoftwareUserUploads { get; set; }
        public DbSet<ProgrammerUploads> ProgrammerUploads { get; set; }
        public DbSet<SoftwareVerification> SoftwareVerification { get; set; }
        public DbSet<SoftwareApproved> SoftwareApproveds { get; set; }
        public DbSet<HardwareTasksList> HardwareTasksLists { get; set; }
        public DbSet<Ftp> Ftp { get; set; }
        public ApplicationDbContext()
            : base("CGPPServiceRequest", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}