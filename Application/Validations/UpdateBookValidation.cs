using Application.Extensions;
using Domain.Entities;

namespace Application.Validations;
public class UpdateBookValidation
{
    public string? ErrorMessage { get; private set; }

    public bool Validate(Book book)
    {
        if (!book.ISBN.IsValidIsbn13())
        {
            ErrorMessage = "ISBN is not in a valid format.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(book.Title))
        {
            ErrorMessage = "Book title cannot be empty.";
            return false;
        }
        else if (book.Title.Length > 150)
        {
            ErrorMessage = "Book title exceeds maximum length of 150 characters.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(book.Author))
        {
            ErrorMessage = "Book author cannot be empty.";
            return false;
        }
        else if (book.Author.Length > 100)
        {
            ErrorMessage = "Book author exceeds maximum length of 100 characters.";
            return false;
        }

        return true;
    }
}
