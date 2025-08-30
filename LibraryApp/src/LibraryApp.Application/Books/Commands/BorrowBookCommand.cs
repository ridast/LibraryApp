using MediatR;

namespace LibraryApp.src.LibraryApp.Application.Books.Commands
{
    public record BorrowBookCommand(Guid Id) : IRequest;

}
