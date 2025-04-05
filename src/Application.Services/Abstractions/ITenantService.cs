using VistaLOS.Application.Data.Master.Entities;

namespace VistaLOS.Application.Services.Abstractions;

public interface ITenantService
{
    Task CreateDemoTenantsAsync();
    Task<TenantEntity> InitializeTenantForScopeAsync(string tenantId);
}
