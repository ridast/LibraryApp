namespace LibraryApp.Domain.Entities;

public class Book
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public bool IsAvailable { get;  set; } = true;

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
            throw new InvalidOperationException("Book not available.");

        IsAvailable = false;
    }

    public void Return()
    {
        IsAvailable = true;
    }
}
