namespace VistaLOS.Application.Identity.Services.Models.Permissions;

public class PermissionModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
