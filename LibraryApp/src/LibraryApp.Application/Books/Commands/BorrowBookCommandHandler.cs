using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Domain.Entities;
using MediatR;
using System;

namespace LibraryApp.src.LibraryApp.Application.Books.Commands
{
    public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand>
    {
        private readonly IApplicationDbContext _context;

        public BorrowBookCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { request.Id }, cancellationToken);

            if (book == null)
                throw new KeyNotFoundException($"Book with ID {request.Id} not found.");

            if (!book.IsAvailable)
                throw new InvalidOperationException("This book is already borrowed.");

            book.IsAvailable = false;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;


        }

        Task IRequestHandler<BorrowBookCommand>.Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }

}
