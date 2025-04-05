using Application.ExternalContracts.ResponseModels;
using Application.ExternalContracts.UserManagement;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VistaLOS.Application.Api.Controllers.Base;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Identity.Services.Abstraction;
using VistaLOS.Application.Identity.Services.Models;

namespace VistaLOS.Application.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthenticationController : TenantBaseController
{
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(
        IUserContextAccessor userContextAccessor,
        IAuthenticationService authenticationService,
        IMapper mapper)
        : base(userContextAccessor)
    {
        _authenticationService = authenticationService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost("signIn")]
    public async Task<ActionResult<DataResponse<AccessTokenVm>>> SignInAsync(UserSignInIm userSignInIm)
    {
        var userSignInModel = _mapper.Map<SignInModel>(userSignInIm);

        var accessTokenDto = await _authenticationService.SignInUserAsync(userSignInModel);

        if (accessTokenDto == null) {
            var response = DataResponse<AccessTokenVm>
                .BuildFailedResponse("User not found", StatusCodes.Status401Unauthorized);
            return ApiResponse(response);
        }

        var accessTokenVm = _mapper.Map<AccessTokenVm>(accessTokenDto);

        return ApiResponse(accessTokenVm);
    }
}