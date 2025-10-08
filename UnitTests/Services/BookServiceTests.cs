using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;

namespace UnitTests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly BookService _service;

    public BookServiceTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
        _service = new BookService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenISBNValidationFails()
    {
        // Arrange
        var invalidBook = new Book { Author = "Jane Doe", ISBN = "12345", Title = "Doe Jane" };

        // Act
        var result = await _service.CreateAsync(invalidBook, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().Contain("Could not create the book due to following error: ISBN is not in a valid format.");
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenBookTitleValidationFails()
    {
        // Arrange
        var invalidBook = new Book { Author = "Jane Doe", ISBN = "9781234567897", Title = "" };

        // Act
        var result = await _service.CreateAsync(invalidBook, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().Contain("Could not create the book due to following error: Book title cannot be empty.");
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenBookAuthorValidationFails()
    {
        // Arrange
        var invalidBook = new Book { Author = "", ISBN = "9781234567897", Title = "Forever More" };

        // Act
        var result = await _service.CreateAsync(invalidBook, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().Contain("Could not create the book due to following error: Book author cannot be empty.");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnOk_WhenValidBook()
    {
        // Arrange
        var book = new Book { Title = "Test", Author = "Author", ISBN = "9781234567897" };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book b, CancellationToken ct) => b);

        // Act
        var result = await _service.CreateAsync(book, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Title.Should().Be(book.Title);
        result.Error.Should().BeNull();

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldFail_WhenBookNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Book?)null);

        // Act
        var result = await _service.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().Contain($"Book '{id}' not found");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBookDTO_WhenBookExists()
    {
        // Arrange
        var book = new Book { Id = Guid.NewGuid(), Title = "Test", Author = "Author", ISBN = "9781234567897" };
        _repositoryMock.Setup(r => r.GetByIdAsync(book.Id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(book);

        // Act
        var result = await _service.GetByIdAsync(book.Id, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(book.Id);
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldFail_WhenBookNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Book?)null);

        // Act
        var result = await _service.DeleteAsync(id, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Value.Should().BeFalse();
        result.Error.Should().Contain("Failed to delete book");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnOk_WhenBookExists()
    {
        // Arrange
        var book = new Book { Id = Guid.NewGuid(), Title = "Test", Author = "Author", ISBN = "9781234567897" };
        _repositoryMock.Setup(r => r.GetByIdAsync(book.Id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(book);
        _repositoryMock.Setup(r => r.DeleteAsync(book, It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(book.Id, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().BeTrue();
        result.Error.Should().BeNull();

        _repositoryMock.Verify(r => r.DeleteAsync(book, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnListOfBookDTOs()
    {
        // Arrange
        var books = new List<Book>
        {
            new() { Id = Guid.NewGuid(), Title = "A", Author = "AuthorA", ISBN = "9781234567897" },
            new() { Id = Guid.NewGuid(), Title = "B", Author = "AuthorB", ISBN = "9781234567890" }
        };
        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(books);

        // Act
        var result = await _service.GetAsync(CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Error.Should().BeNull();
    }
}
