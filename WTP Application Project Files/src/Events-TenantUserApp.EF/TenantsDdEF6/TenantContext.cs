using System.Data.Common;

namespace Events_TenantUserApp.EF.TenantsDdEF6
{
    using System.Data.Entity;
    using System.Data.SqlClient;

    public partial class TenantContext : DbContext
    {
        public TenantContext()
            : base("name=TenantContext")
        {
        }

        public TenantContext(string connectionStr)
            : base(CreateDdrConnection(connectionStr), true)
        {

        }

        private static DbConnection CreateDdrConnection(string connectionStr)
        {
            // No initialization
            Database.SetInitializer<TenantContext>(null);

            // Ask shard map to broker a validated connection for the given key
            SqlConnection sqlConn = new SqlConnection(connectionStr);

            return sqlConn;
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
