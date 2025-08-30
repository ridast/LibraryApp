using MediatR;

namespace LibraryApp.src.LibraryApp.Application.Books.Commands
{
    public record DeleteBookCommand(Guid Id) : IRequest;

}
