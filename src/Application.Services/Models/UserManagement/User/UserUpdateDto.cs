using VistaLOS.Application.Data.Master;

namespace VistaLOS.Application.Services.Models.UserManagement.User;

public class UserUpdateDto
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public UserStatus Status { get; set; }
}
