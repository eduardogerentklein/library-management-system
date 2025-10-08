using Application.DTO;
using Domain.Entities;

namespace Application.Services;

public interface IBookService
{
    Task<ApplicationResult<BookDTO>> CreateAsync(Book entity, CancellationToken cancellationToken);
    Task<ApplicationResult<BookDTO>> UpdateAsync(Book entity, CancellationToken cancellationToken);
    Task<ApplicationResult<bool>> DeleteAsync(Guid bookId, CancellationToken cancellationToken);
    Task<ApplicationResult<BookDTO>> GetByIdAsync(Guid bookId, CancellationToken cancellationToken);
    Task<ApplicationResult<List<BookDTO>>> GetAsync(CancellationToken cancellationToken);
}
