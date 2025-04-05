using Application.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VistaLOS.Application.Common.Constants;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Common.Helpers;
using VistaLOS.Application.Data.Common.Utilities;
using VistaLOS.Application.Data.Master;
using VistaLOS.Application.Data.Master.Constants;
using VistaLOS.Application.Data.Master.Entities;
using VistaLOS.Application.Data.Master.Helpers;
using VistaLOS.Application.Data.Master.Options;
using VistaLOS.Application.Data.Master.Repositories;
using VistaLOS.Application.Data.Tenant;
using VistaLOS.Application.Identity.Data.Constants;
using VistaLOS.Application.Identity.Services.Managers;
using VistaLOS.Application.Identity.Services.Models.User;
using VistaLOS.Application.Services.Abstractions;

namespace VistaLOS.Application.Services.Impl;

public class TenantService : ITenantService
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IUserContextWriter _userContextWriter;
    private readonly MasterDbOptions _options;
    private readonly MasterDbContext _masterDbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITenantRepository _tenantRepository;
    private readonly IdentityUserManager _identityUserManager;
    private readonly ITransactionManager<MasterDbContext> _masterTransactionManager;
    private readonly ITransactionManager<IdentityDbContext> _identityTransactionManager;

    public TenantService(IUserContextWriter userContextWriter,
        MasterDbContext masterDbContext,
        IServiceProvider serviceProvider,
        MasterDbOptions options,
        ITenantRepository tenantRepository,
        IdentityUserManager identityUserManager,
        ITransactionManager<MasterDbContext> masterTransactionManager,
        ITransactionManager<IdentityDbContext> identityTransactionManager,
        IHostEnvironment hostingEnvironment)
    {
        _options = options;
        _userContextWriter = userContextWriter;
        _masterDbContext = masterDbContext;
        _serviceProvider = serviceProvider;
        _tenantRepository = tenantRepository;
        _identityUserManager = identityUserManager;
        _masterTransactionManager = masterTransactionManager;
        _identityTransactionManager = identityTransactionManager;
        _hostEnvironment = hostingEnvironment;
    }

    public async Task CreateDemoTenantsAsync()
    {
        if (!_hostEnvironment.IsEnvironment(VistaEnvironments.Local)) {
            throw new InvalidOperationException("Unable to create demo tenants. Wrong environment.");
        }

        await _masterTransactionManager.ExecuteInTransactionAsync(async () => {
            await _identityTransactionManager.ExecuteInTransactionAsync(async () => {

                var defaultTenantGroup = new TenantGroupEntity {
                    Name = TenantGroups.Default.ToString()
                };

                var connectionParams = "{\"MaxPoolSize\": \"100\",\r\n \"MinPoolSize\": \"1\",\r\n \"Pooling\": \"True\",\r\n \"LoadBalanceTimeout\": \"30\",\r\n \"ConnectTimeout\": \"30\",\r\n \"TrustServerCertificate\": \"False\",\r\n \"Encrypt\": \"False\",\r\n \"MultipleActiveResultSets\": \"False\",\r\n \"PersistSecurityInfo\": \"False\"}";

                var firstStorage = new TenantStorageEntity {
                    StorageName = "FirstStorage",
                    Database = SecurityHelper.Encrypt(_options.EncryptionKey, "Local-Tenant-Storage-1"),
                    Server = SecurityHelper.Encrypt(_options.EncryptionKey, "(localdb)\\MSSQLLocalDB"),
                    Username = "",
                    Password = "",
                    Location = "WUS",
                    Status = TenantStorageStatus.Active,
                    Type = TenantStorageType.Shared,
                    ConnectionParameters = connectionParams
                };

                var secondStorage = new TenantStorageEntity {
                    StorageName = "SecondStorage",
                    Database = SecurityHelper.Encrypt(_options.EncryptionKey, "Local-Tenant-Storage-2"),
                    Server = SecurityHelper.Encrypt(_options.EncryptionKey, "(localdb)\\MSSQLLocalDB"),
                    Username = "",
                    Password = "",
                    Location = "WUS",
                    Status = TenantStorageStatus.Active,
                    Type = TenantStorageType.Shared,
                    ConnectionParameters = connectionParams
                };

                var tenants = new List<TenantEntity>() {
                    new TenantEntity {
                        Name = "tenant1",
                        CompanyName = "tenant1",
                        TenantStorage = firstStorage,
                        Status = TenantStatus.Active,
                        Email = "tenant1@gmail.com",
                        TenantGroup = defaultTenantGroup
                    },
                    new TenantEntity {
                        Name = "tenant2",
                        CompanyName = "tenant2",
                        TenantStorage = secondStorage,
                        Status = TenantStatus.Active,
                        Email = "tenant2@gmail.com",
                        TenantGroup = defaultTenantGroup
                    },
                    new TenantEntity {
                        Name = "tenant3",
                        CompanyName = "tenant3",
                        TenantStorage = secondStorage,
                        Status = TenantStatus.Active,
                        Email = "tenant3@gmail.com",
                        TenantGroup = defaultTenantGroup
                    }
                };

                await _tenantRepository.CreateRangeAsync(tenants);

                foreach (var tenant in tenants) {
                    var owner = new UserCreateModel {
                        FirstName = $"Owner_{tenant.Name}",
                        LastName = $"Owner_{tenant.Name}",
                        Email = $"owner@{tenant.Name}.com",
                        Password = "@Aa123456",
                        UserName = $"owner_{tenant.Name}",
                        RoleId = (long)DefaultRoles.Owner,
                        TenantId = tenant.Id,
                    };

                    var admin = new UserCreateModel {
                        FirstName = $"Admin_{tenant.Name}",
                        LastName = $"Admin_{tenant.Name}",
                        Email = $"admin@{tenant.Name}.com",
                        Password = "@Aa123456",
                        UserName = $"admin_{tenant.Name}",
                        RoleId = (long)DefaultRoles.Admin,
                        TenantId = tenant.Id,
                    };

                    await _identityUserManager.CreateManyUsersAsync(new List<UserCreateModel> { owner, admin });

                    var scope = _serviceProvider.CreateScope();
                    var userContextWriter = scope.ServiceProvider.GetRequiredService<IUserContextWriter>();
                    var connectionBuilder = ConnectionHelper.GetConnectionBuilder(_options.EncryptionKey, tenant.TenantStorage);
                    var userContext = UserContext.Create(tenant.Id, default, default, connectionBuilder.ToString());
                    userContextWriter.SetUserContext(userContext);

                    var newTenantDbContext = scope.ServiceProvider.GetRequiredService<TenantDbContext>();
                    await newTenantDbContext.Database.MigrateAsync();
                    await newTenantDbContext.SaveChangesAsync();
                }
            });
        });
    }

    public async Task<TenantEntity> InitializeTenantForScopeAsync(string tenantId)
    {
        var tenant = await _masterDbContext.Tenants
            .Include(t => t.TenantStorage)
            .Where(t => t.Id == tenantId)
            .FirstOrDefaultAsync();

        if (tenant == null) {
            throw new ArgumentNullException($"Tenant with {tenantId} Id was not found.");
        }

        var connectionBuilder = ConnectionHelper.GetConnectionBuilder(_options.EncryptionKey, tenant.TenantStorage);
        var userContext = UserContext.Create(tenantId, default, default, connectionBuilder.ToString());
        _userContextWriter.SetUserContext(userContext);

        return tenant;
    }
}
