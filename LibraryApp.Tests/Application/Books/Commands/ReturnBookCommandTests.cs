using FluentAssertions;
using LibraryApp.Application.Books.Commands;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Domain.Entities;
using LibraryApp.src.LibraryApp.Application.Books.Commands;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApp.Tests.Application.Books.Commands
{
    public class ReturnBookCommandTests
    {
        [Fact]
        public async Task Handle_ShouldSetIsAvailableTrue_WhenBookExists()
        {
            // Arrange
            var book = new Book("Test Book");
            book.IsAvailable = false; // Start with book borrowed (not available)

            var dbContextMock = new Mock<IApplicationDbContext>();

            // Create a mock DbSet that properly handles FindAsync with object array
            var mockDbSet = new Mock<DbSet<Book>>();

            // Setup FindAsync to return the book when called with the correct ID
            mockDbSet.Setup(m => m.FindAsync(It.Is<object[]>(keys => keys.Length == 1 && (Guid)keys[0] == book.Id), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(book);

            dbContextMock.Setup(x => x.Books).Returns(mockDbSet.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new ReturnBookCommandHandler(dbContextMock.Object);

            // Act
            await handler.Handle(new ReturnBookCommand(book.Id), CancellationToken.None);

            // Assert
            book.IsAvailable.Should().BeTrue(); // Should be true after returning
            dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}