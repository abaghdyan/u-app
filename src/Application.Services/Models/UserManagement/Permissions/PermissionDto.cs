namespace VistaLOS.Application.Services.Models.UserManagement.Permissions;

public class PermissionDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
