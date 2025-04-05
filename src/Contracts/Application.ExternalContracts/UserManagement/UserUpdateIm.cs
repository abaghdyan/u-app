namespace Application.ExternalContracts.UserManagement;

public class UserUpdateIm
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
