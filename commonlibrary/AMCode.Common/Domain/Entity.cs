using System;
using System.Collections.Generic;

namespace AMCode.Common.Domain
{
    /// <summary>
    /// Base class for all domain entities with a strongly-typed ID
    /// </summary>
    /// <typeparam name="TId">The type of the entity's identifier</typeparam>
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// Gets or sets the entity's unique identifier
        /// </summary>
        public TId Id { get; protected set; }

        private readonly List<IDomainEvent> _domainEvents = new();

        /// <summary>
        /// Initializes a new instance of the Entity class
        /// </summary>
        /// <param name="id">The unique identifier for the entity</param>
        protected Entity(TId id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets all domain events that have been raised by this entity
        /// </summary>
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Adds a domain event to be published later
        /// </summary>
        /// <param name="domainEvent">The domain event to add</param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Removes a domain event from the collection
        /// </summary>
        /// <param name="domainEvent">The domain event to remove</param>
        protected void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        /// <summary>
        /// Clears all domain events (typically called after publishing)
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        /// <summary>
        /// Determines whether the specified entity is equal to the current entity
        /// </summary>
        /// <param name="other">The entity to compare with the current entity</param>
        /// <returns>true if the specified entity is equal to the current entity; otherwise, false</returns>
        public bool Equals(Entity<TId> other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current entity
        /// </summary>
        /// <param name="obj">The object to compare with the current entity</param>
        /// <returns>true if the specified object is equal to the current entity; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<TId>);
        }

        /// <summary>
        /// Returns a hash code for the current entity
        /// </summary>
        /// <returns>A hash code for the current entity</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<TId>.Default.GetHashCode(Id);
        }

        /// <summary>
        /// Determines whether two entities are equal
        /// </summary>
        /// <param name="left">The first entity to compare</param>
        /// <param name="right">The second entity to compare</param>
        /// <returns>true if the entities are equal; otherwise, false</returns>
        public static bool operator ==(Entity<TId> left, Entity<TId> right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether two entities are not equal
        /// </summary>
        /// <param name="left">The first entity to compare</param>
        /// <param name="right">The second entity to compare</param>
        /// <returns>true if the entities are not equal; otherwise, false</returns>
        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Returns a string representation of the entity
        /// </summary>
        /// <returns>A string representation of the entity</returns>
        public override string ToString()
        {
            return $"{GetType().Name}[Id={Id}]";
        }
    }

    /// <summary>
    /// Base class for domain entities with a Guid ID
    /// </summary>
    public abstract class Entity : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the Entity class with a Guid ID
        /// </summary>
        /// <param name="id">The unique identifier for the entity</param>
        protected Entity(Guid id) : base(id)
        {
        }
    }

    /// <summary>
    /// Marker interface for domain events
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the date and time when the domain event occurred
        /// </summary>
        DateTime OccurredOn { get; }
    }

    /// <summary>
    /// Base class for domain events
    /// </summary>
    public abstract class DomainEvent : IDomainEvent
    {
        /// <summary>
        /// Gets the date and time when the domain event occurred
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Initializes a new instance of the DomainEvent class
        /// </summary>
        protected DomainEvent()
        {
            OccurredOn = DateTime.UtcNow;
        }
    }
}

