using Application.DTO;
using Application.Mappings;
using Application.Validations;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;
public class BookService(IBookRepository bookRepository) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;

    public async Task<ApplicationResult<BookDTO>> CreateAsync(Book entity, CancellationToken cancellationToken)
    {
        var createBookValidation = new CreateBookValidation();
        if (!createBookValidation.Validate(entity))
        {
            return ApplicationResult<BookDTO>.Fail($"Could not create the book due to following error: {createBookValidation.ErrorMessage}");
        }

        var bookEntity = new Book
        {
            Id = Guid.NewGuid(),
            Author = entity.Author,
            Title = entity.Title,
            ISBN = entity.ISBN
        };

        await _bookRepository.AddAsync(bookEntity, cancellationToken);

        return ApplicationResult<BookDTO>.Ok(bookEntity.ToResponse());
    }

    public async Task<ApplicationResult<BookDTO>> UpdateAsync(Book entity, CancellationToken cancellationToken)
    {
        var updateBookValidation = new UpdateBookValidation();
        if (!updateBookValidation.Validate(entity))
        {
            return ApplicationResult<BookDTO>.Fail($"Could not update the book due to following error: {updateBookValidation.ErrorMessage}");
        }

        var book = await _bookRepository.GetByIdAsync(entity.Id, cancellationToken);

        if (book is null) return ApplicationResult<BookDTO>.Fail($"Book '{entity.Id}' not found");

        var updatedBook = new Book()
        {
            Author = entity.Author,
            Title = entity.Title,
            ISBN = entity.ISBN,
            Id = book.Id
        };

        await _bookRepository.UpdateAsync(book, updatedBook, cancellationToken);

        return ApplicationResult<BookDTO>.Ok(updatedBook.ToResponse());
    }

    public async Task<ApplicationResult<bool>> DeleteAsync(Guid bookId, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);

        if (book is null) return ApplicationResult<bool>.Fail($"Failed to delete book");

        await _bookRepository.DeleteAsync(book, cancellationToken);

        return ApplicationResult<bool>.Ok(true);
    }

    public async Task<ApplicationResult<List<BookDTO>>> GetAsync(CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync(cancellationToken);

        var response = books
            .Select(book => book.ToResponse())
            .ToList();

        return ApplicationResult<List<BookDTO>>.Ok(response);
    }

    public async Task<ApplicationResult<BookDTO>> GetByIdAsync(Guid bookId, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);

        if (book is null) return ApplicationResult<BookDTO>.Fail($"Book '{bookId}' not found");

        var response = book.ToResponse();

        return ApplicationResult<BookDTO>.Ok(response);
    }
}
