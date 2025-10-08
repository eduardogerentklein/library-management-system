using Application.Services;
using Domain.Entities;

namespace ConsoleApp.Menus;

public class MainMenu(IBookService bookService)
{
    private readonly IBookService _bookService = bookService;

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Library Management ===");
            Console.WriteLine("1 - Add book");
            Console.WriteLine("2 - List books");
            Console.WriteLine("3 - Get book by Id");
            Console.WriteLine("4 - Update book");
            Console.WriteLine("5 - Delete book");
            Console.WriteLine("6 - Exit");
            Console.Write("\nOption: ");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1": await AddBookAsync(); break;
                case "2": await ListBooksAsync(); break;
                case "3": await GetBookByIdAsync(); break;
                case "4": await UpdateBookAsync(); break;
                case "5": await DeleteBookAsync(); break;
                case "6": return;
                default: Console.WriteLine("Invalid option!"); break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private async Task AddBookAsync()
    {
        Console.Write("Title: ");
        var title = Console.ReadLine()!;
        Console.Write("Author: ");
        var author = Console.ReadLine()!;
        Console.Write("ISBN: ");
        var isbn = Console.ReadLine()!;

        var book = new Book
        {
            Title = title,
            Author = author,
            ISBN = isbn
        };

        var result = await _bookService.CreateAsync(book, CancellationToken.None);
        if (result.Success)
            Console.WriteLine($"Book added successfully! Id: {result.Value!.Id}");
        else
            Console.WriteLine($"Error adding book: {result.Error}");
    }

    private async Task ListBooksAsync()
    {
        var books = await _bookService.GetAsync(CancellationToken.None);

        if (books.Value?.Count is not > 0)
        {
            Console.WriteLine("\nNo books found, try inserting one book");
            return;
        }
        else
            Console.WriteLine("\nBooks:");

        foreach (var b in books.Value!)
        {
            Console.WriteLine($"- {b.Title} by {b.Author} (ISBN: {b.ISBN})");
        }
    }

    private async Task GetBookByIdAsync()
    {
        Console.Write("Enter the Id of the book: ");
        if (!Guid.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Invalid Id format.");
            return;
        }

        var book = await _bookService.GetByIdAsync(id, CancellationToken.None);

        if (book.Success)
        {
            Console.WriteLine("\nBook details:");
            Console.WriteLine($"Id: {book.Value!.Id}");
            Console.WriteLine($"Title: {book.Value.Title}");
            Console.WriteLine($"Author: {book.Value.Author}");
            Console.WriteLine($"ISBN: {book.Value.ISBN}");
        }
        else
        {
            Console.WriteLine($"Error retrieving book: {book.Error}");
        }
    }

    private async Task UpdateBookAsync()
    {
        Console.Write("Enter the Id of the book to update: ");
        if (!Guid.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Invalid Id format.");
            return;
        }

        var existing = await _bookService.GetByIdAsync(id, CancellationToken.None);
        if (!existing.Success)
        {
            Console.WriteLine(existing.Error);
            return;
        }

        Console.WriteLine($"Current Title: {existing.Value!.Title}");
        Console.Write("New Title (leave empty to keep): ");
        var title = Console.ReadLine();
        title = string.IsNullOrWhiteSpace(title) ? existing.Value.Title : title;

        Console.WriteLine($"Current Author: {existing.Value.Author}");
        Console.Write("New Author (leave empty to keep): ");
        var author = Console.ReadLine();
        author = string.IsNullOrWhiteSpace(author) ? existing.Value.Author : author;

        Console.WriteLine($"Current ISBN: {existing.Value.ISBN}");
        Console.Write("New ISBN (leave empty to keep): ");
        var isbn = Console.ReadLine();
        isbn = string.IsNullOrWhiteSpace(isbn) ? existing.Value.ISBN : isbn;

        var updatedBook = new Book
        {
            Id = id,
            Title = title!,
            Author = author!,
            ISBN = isbn!
        };

        var result = await _bookService.UpdateAsync(updatedBook, CancellationToken.None);
        if (result.Success)
            Console.WriteLine($"Book updated successfully: {result.Value!.Title} by {result.Value.Author}");
        else
            Console.WriteLine($"Error updating book: {result.Error}");
    }

    private async Task DeleteBookAsync()
    {
        Console.Write("Enter the Id of the book to delete: ");
        if (!Guid.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Invalid Id format.");
            return;
        }

        var deleteBook = await _bookService.DeleteAsync(id, CancellationToken.None);
        if (deleteBook.Success)
            Console.WriteLine("Book deleted successfully.");
        else
            Console.WriteLine($"Error deleting book: {deleteBook.Error}");
    }
}
