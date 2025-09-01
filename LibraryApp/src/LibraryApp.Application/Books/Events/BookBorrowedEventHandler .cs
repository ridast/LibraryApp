using MediatR;
using LibraryApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using LibraryApp.src.LibraryApp.Domain.Entities;

namespace LibraryApp.src.LibraryApp.Application.Books.Events
{
    public class BookBorrowedEventHandler : INotificationHandler<BookBorrowedEvent>
    {
        public Task Handle(BookBorrowedEvent notification, CancellationToken cancellationToken)
        {
            // Add your logic here (e.g., send email, log, etc.)
            Console.WriteLine($"Book borrowed: {notification.Book.Title}");
            return Task.CompletedTask;
        }
    }
}
