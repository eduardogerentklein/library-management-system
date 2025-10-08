using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class BookRepository(ApplicationDbContext dbContext) : IBookRepository
{
    private readonly ApplicationDbContext DbContext = dbContext;

    public async Task<Book> AddAsync(Book entity, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<Book>().AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Book> UpdateAsync(Book existingEntity, Book updatedEntity, CancellationToken cancellationToken = default)
    {
        DbContext
            .Set<Book>()
            .Entry(existingEntity)
            .CurrentValues
            .SetValues(updatedEntity);

        await DbContext.SaveChangesAsync(cancellationToken);

        return updatedEntity;
    }

    public async Task DeleteAsync(Book entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<Book>().Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Book>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbContext
            .Set<Book>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbContext.Set<Book>().FindAsync([id], cancellationToken);
}
