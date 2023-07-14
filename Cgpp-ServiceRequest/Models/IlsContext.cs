using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Cgpp_ServiceRequest.Models
{
    public partial class IlsContext : DbContext
    {
        public IlsContext()
            : base("name=IlsContext")
        {
        }

        public virtual DbSet<Draft> Drafts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
