using MediatR;

namespace LibraryApp.Application.Books.Queries;

public record GetBooksQuery() : IRequest<List<BookDto>>;

public record BookDto(Guid Id, string Title, bool IsAvailable);
