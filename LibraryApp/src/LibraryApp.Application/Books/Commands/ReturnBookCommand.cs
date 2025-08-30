using MediatR;

namespace LibraryApp.src.LibraryApp.Application.Books.Commands
{
    public record ReturnBookCommand(Guid Id) : IRequest;

}
