using Microsoft.EntityFrameworkCore;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Tenant.Entities;
using VistaLOS.Application.Data.Tenant.Repositories;
using VistaLOS.Application.Services.Abstractions;
using VistaLOS.Application.Services.Models;

namespace VistaLOS.Application.Services.Impl;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserContextAccessor _userContextAccessor;

    public BookService(IBookRepository bookRepository,
        IUserContextAccessor userContextAccessor)
    {
        _bookRepository = bookRepository;
        _userContextAccessor = userContextAccessor;
    }

    public async Task<List<BookEntity>> GetBooksAsync()
    {
        var books = await _bookRepository.Get().ToListAsync();
        return books;
    }

    public async Task<BookEntity?> GetBookByIdAsync(int id)
    {
        var book = await _bookRepository.Get().FirstOrDefaultAsync(i => i.Id == id);
        return book;
    }

    public async Task AddBookAsync(BookInputModel bookInputModel)
    {
        var userContext = _userContextAccessor.GetRequiredUserContext();
        var book = new BookEntity {
            TenantId = userContext.TenantId,
            Name = bookInputModel.Name,
            Author = bookInputModel.Author,
            PageCount = bookInputModel.PageCount
        };

        await _bookRepository.CreateAsync(book);
    }
}
