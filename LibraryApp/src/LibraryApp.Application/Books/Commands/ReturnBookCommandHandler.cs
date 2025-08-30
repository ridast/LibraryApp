using LibraryApp.Application.Common.Interfaces;
using MediatR;
using System;

namespace LibraryApp.src.LibraryApp.Application.Books.Commands
{
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand>
    {
        private readonly IApplicationDbContext _context;

        public ReturnBookCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { request.Id }, cancellationToken);

            if (book == null)
                throw new KeyNotFoundException($"Book with ID {request.Id} not found.");

            if (book.IsAvailable)
                throw new InvalidOperationException("This book is already available.");

            book.IsAvailable = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        Task IRequestHandler<ReturnBookCommand>.Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }

}
