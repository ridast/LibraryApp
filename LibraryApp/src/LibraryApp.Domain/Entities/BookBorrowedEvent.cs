using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;

namespace LibraryApp.src.LibraryApp.Domain.Entities
{
    public class BookBorrowedEvent : DomainEvent
    {
        public Book Book { get; }
        public BookBorrowedEvent(Book book) => Book = book;
    }
}
