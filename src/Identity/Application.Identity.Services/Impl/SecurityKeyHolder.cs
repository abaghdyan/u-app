using Microsoft.IdentityModel.Tokens;

namespace VistaLOS.Application.Identity.Services.Impl;

public class SecurityKeyHolder
{
    public RsaSecurityKey RsaSecurityKey { get; set; } = null!;

    public SecurityKeyHolder(RsaSecurityKey rsaSecurityKey)
    {
        RsaSecurityKey = rsaSecurityKey;
    }
}
