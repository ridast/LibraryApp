using LibraryApp.Application.Common.Interfaces;
using MediatR;

namespace LibraryApp.src.LibraryApp.Application.Books.Commands
{

    namespace LibraryApp.Application.Books.Commands
    {
        public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
        {
            private readonly IApplicationDbContext _context;

            public DeleteBookCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
            {
                var book = await _context.Books.FindAsync(new object[] { request.Id }, cancellationToken);

                if (book == null)
                    throw new KeyNotFoundException($"Book with ID {request.Id} not found.");

                _context.Books.Remove(book);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            Task IRequestHandler<DeleteBookCommand>.Handle(DeleteBookCommand request, CancellationToken cancellationToken)
            {
                return Handle(request, cancellationToken);
            }
        }
    }


}
