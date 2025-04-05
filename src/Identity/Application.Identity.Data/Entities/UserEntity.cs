using VistaLOS.Application.Common.Interfaces;
using VistaLOS.Application.Data.Common.Entities;
using VistaLOS.Application.Data.Master;

namespace VistaLOS.Application.Identity.Data.Entities;

public class UserEntity : AbstractEntity, IIdentifiable<long>
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string Email { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; } = null!;
    public bool ChangePasswordRequired { get; set; }
    public string? PhoneNumber { get; set; } = null!;
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public UserStatus Status { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string? Image { get; set; }

    public string TenantId { get; set; } = null!;
    public long RoleId { get; set; }
    public RoleEntity Role { get; set; } = null!;
}
