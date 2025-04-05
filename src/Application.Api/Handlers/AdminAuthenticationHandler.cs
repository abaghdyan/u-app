using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VistaLOS.Application.Common.Constants;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Identity.Services.Options;

namespace VistaLOS.Application.Api.Handlers;

public class AdminAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly AdminAuthOptions _adminAuthOptions;
    private readonly IHostEnvironment _hostEnvironment;

    public AdminAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        AdminAuthOptions adminAuthOptions,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IHostEnvironment hostEnvironment)
        : base(options, logger, encoder)
    {
        _adminAuthOptions = adminAuthOptions;
        _hostEnvironment = hostEnvironment;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        AuthenticateResult authenticateResult;

        var apiKeyToValidate = Request.Headers[ApplicationHeaders.AdminFlowKey];

        if (_hostEnvironment.IsLocal()) {
            return Task.FromResult(AuthenticateResult.Success(CreateTicket()));
        }

        if (!Request.Headers.ContainsKey(ApplicationHeaders.AdminFlowKey)) {
            authenticateResult = AuthenticateResult.Fail($"{ApplicationHeaders.AdminFlowKey} header is missing");
        }
        else if (apiKeyToValidate == _adminAuthOptions.ApiKeyValue) {
            authenticateResult = AuthenticateResult.Success(CreateTicket());
        }
        else {
            authenticateResult = AuthenticateResult.Fail("Invalid API key");
        }

        return Task.FromResult(authenticateResult);
    }

    private AuthenticationTicket CreateTicket()
    {
        var identity = new ClaimsIdentity(new List<Claim>(), Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return ticket;
    }
}