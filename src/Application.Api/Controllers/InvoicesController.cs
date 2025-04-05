using Microsoft.AspNetCore.Mvc;
using VistaLOS.Application.Api.Attributes;
using VistaLOS.Application.Api.Controllers.Base;
using VistaLOS.Application.Common.Identity;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Master.Entities;
using VistaLOS.Application.Services.Abstractions;
using VistaLOS.Application.Services.Models;

namespace VistaLOS.Application.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class InvoicesController : TenantBaseController
{
    private readonly IInvoiceService _invoiceService;
    public InvoicesController(IUserContextAccessor userContextAccessor,
        IInvoiceService invoiceService)
        : base(userContextAccessor)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet]
    [Permission(Permissions.ViewInvoice)]
    public async Task<ActionResult<List<InvoiceEntity>>> GetInvoices()
    {
        var invoices = await _invoiceService.GetInvoicesAsync();
        return Ok(invoices);
    }

    [HttpGet("{invoiceId}")]
    [Permission(Permissions.ViewInvoice)]
    public async Task<ActionResult<InvoiceEntity>> GetInvoiceById(int invoiceId)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
        return Ok(invoice);
    }

    [HttpPost("addInvoice")]
    [Permission(Permissions.EditInvoice)]
    public async Task<ActionResult<InvoiceEntity>> AddInvoice(InvoiceInputModel invoiceInputModel)
    {
        await _invoiceService.AddInvoiceAsync(invoiceInputModel);
        return Ok();
    }
}