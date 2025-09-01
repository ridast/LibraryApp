using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LibraryApp.Application.Books.Commands;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Domain.Entities;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp.Tests.Application.Books.Commands
{
    public class CreateBookCommandTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly CreateBookCommandHandler _handler;

        public CreateBookCommandTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();

            // Set up the Books DbSet mock (in-memory collection)
            var books = new List<Book>();
            var mockDbSet = new Mock<DbSet<Book>>();
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(books.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(books.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(books.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(() => books.GetEnumerator());

            _dbContextMock.Setup(db => db.Books).Returns(mockDbSet.Object);
            _dbContextMock.Setup(db => db.Books.Add(It.IsAny<Book>()))
                          .Callback<Book>(book => books.Add(book));

            _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(1);

            _handler = new CreateBookCommandHandler(_dbContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateBook_AndReturnId()
        {
            // Arrange
            var command = new CreateBookCommand("Test Book");
          

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            _dbContextMock.Verify(db => db.Books.Add(It.IsAny<Book>()), Times.Once);
            _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
