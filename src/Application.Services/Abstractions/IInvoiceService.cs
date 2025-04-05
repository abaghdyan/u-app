using VistaLOS.Application.Data.Master.Entities;
using VistaLOS.Application.Services.Models;

namespace VistaLOS.Application.Services.Abstractions;

public interface IInvoiceService
{
    Task<List<InvoiceEntity>> GetInvoicesAsync();
    Task<InvoiceEntity?> GetInvoiceByIdAsync(int id);
    Task AddInvoiceAsync(InvoiceInputModel invoiceInputModel);
}
