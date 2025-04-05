using Microsoft.EntityFrameworkCore;

namespace VistaLOS.Application.Jobs.Helpers;

public static class DatabaseHelper
{
    public static void EnsureDatabaseCreated(string connectionString)
    {
        var contextOptions = new DbContextOptionsBuilder<DbContext>()
            .UseSqlServer(connectionString)
            .Options;

        using var context = new DbContext(contextOptions);
        context.Database.EnsureCreated();
    }
}
