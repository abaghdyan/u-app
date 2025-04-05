namespace VistaLOS.Application.Common.Multitenancy;

public class UserContext
{
    public string TenantId { get; private set; } = null!;
    public string ConnectionString { get; private set; } = null!;
    public long UserId { get; private set; }
    public string? InitialUserName { get; private set; }
    public List<long> PermissionIds { get; private set; } = new();

    private UserContext()
    {
    }

    public static UserContext Create(string tenantId,
        long userId,
        List<long> permissionIds,
        string connectionString)
    {
        return new UserContext() {
            TenantId = tenantId,
            UserId = userId,
            PermissionIds = permissionIds,
            ConnectionString = connectionString,
        };
    }
}
