using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using VistaLOS.Application.Common.Constants;
using VistaLOS.Application.Common.Helpers;
using VistaLOS.Application.Identity.Services.Abstraction;
using VistaLOS.Application.Identity.Services.Managers;
using VistaLOS.Application.Identity.Services.Models;
using VistaLOS.Application.Identity.Services.Models.User;
using VistaLOS.Application.Identity.Services.Options;

namespace VistaLOS.Application.Identity.Services.Impl;

public class AuthenticationService : IAuthenticationService
{
    private readonly IdentityUserManager _userManager;
    private readonly SecurityKeyHolder _securityKeyHolder;
    private readonly AuthOptions _authOptions;

    public AuthenticationService(SecurityKeyHolder securityKeyHolder,
        IdentityUserManager userManager,
        AuthOptions authOptions)
    {
        _securityKeyHolder = securityKeyHolder;
        _userManager = userManager;
        _authOptions = authOptions;
    }

    public async Task<AccessTokenModel?> SignInUserAsync(SignInModel userSignInModel)
    {
        var user = await _userManager.GetUserByEmailAsync(userSignInModel.Email);

        if (user != null) {
            var isPasswordCorrect = await _userManager
                .IsPasswordCorrectAsync(user.Id, userSignInModel.Password);

            if (isPasswordCorrect) {
                await _userManager.UpdateUserLoginDateAsync(user.Id);

                return new AccessTokenModel {
                    AccessToken = GenerateAccessToken(user),
                    ExpiresInSec = _authOptions.ExpirationTimeInSec
                };
            }
        }

        return null;
    }

    private string GenerateAccessToken(UserModel user)
    {
        var permissionIds = user.Role.Permissions.Select(p => p.Id).ToList();
        var serializedPermissionIds = JsonHelper.SerializeObject(permissionIds);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(ApplicationClaims.UserId, user.Id.ToString()),
            new(ApplicationClaims.PermissionIds, serializedPermissionIds),
            new(ApplicationClaims.TenantId, user.TenantId.ToString())
        };

        var jwt = new JwtSecurityToken(
            issuer: "VistaLOS",
            audience: "VistaLOS",
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddSeconds(_authOptions.ExpirationTimeInSec),
            signingCredentials: new SigningCredentials(
                _securityKeyHolder.RsaSecurityKey, SecurityAlgorithms.RsaSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
