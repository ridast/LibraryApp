// src/LibraryApp.Domain/Common/DomainEvent.cs
using MediatR;

namespace LibraryApp.Domain.Common;

public abstract class DomainEvent : INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
