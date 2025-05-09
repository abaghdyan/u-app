using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Master.Infrastructure.Configurations;

namespace VistaLOS.Application.Data.Master;

public partial class MasterDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var schemaName = GetSchemaName(this._dbOptions);

        modelBuilder.HasDefaultSchema(schemaName);

        // Setting global query filter for TenantId property
        Expression<Func<IHasTenantId, bool>> filterExpr = e => e.TenantId == UserContextAccessor.GetRequiredUserContext().TenantId;
        foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes()) {
            if (mutableEntityType.ClrType.IsAssignableTo(typeof(IHasTenantId))) {
                var parameter = Expression.Parameter(mutableEntityType.ClrType);
                var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);
                mutableEntityType.SetQueryFilter(lambdaExpression);
            }
        }

        modelBuilder.ApplyConfiguration(new InvoiceConfiguration(schemaName));
        modelBuilder.ApplyConfiguration(new PermissionConfiguration(schemaName));
        modelBuilder.ApplyConfiguration(new TenantConfiguration(schemaName));
        modelBuilder.ApplyConfiguration(new TenantStorageConfiguration(schemaName));
        modelBuilder.ApplyConfiguration(new TenantGroupConfiguration(schemaName));

        base.OnModelCreating(modelBuilder);
    }
}