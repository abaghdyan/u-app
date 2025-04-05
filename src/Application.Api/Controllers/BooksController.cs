using Microsoft.AspNetCore.Mvc;
using VistaLOS.Application.Api.Attributes;
using VistaLOS.Application.Api.Controllers.Base;
using VistaLOS.Application.Common.Identity;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Master.Entities;
using VistaLOS.Application.Data.Tenant.Entities;
using VistaLOS.Application.Services.Abstractions;
using VistaLOS.Application.Services.Models;

namespace VistaLOS.Application.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BooksController : TenantBaseController
{
    private readonly IBookService _bookService;

    public BooksController(IUserContextAccessor userContextAccessor,
        IBookService bookService)
        : base(userContextAccessor)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [Permission(Permissions.ViewBook)]
    public async Task<ActionResult<List<BookEntity>>> GetBooks()
    {
        var books = await _bookService.GetBooksAsync();
        return Ok(books);
    }

    [HttpGet("{bookId}")]
    [Permission(Permissions.ViewBook)]
    public async Task<ActionResult<BookEntity>> GetBookById(int bookId)
    {
        var book = await _bookService.GetBookByIdAsync(bookId);
        return Ok(book);
    }

    [HttpPost("addBook")]
    [Permission(Permissions.EditBook)]
    public async Task<ActionResult<InvoiceEntity>> AddBook(BookInputModel bookInputModel)
    {
        await _bookService.AddBookAsync(bookInputModel);
        return Ok();
    }
}