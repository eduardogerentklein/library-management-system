using Domain.Entities;

namespace Domain.Repositories;

public interface IBookRepository
{
    Task<Book> AddAsync(Book entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Book entity, CancellationToken cancellationToken = default);
    Task<List<Book>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Book> UpdateAsync(Book existingEntity, Book updatedEntity, CancellationToken cancellationToken = default);
}
