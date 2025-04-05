namespace Application.ExternalContracts.UserManagement;

public class AccessTokenVm
{
    public string AccessToken { get; set; } = null!;
    public long ExpiresInSec { get; set; }
}
