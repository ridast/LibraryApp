using FluentAssertions;
using LibraryApp.Application.Books.Commands;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Domain.Entities;
using LibraryApp.src.LibraryApp.Application.Books.Commands;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xunit;


namespace LibraryApp.Tests.Application.Books.Commands
{
    public class BorrowBookCommandTests
    {
        [Fact]
        public async Task Handle_ShouldSetIsBorrowedTrue_WhenBookExists()
        {
            // Arrange
            var book = new Book("My Title");
            var command = new BorrowBookCommand(book.Id);

            var dbContextMock = new Mock<IApplicationDbContext>();

            // Match the exact signature: FindAsync(object[] keyValues, CancellationToken)
            dbContextMock.Setup(x => x.Books.FindAsync(It.Is<object[]>(keys => keys.Contains(book.Id)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var handler = new BorrowBookCommandHandler(dbContextMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(book.IsAvailable); // Should be false after borrowing
            dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}