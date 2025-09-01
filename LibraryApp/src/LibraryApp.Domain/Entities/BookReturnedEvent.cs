using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;

public class BookReturnedEvent : DomainEvent
{
    public Book Book { get; }
    public BookReturnedEvent(Book book) => Book = book;
}