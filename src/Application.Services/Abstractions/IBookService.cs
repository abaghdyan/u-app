using VistaLOS.Application.Data.Tenant.Entities;
using VistaLOS.Application.Services.Models;

namespace VistaLOS.Application.Services.Abstractions;

public interface IBookService
{
    Task<List<BookEntity>> GetBooksAsync();
    Task<BookEntity?> GetBookByIdAsync(int id);
    Task AddBookAsync(BookInputModel bookInputModel);
}
