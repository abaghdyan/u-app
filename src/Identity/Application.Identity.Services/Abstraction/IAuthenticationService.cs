using VistaLOS.Application.Identity.Services.Models;

namespace VistaLOS.Application.Identity.Services.Abstraction;

public interface IAuthenticationService
{
    Task<AccessTokenModel?> SignInUserAsync(SignInModel userSignInModel);
}
