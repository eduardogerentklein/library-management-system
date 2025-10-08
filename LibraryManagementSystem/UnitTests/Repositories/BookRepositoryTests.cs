using Domain.Entities;
using FluentAssertions;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Repositories;

public class BookRepositoryTests
{
    private static BookRepository CreateRepository()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        return new BookRepository(context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddBook()
    {
        // Arrange
        var repo = CreateRepository();
        var book = new Book { Id = Guid.NewGuid(), Title = "Test", Author = "Author", ISBN = "9781234567897" };

        // Act
        var result = await repo.AddAsync(book);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(book.Id);

        var dbBook = await repo.GetByIdAsync(book.Id);
        dbBook.Should().NotBeNull();
        dbBook!.Title.Should().Be("Test");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateBook()
    {
        // Arrange
        var repo = CreateRepository();
        var book = new Book { Id = Guid.NewGuid(), Title = "Old", Author = "Author", ISBN = "9781234567897" };
        await repo.AddAsync(book);

        var updatedBook = new Book { Id = book.Id, Title = "New", Author = "Author", ISBN = "9781234567897" };

        // Act
        var result = await repo.UpdateAsync(book, updatedBook);

        // Assert
        result.Title.Should().Be("New");
        var dbBook = await repo.GetByIdAsync(book.Id);
        dbBook!.Title.Should().Be("New");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveBook()
    {
        // Arrange
        var repo = CreateRepository();
        var book = new Book { Id = Guid.NewGuid(), Title = "Test", Author = "Author", ISBN = "9781234567897" };
        await repo.AddAsync(book);

        // Act
        await repo.DeleteAsync(book);

        // Assert
        var dbBook = await repo.GetByIdAsync(book.Id);
        dbBook.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBooks()
    {
        // Arrange
        var repo = CreateRepository();

        var book1 = new Book() { Id = Guid.NewGuid(), Title = "A", Author = "AuthorA", ISBN = "9781234567897" };
        var book2 = new Book() { Id = Guid.NewGuid(), Title = "B", Author = "AuthorB", ISBN = "9781234567890" };
        await repo.AddAsync(book1);
        await repo.AddAsync(book2);

        // Act
        var result = await repo.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainSingle(b => b.Title == "A");
        result.Should().ContainSingle(b => b.Title == "B");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBook_WhenExists()
    {
        // Arrange
        var repo = CreateRepository();
        var book = new Book { Id = Guid.NewGuid(), Title = "Test", Author = "Author", ISBN = "9781234567897" };
        await repo.AddAsync(book);

        // Act
        var result = await repo.GetByIdAsync(book.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(book.Id);
        result.Title.Should().Be("Test");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var repo = CreateRepository();
        var id = Guid.NewGuid();

        // Act
        var result = await repo.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}

