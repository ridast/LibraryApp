using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LibraryApp.Application.Books.Queries;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Domain.Entities;
using Moq;
using Xunit;

namespace LibraryApp.Tests.Application.Books.Queries
{
    public class GetBooksQueryTests
    {
        [Fact]
        public async Task Handle_ShouldReturnAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book("Book 1"),
                new Book("Book 2")
            };

            var mockDbSet = MockDbSetExtensions.CreateMockDbSet(books);

            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new GetBooksQueryHandler(dbContextMock.Object);

            // Act
            var result = await handler.Handle(new GetBooksQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
        }
    }
}
