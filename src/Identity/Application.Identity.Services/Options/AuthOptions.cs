namespace VistaLOS.Application.Identity.Services.Options;

public class AuthOptions
{
    public const string Section = "Auth";

    public string PrivateKey { get; set; } = null!;
    public long ExpirationTimeInSec { get; set; }
}
