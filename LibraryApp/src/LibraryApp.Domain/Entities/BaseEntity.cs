using LibraryApp.Domain.Common;

namespace LibraryApp.src.LibraryApp.Domain.Entities
{
    public abstract class BaseEntity
    {
        private readonly List<DomainEvent> _domainEvents = new();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }

}
