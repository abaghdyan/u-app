using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Serilog.Core;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Master.Options;

namespace VistaLOS.Application.Data.Master.Factories;

public class MasterDbContextFactory : IDesignTimeDbContextFactory<MasterDbContext>
{
    private class TempUserContextAccessor : IUserContextAccessor
    {
        public UserContext? GetUserContext() => null;
    }

    public MasterDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MasterDbContext>();
        optionsBuilder.UseSqlServer();

        return new MasterDbContext(optionsBuilder.Options,
            new TempUserContextAccessor(),
            Logger.None,
            new MasterDbOptions());
    }
}
