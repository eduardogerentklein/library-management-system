namespace Application.DTO;

public record BookDTO(
    Guid Id,
    string Title,
    string Author,
    string ISBN);
