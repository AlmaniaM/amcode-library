using System;
using AMCode.Common.Domain;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Domain
{
    [TestFixture]
    public class DomainExceptionTest
    {
        [Test]
        public void DomainException_WithMessage_ShouldSetMessage()
        {
            // Arrange & Act
            var exception = new BusinessRuleViolationException("Test message");

            // Assert
            Assert.AreEqual("Test message", exception.Message);
            Assert.IsInstanceOf<DomainException>(exception);
        }

        [Test]
        public void DomainException_WithMessageAndInnerException_ShouldSetBoth()
        {
            // Arrange
            var innerException = new InvalidOperationException("Inner message");

            // Act
            var exception = new BusinessRuleViolationException("Test message", innerException);

            // Assert
            Assert.AreEqual("Test message", exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }

        [Test]
        public void BusinessRuleViolationException_ShouldInheritFromDomainException()
        {
            // Arrange & Act
            var exception = new BusinessRuleViolationException("Test");

            // Assert
            Assert.IsInstanceOf<DomainException>(exception);
        }

        [Test]
        public void EntityNotFoundException_ShouldInheritFromDomainException()
        {
            // Arrange & Act
            var exception = new EntityNotFoundException("Test");

            // Assert
            Assert.IsInstanceOf<DomainException>(exception);
        }

        [Test]
        public void InvalidEntityStateException_ShouldInheritFromDomainException()
        {
            // Arrange & Act
            var exception = new InvalidEntityStateException("Test");

            // Assert
            Assert.IsInstanceOf<DomainException>(exception);
        }

        [Test]
        public void ValidationException_ShouldInheritFromDomainException()
        {
            // Arrange & Act
            var exception = new ValidationException("Test");

            // Assert
            Assert.IsInstanceOf<DomainException>(exception);
        }

        [Test]
        public void AllDomainExceptions_ShouldBeThrowable()
        {
            // Arrange & Act & Assert
            Assert.Throws<BusinessRuleViolationException>(() => throw new BusinessRuleViolationException("Test"));
            Assert.Throws<EntityNotFoundException>(() => throw new EntityNotFoundException("Test"));
            Assert.Throws<InvalidEntityStateException>(() => throw new InvalidEntityStateException("Test"));
            Assert.Throws<ValidationException>(() => throw new ValidationException("Test"));
        }
    }
}

