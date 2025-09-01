using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

public static class MockDbSetExtensions
{
    public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();

        var asyncEnumerable = new TestAsyncEnumerable<T>(queryable);

        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IAsyncEnumerable<T>>()
               .Setup(d => d.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
               .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(asyncEnumerable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        return mockSet;
    }

    private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;

        public T Current => _inner.Current;
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
        public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(_inner.MoveNext());
    }

    private class TestAsyncEnumerable<T> : IAsyncEnumerable<T>, IQueryable<T>
    {
        private readonly IQueryable<T> _inner;
        public TestAsyncEnumerable(IQueryable<T> inner) => _inner = inner;

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            new TestAsyncEnumerator<T>(_inner.GetEnumerator());

        public Type ElementType => _inner.ElementType;
        public Expression Expression => _inner.Expression;
        public IQueryProvider Provider => new TestAsyncQueryProvider<T>(_inner.Provider);

        // 👇 These two were missing:
        public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();
    }

    private class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;
        internal TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

        public IQueryable CreateQuery(Expression expression) =>
            new TestAsyncEnumerable<TEntity>(_inner.CreateQuery<TEntity>(expression));

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
            new TestAsyncEnumerable<TElement>(_inner.CreateQuery<TElement>(expression));

        public object Execute(Expression expression) => _inner.Execute(expression);
        public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default) =>
            Task.FromResult(Execute<TResult>(expression)).Result;
    }
}
