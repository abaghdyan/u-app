namespace VistaLOS.Application.Identity.Services.Models.User;

public class UserCreateModel
{
    public string TenantId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public long RoleId { get; set; }
}
