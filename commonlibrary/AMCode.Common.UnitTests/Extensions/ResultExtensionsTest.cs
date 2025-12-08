using System;
using System.Collections.Generic;
using System.Linq;
using AMCode.Common.Extensions;
using AMCode.Common.Models;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Extensions
{
    [TestFixture]
    public class ResultExtensionsTest
    {
        [Test]
        public void Combine_WithAllSuccess_ShouldReturnSuccess()
        {
            // Arrange
            var results = new[]
            {
                Result<int>.Success(1),
                Result<int>.Success(2),
                Result<int>.Success(3)
            };

            // Act
            var combined = results.Combine();

            // Assert
            Assert.IsTrue(combined.IsSuccess);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, combined.Value);
        }

        [Test]
        public void Combine_WithAnyFailure_ShouldReturnFailure()
        {
            // Arrange
            var results = new[]
            {
                Result<int>.Success(1),
                Result<int>.Failure("Error 1"),
                Result<int>.Success(3)
            };

            // Act
            var combined = results.Combine();

            // Assert
            Assert.IsFalse(combined.IsSuccess);
            Assert.Contains("Error 1", combined.Errors.ToList());
        }

        [Test]
        public void Combine_WithMultipleFailures_ShouldReturnAllErrors()
        {
            // Arrange
            var results = new[]
            {
                Result<int>.Failure("Error 1"),
                Result<int>.Failure("Error 2"),
                Result<int>.Failure("Error 3")
            };

            // Act
            var combined = results.Combine();

            // Assert
            Assert.IsFalse(combined.IsSuccess);
            Assert.AreEqual(3, combined.Errors.Count);
            Assert.Contains("Error 1", combined.Errors);
            Assert.Contains("Error 2", combined.Errors);
            Assert.Contains("Error 3", combined.Errors);
        }

        [Test]
        public void Combine_NonGeneric_WithAllSuccess_ShouldReturnSuccess()
        {
            // Arrange
            var results = new[]
            {
                Result.Success(),
                Result.Success(),
                Result.Success()
            };

            // Act
            var combined = results.Combine();

            // Assert
            Assert.IsTrue(combined.IsSuccess);
        }

        [Test]
        public void Combine_NonGeneric_WithAnyFailure_ShouldReturnFailure()
        {
            // Arrange
            var results = new[]
            {
                Result.Success(),
                Result.Failure("Error 1"),
                Result.Success()
            };

            // Act
            var combined = results.Combine();

            // Assert
            Assert.IsFalse(combined.IsSuccess);
            Assert.Contains("Error 1", combined.Errors.ToList());
        }

        [Test]
        public void Tap_OnSuccess_ShouldExecuteAction()
        {
            // Arrange
            var result = Result<int>.Success(42);
            var executed = false;
            var capturedValue = 0;

            // Act
            var tapped = result.Tap(value =>
            {
                executed = true;
                capturedValue = value;
            });

            // Assert
            Assert.IsTrue(executed);
            Assert.AreEqual(42, capturedValue);
            Assert.AreEqual(result, tapped);
        }

        [Test]
        public void Tap_OnFailure_ShouldNotExecuteAction()
        {
            // Arrange
            var result = Result<int>.Failure("Error");
            var executed = false;

            // Act
            var tapped = result.Tap(value => executed = true);

            // Assert
            Assert.IsFalse(executed);
            Assert.AreEqual(result, tapped);
        }

        [Test]
        public void Tap_NonGeneric_OnSuccess_ShouldExecuteAction()
        {
            // Arrange
            var result = Result.Success();
            var executed = false;

            // Act
            var tapped = result.Tap(() => executed = true);

            // Assert
            Assert.IsTrue(executed);
            Assert.AreEqual(result, tapped);
        }

        [Test]
        public void ToNullable_OnSuccess_WithClass_ShouldReturnValue()
        {
            // Arrange
            var result = Result<string>.Success("Test");

            // Act
            var nullable = result.ToNullable();

            // Assert
            Assert.AreEqual("Test", nullable);
        }

        [Test]
        public void ToNullable_OnFailure_WithClass_ShouldReturnNull()
        {
            // Arrange
            var result = Result<string>.Failure("Error");

            // Act
            var nullable = result.ToNullable();

            // Assert
            Assert.IsNull(nullable);
        }

        [Test]
        public void ToNullableValueType_OnSuccess_ShouldReturnValue()
        {
            // Arrange
            var result = Result<int>.Success(42);

            // Act
            var nullable = result.ToNullableValueType();

            // Assert
            Assert.AreEqual(42, nullable);
        }

        [Test]
        public void ToNullableValueType_OnFailure_ShouldReturnNull()
        {
            // Arrange
            var result = Result<int>.Failure("Error");

            // Act
            var nullable = result.ToNullableValueType();

            // Assert
            Assert.IsNull(nullable);
        }

        [Test]
        public void ToTuple_Generic_ShouldReturnTuple()
        {
            // Arrange
            var result = Result<int>.Success(42);

            // Act
            var tuple = result.ToTuple();

            // Assert
            Assert.IsTrue(tuple.IsSuccess);
            Assert.AreEqual(42, tuple.Value);
            Assert.IsEmpty(tuple.Error);
        }

        [Test]
        public void ToTuple_Generic_OnFailure_ShouldReturnFailureTuple()
        {
            // Arrange
            var result = Result<int>.Failure("Error");

            // Act
            var tuple = result.ToTuple();

            // Assert
            Assert.IsFalse(tuple.IsSuccess);
            Assert.AreEqual(default(int), tuple.Value);
            Assert.AreEqual("Error", tuple.Error);
        }

        [Test]
        public void ToTuple_NonGeneric_ShouldReturnTuple()
        {
            // Arrange
            var result = Result.Success();

            // Act
            var tuple = result.ToTuple();

            // Assert
            Assert.IsTrue(tuple.IsSuccess);
            Assert.IsEmpty(tuple.Error);
        }

        [Test]
        public void ToTuple_NonGeneric_OnFailure_ShouldReturnFailureTuple()
        {
            // Arrange
            var result = Result.Failure("Error");

            // Act
            var tuple = result.ToTuple();

            // Assert
            Assert.IsFalse(tuple.IsSuccess);
            Assert.AreEqual("Error", tuple.Error);
        }
    }
}

