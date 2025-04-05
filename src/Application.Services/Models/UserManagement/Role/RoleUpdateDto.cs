namespace VistaLOS.Application.Services.Models.UserManagement.Role;

public class RoleUpdateDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
