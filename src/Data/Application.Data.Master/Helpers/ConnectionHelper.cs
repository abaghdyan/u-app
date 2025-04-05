using Microsoft.Data.SqlClient;
using VistaLOS.Application.Common.Helpers;
using VistaLOS.Application.Data.Common.Helpers;
using VistaLOS.Application.Data.Master.Entities;
using VistaLOS.Application.Data.Master.Models;

namespace VistaLOS.Application.Data.Master.Helpers;

public static class ConnectionHelper
{
    public static SqlConnectionStringBuilder GetConnectionBuilder(string key, TenantStorageEntity tenantStorage)
    {
        var dataSource = SecurityHelper.Decrypt(key, tenantStorage.Server);
        var initialCatalog = SecurityHelper.Decrypt(key, tenantStorage.Database);
        var userId = string.IsNullOrWhiteSpace(tenantStorage.Username) ? "" : SecurityHelper.Decrypt(key, tenantStorage.Username);
        var password = string.IsNullOrWhiteSpace(tenantStorage.Password) ? "" : SecurityHelper.Decrypt(key, tenantStorage.Password);

        var connectionBuilder = new SqlConnectionStringBuilder {
            DataSource = dataSource,
            InitialCatalog = initialCatalog,
            UserID = userId,
            Password = password
        };

        if (tenantStorage.ConnectionParameters != null) {
            var parameters = JsonHelper.DeserializeObject<TenantStorageConnectionParameter>(tenantStorage.ConnectionParameters);
            if (parameters == null) {
                throw new ArgumentNullException($"{nameof(TenantStorageConnectionParameter)} was in a wrong format");
            }
            
            connectionBuilder.PersistSecurityInfo = parameters.PersistSecurityInfo ?? connectionBuilder.PersistSecurityInfo;
            connectionBuilder.MultipleActiveResultSets = parameters.MultipleActiveResultSets ?? connectionBuilder.MultipleActiveResultSets;
            connectionBuilder.Encrypt = parameters.Encrypt ?? connectionBuilder.Encrypt;
            connectionBuilder.TrustServerCertificate = parameters.TrustServerCertificate ?? connectionBuilder.TrustServerCertificate;
            connectionBuilder.ConnectTimeout = parameters.ConnectTimeout ?? connectionBuilder.ConnectTimeout;
            connectionBuilder.LoadBalanceTimeout = parameters.LoadBalanceTimeout ?? connectionBuilder.LoadBalanceTimeout;
            connectionBuilder.Pooling = parameters.Pooling ?? connectionBuilder.Pooling;
            connectionBuilder.MinPoolSize = parameters.MinPoolSize ?? connectionBuilder.MinPoolSize;
            connectionBuilder.MaxPoolSize = parameters.MaxPoolSize ?? connectionBuilder.MaxPoolSize;
        }

        return connectionBuilder;
    }
}
