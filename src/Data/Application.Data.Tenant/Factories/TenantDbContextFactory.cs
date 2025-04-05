using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Serilog.Core;
using VistaLOS.Application.Common.Multitenancy;

namespace VistaLOS.Application.Data.Tenant.Factories;

public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
{
    private class TempUserContextAccessor : IUserContextAccessor
    {
        public UserContext? GetUserContext() => null;
    }

    public TenantDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
        optionsBuilder.UseSqlServer();

        return new TenantDbContext(optionsBuilder.Options, new TempUserContextAccessor(), Logger.None);
    }
}
