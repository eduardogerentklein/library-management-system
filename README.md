# Library Management System (Console Application)

A simple console-based library management system built with .NET 8, following Repository Patterns, Clean Architecture, SOLID principles, and using Dependency Injection.
The application allows you to manage books: add, update, delete, list, and search by ID.

## Features

- Add a new book with Title, Author, and ISBN
- List all books in the library
- Update book information
- Delete a book
- Search a book by ID
- Input validation (e.g., ISBN 13-character validation)
- Returns DTOs to avoid exposing entities directly

## Architecture

The project follows Clean Architecture and SOLID principles:

- Domain Layer – Entity and Repository Interface (Book, IBookRepository)
- Application Layer – Services and validations (BookService)
- Infrastructure Layer – Repository and persistence (BookRepository, ApplicationDbContext)
- Console/UI Layer – Menu and user interaction (MainMenu)
- Dependency Injection is used to wire services and repositories in Program.cs

## Getting Started
### Prerequisites

- .NET 8 SDK
- IDE (Visual Studio 2022 / VS Code)

### Installation

#### Clone the repository:

```bash
git clone git@github.com:eduardogerentklein/library-management-system.git
cd library-management-console
```

#### Restore dependencies:
```bash
dotnet restore
```

### Running the Application
Run the console app:

```bash
dotnet run --project src/LibraryManagement.ConsoleApp
```

## Testing

Unit tests are provided for both BookService and BookRepository:

Run all tests:
```bash
dotnet test
```