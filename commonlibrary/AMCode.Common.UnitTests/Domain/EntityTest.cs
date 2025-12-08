using System;
using System.Collections.Generic;
using AMCode.Common.Domain;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Domain
{
    [TestFixture]
    public class EntityTest
    {
        private class TestEntity : Entity<Guid>
        {
            public string Name { get; set; }

            public TestEntity(Guid id, string name) : base(id)
            {
                Name = name;
            }

            public void RaiseTestEvent()
            {
                AddDomainEvent(new TestDomainEvent { Message = "Test event" });
            }
        }

        private class TestDomainEvent : DomainEvent
        {
            public string Message { get; set; }
        }

        [Test]
        public void Entity_WithGuidId_ShouldSetId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var entity = new TestEntity(id, "Test");

            // Assert
            Assert.AreEqual(id, entity.Id);
        }

        [Test]
        public void Entity_Equality_WithSameId_ShouldBeEqual()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity1 = new TestEntity(id, "Test 1");
            var entity2 = new TestEntity(id, "Test 2");

            // Act & Assert
            Assert.IsTrue(entity1.Equals(entity2));
            Assert.IsTrue(entity1 == entity2);
            Assert.IsFalse(entity1 != entity2);
        }

        [Test]
        public void Entity_Equality_WithDifferentId_ShouldNotBeEqual()
        {
            // Arrange
            var entity1 = new TestEntity(Guid.NewGuid(), "Test 1");
            var entity2 = new TestEntity(Guid.NewGuid(), "Test 2");

            // Act & Assert
            Assert.IsFalse(entity1.Equals(entity2));
            Assert.IsFalse(entity1 == entity2);
            Assert.IsTrue(entity1 != entity2);
        }

        [Test]
        public void Entity_Equality_WithNull_ShouldNotBeEqual()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid(), "Test");

            // Act & Assert
            Assert.IsFalse(entity.Equals(null));
            Assert.IsFalse(entity == null);
            Assert.IsTrue(entity != null);
        }

        [Test]
        public void Entity_GetHashCode_WithSameId_ShouldBeEqual()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity1 = new TestEntity(id, "Test 1");
            var entity2 = new TestEntity(id, "Test 2");

            // Act
            var hashCode1 = entity1.GetHashCode();
            var hashCode2 = entity2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCode1, hashCode2);
        }

        [Test]
        public void Entity_AddDomainEvent_ShouldAddEvent()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid(), "Test");

            // Act
            entity.RaiseTestEvent();

            // Assert
            Assert.AreEqual(1, entity.DomainEvents.Count);
            Assert.IsInstanceOf<TestDomainEvent>(entity.DomainEvents[0]);
        }

        [Test]
        public void Entity_ClearDomainEvents_ShouldRemoveAllEvents()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid(), "Test");
            entity.RaiseTestEvent();
            entity.RaiseTestEvent();

            // Act
            entity.ClearDomainEvents();

            // Assert
            Assert.AreEqual(0, entity.DomainEvents.Count);
        }

        [Test]
        public void Entity_ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new TestEntity(id, "Test");

            // Act
            var str = entity.ToString();

            // Assert
            Assert.AreEqual($"TestEntity[Id={id}]", str);
        }

        [Test]
        public void Entity_WithGuidId_ShouldUseGuidEntity()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var entity = new TestEntity(id, "Test");

            // Assert
            Assert.IsInstanceOf<Entity<Guid>>(entity);
            Assert.AreEqual(id, entity.Id);
        }

        [Test]
        public void DomainEvent_OccurredOn_ShouldBeSet()
        {
            // Arrange
            var beforeCreation = DateTime.UtcNow;

            // Act
            var domainEvent = new TestDomainEvent { Message = "Test" };
            var afterCreation = DateTime.UtcNow;

            // Assert
            Assert.GreaterOrEqual(domainEvent.OccurredOn, beforeCreation);
            Assert.LessOrEqual(domainEvent.OccurredOn, afterCreation);
        }
    }
}

