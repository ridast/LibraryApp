using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;
using LibraryApp.src.LibraryApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>(); // ignore base class
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker
      .Entries<BaseEntity>() // ✅ generic
      .Where(x => x.Entity.DomainEvents.Any())
      .Select(x => x.Entity)
      .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        domainEntities.ForEach(x => x.ClearDomainEvents());

        return result;
    }
}

