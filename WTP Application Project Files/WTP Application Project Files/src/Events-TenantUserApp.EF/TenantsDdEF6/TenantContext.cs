using System.Data.Common;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace Events_TenantUserApp.EF.TenantsDdEF6
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.SqlClient;

    public partial class TenantContext : DbContext
    {
        public TenantContext()
            : base("name=TenantContext")
        {
        }

        public TenantContext(string connString)
          : base(connString)
        {
        }

        public virtual DbSet<EventsWithNoTicket> EventsWithNoTickets { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<database_firewall_rules>()
                .Property(e => e.start_ip_address)
                .IsUnicode(false);

            modelBuilder.Entity<database_firewall_rules>()
                .Property(e => e.end_ip_address)
                .IsUnicode(false);
        }
    }
}
