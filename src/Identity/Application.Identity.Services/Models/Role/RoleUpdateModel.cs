namespace VistaLOS.Application.Identity.Services.Models.Role;

public class RoleUpdateModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
