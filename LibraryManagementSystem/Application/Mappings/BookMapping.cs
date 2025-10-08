using Application.DTO;
using Domain.Entities;

namespace Application.Mappings;
public static class BookMapping
{
    public static BookDTO ToResponse(this Book book) =>
        new(
            book.Id,
            book.Title,
            book.Author,
            book.ISBN
        );
}
