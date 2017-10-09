using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Events_Tenant.Common.Interfaces;

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
        /// Converts the int key to bytes array.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public byte[] ConvertIntKeyToBytesArray(int key)
        {
            byte[] normalized = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(key));

            // Maps Int32.Min - Int32.Max to UInt32.Min - UInt32.Max.
            normalized[0] ^= 0x80;

            return normalized;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets all tenant names from tenant server
        /// </summary>
        /// <param name="tenantServerConfig">The tenant server configuration.</param>
        /// <param name="databaseConfig">The database configuration.</param>
        /// <returns></returns>
        private List<string> GetAllTenantNames(TenantServerConfig tenantServerConfig, DatabaseConfig databaseConfig)
        {
            List<string> list = new List<string>();

            string conString = $"Server={databaseConfig.SqlProtocol}:{tenantServerConfig.TenantServer},{databaseConfig.DatabaseServerPort};Database={""};User ID={databaseConfig.DatabaseUser};Password={databaseConfig.DatabasePassword};Trusted_Connection=False;Encrypt=True;Connection Timeout={databaseConfig.ConnectionTimeOut};";

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases WHERE name NOT IN ('master')", con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(dr[0].ToString());
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Generates the tenant Id using MD5 Hashing.
        /// </summary>
        /// <param name="tenantName">Name of the tenant.</param>
        /// <returns></returns>
        private int GetTenantKey(string tenantName)
        {
            var normalizedTenantName = tenantName.Replace(" ", string.Empty).ToLower();

            //Produce utf8 encoding of tenant name 
            var tenantNameBytes = Encoding.UTF8.GetBytes(normalizedTenantName);

            //Produce the md5 hash which reduces the size
            MD5 md5 = MD5.Create();
            var tenantHashBytes = md5.ComputeHash(tenantNameBytes);

            //Convert to integer for use as the key
            int tenantKey = BitConverter.ToInt32(tenantHashBytes, 0);

            return tenantKey;
        }
        #endregion
    }
}
