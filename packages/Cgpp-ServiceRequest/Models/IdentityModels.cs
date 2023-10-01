using System;
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
       public DateTime? DateCreated { get; set; }
       public string MobileNumber { get; set; }
       public int? DepartmentsId { get; set; }
       public Departments Departments { get; set; }
       //public string ImgPath { get; set; }                    
       public string ImagePath { get; set; }
       public int? DivisionsId { get; set; }
       public Divisions Divisions { get; set; }
       public bool IsLogged { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            userIdentity.AddClaim(new Claim("Id", this.Id));
            userIdentity.AddClaim(new Claim("FullName", this.FullName));
            userIdentity.AddClaim(new Claim("Email", this.Email));
            userIdentity.AddClaim(new Claim("MobileNumber", this.MobileNumber));
            userIdentity.AddClaim(new Claim("ImagePath", this.ImagePath));
            userIdentity.AddClaim(new Claim("DepartmentsId", this.DepartmentsId.ToString()));
            userIdentity.AddClaim(new Claim("DivisionsId", this.DivisionsId.ToString()));

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
        public DbSet<SoftwareRequest> SoftwareRequest { get; set; }
        public DbSet<HardwareRequest> HardwareRequest { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Finding> Finding { get; set; }
        public DbSet<InformationSystem> InformationSystem { get; set; }
        public DbSet<RequestHistory> RequestHistory { get; set; }
        public DbSet<SoftwareRequestHistory> SoftwareRequestHistory { get; set; }
        public DbSet<HardwareRequestHistory> HardwareRequestHistoty { get; set; }
        public DbSet<HardwareUploads> HardwareUploads { get; set; }
        public DbSet<SoftwareUploads> SoftwareUploads { get; set; }
        public DbSet<FullCalendar> FullCalendars { get; set; }
        public DbSet<MaintenanceMode> MaintenanceMode { get; set; }
        public DbSet<FtpModel> FtpModels { get; set; }
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