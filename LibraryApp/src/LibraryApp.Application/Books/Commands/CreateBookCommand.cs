using MediatR;

namespace LibraryApp.Application.Books.Commands;

public record CreateBookCommand(string Title) : IRequest<Guid>;
