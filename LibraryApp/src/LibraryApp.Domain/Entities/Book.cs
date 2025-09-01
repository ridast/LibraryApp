using LibraryApp.Domain.Common;
using LibraryApp.src.LibraryApp.Domain.Entities;
using System.Collections.Generic;

namespace LibraryApp.Domain.Entities;

public class Book
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public bool IsAvailable { get;  set; } = true;

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Book() {
        Title = string.Empty;
    } // EF Core

    public Book(string title)
    {
        Title = title;
    }

    public void Borrow()
    {
        if (!IsAvailable)
            throw new InvalidOperationException("Book is already borrowed.");

        IsAvailable = false;
        _domainEvents.Add(new BookBorrowedEvent(this));
    }

    public void Return()
    {
        IsAvailable = true;
        _domainEvents.Add(new BookReturnedEvent(this));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
