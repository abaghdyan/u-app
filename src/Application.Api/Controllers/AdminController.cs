using Microsoft.AspNetCore.Mvc;
using VistaLOS.Application.Api.Controllers.Base;
using VistaLOS.Application.Services.Abstractions;

namespace VistaLOS.Application.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AdminController : MasterBaseController
{
    private readonly ITenantService _tenantService;

    public AdminController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpPost("createDemoTenants")]
    public async Task<IActionResult> CreateDemoTenants()
    {
        await _tenantService.CreateDemoTenantsAsync();
        return Ok();
    }
}