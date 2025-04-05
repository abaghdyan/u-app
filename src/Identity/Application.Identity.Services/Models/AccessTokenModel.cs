namespace VistaLOS.Application.Identity.Services.Models;

public class AccessTokenModel
{
    public string AccessToken { get; set; } = null!;
    public long ExpiresInSec { get; set; }
}
