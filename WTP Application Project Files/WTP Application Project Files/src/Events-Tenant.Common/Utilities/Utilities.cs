using Events_Tenant.Common.Interfaces;
using Events_TenantUserApp.EF.TenantsDdEF6;

namespace Events_Tenant.Common.Utilities
{

    /// <summary>
    /// The Utilities class for doing common methods
    /// </summary>
    /// <seealso cref="Events_Tenant.Common.Interfaces.IUtilities" />
    public class Utilities : IUtilities
    {
        #region Public methods

        /// <summary>
        /// This method will ensure that the stored proc Reset Event Dates is invoked which will reset
        /// </summary>
        /// <param name="connString">The connection string to be passed to EF6</param>
        public void ResetEventDates(string connString)
        {
            #region EF6
            //use EF6 since execution of Stored Procedure in EF Core for anonymous return type is not supported yet
            using (var context = new TenantContext(connString))
            {
                context.Database.ExecuteSqlCommand("sp_ResetEventDates");
            }
            #endregion
        }

        #endregion
    }
}
