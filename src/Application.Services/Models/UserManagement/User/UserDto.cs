using VistaLOS.Application.Data.Master;
using VistaLOS.Application.Identity.Services.Models.Role;

namespace VistaLOS.Application.Services.Models.UserManagement.User;

public class UserDto
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string Email { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
    public bool ChangePasswordRequired { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string TenantId { get; set; } = null!;
    public UserStatus Status { get; set; }
    public RoleModel Role { get; set; } = new();
}
