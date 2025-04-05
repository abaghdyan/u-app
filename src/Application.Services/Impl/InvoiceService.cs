using Microsoft.EntityFrameworkCore;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Master.Entities;
using VistaLOS.Application.Data.Master.Repositories;
using VistaLOS.Application.Services.Abstractions;
using VistaLOS.Application.Services.Models;

namespace VistaLOS.Application.Services.Impl;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUserContextAccessor _userContextAccessor;

    public InvoiceService(IInvoiceRepository invoiceRepository,
        IUserContextAccessor userContextAccessor)
    {
        _invoiceRepository = invoiceRepository;
        _userContextAccessor = userContextAccessor;
    }

    public async Task<List<InvoiceEntity>> GetInvoicesAsync()
    {
        var invoices = await _invoiceRepository.Get().ToListAsync();
        return invoices;
    }

    public async Task<InvoiceEntity?> GetInvoiceByIdAsync(int id)
    {
        var invoice = await _invoiceRepository.Get().FirstOrDefaultAsync(i => i.Id == id);
        return invoice;
    }

    public async Task AddInvoiceAsync(InvoiceInputModel invoiceInputModel)
    {
        var userContext = _userContextAccessor.GetRequiredUserContext();
        var invoice = new InvoiceEntity {
            TenantId = userContext.TenantId,
            Amount = invoiceInputModel.Amount,
            Date = invoiceInputModel.Date
        };

        await _invoiceRepository.CreateAsync(invoice);
    }
}
