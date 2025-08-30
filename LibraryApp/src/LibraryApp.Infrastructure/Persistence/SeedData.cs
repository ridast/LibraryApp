namespace LibraryApp.src.LibraryApp.Infrastructure.Persistence
{
    using global::LibraryApp.Domain.Entities;
    using global::LibraryApp.Infrastructure.Persistence;

    namespace LibraryApp.Infrastructure.Persistence
    {
        public static class SeedData
        {
            public static async Task Initialize(ApplicationDbContext context)
            {
                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Check if data already exists
                if (context.Books.Any())
                    return; // DB has been seeded

                // Seed books
                var books = new List<Book>
            {
                new Book("Clean Code"),
                new Book("Domain-Driven Design"),
                new Book("C# in Depth")
            };

                context.Books.AddRange(books);
                await context.SaveChangesAsync();
            }
        }

    }
}
