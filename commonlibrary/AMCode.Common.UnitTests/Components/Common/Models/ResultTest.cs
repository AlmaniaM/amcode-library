using System;
using System.Collections.Generic;
using AMCode.Common.Models;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Components.Common.Models
{
    [TestFixture]
    public class ResultTest
    {
        [Test]
        public void Success_ShouldCreateSuccessfulResult()
        {
            // Arrange & Act
            var result = Result<int>.Success(42);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(42, result.Value);
            Assert.IsEmpty(result.Error);
            Assert.IsEmpty(result.Errors);
        }

        [Test]
        public void Failure_WithSingleError_ShouldCreateFailedResult()
        {
            // Arrange & Act
            var result = Result<int>.Failure("Error occurred");

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("Error occurred", result.Error);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.Contains("Error occurred", result.Errors);
        }

        [Test]
        public void Failure_WithMultipleErrors_ShouldCreateFailedResult()
        {
            // Arrange
            var errors = new List<string> { "Error 1", "Error 2", "Error 3" };

            // Act
            var result = Result<int>.Failure(errors);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("Error 1", result.Error);
            Assert.AreEqual(3, result.Errors.Count);
            CollectionAssert.AreEqual(errors, result.Errors);
        }

        [Test]
        public void Failure_WithPrimaryAndAdditionalErrors_ShouldCreateFailedResult()
        {
            // Arrange & Act
            var result = Result<int>.Failure("Primary error", "Additional 1", "Additional 2");

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("Primary error", result.Error);
            Assert.AreEqual(3, result.Errors.Count);
            Assert.Contains("Primary error", result.Errors);
            Assert.Contains("Additional 1", result.Errors);
            Assert.Contains("Additional 2", result.Errors);
        }

        [Test]
        public void ImplicitConversion_FromValue_ShouldCreateSuccessfulResult()
        {
            // Arrange & Act
            Result<int> result = 42;

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void ImplicitConversion_FromErrorString_ShouldCreateFailedResult()
        {
            // Arrange & Act
            Result<int> result = "Error occurred";

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Error occurred", result.Error);
        }

        [Test]
        public void Map_OnSuccess_ShouldMapValue()
        {
            // Arrange
            var result = Result<int>.Success(42);

            // Act
            var mapped = result.Map(x => x.ToString());

            // Assert
            Assert.IsTrue(mapped.IsSuccess);
            Assert.AreEqual("42", mapped.Value);
        }

        [Test]
        public void Map_OnFailure_ShouldPreserveError()
        {
            // Arrange
            var result = Result<int>.Failure("Error occurred");

            // Act
            var mapped = result.Map(x => x.ToString());

            // Assert
            Assert.IsFalse(mapped.IsSuccess);
            Assert.AreEqual("Error occurred", mapped.Error);
        }

        [Test]
        public void Bind_OnSuccess_ShouldBindToNewResult()
        {
            // Arrange
            var result = Result<int>.Success(42);

            // Act
            var bound = result.Bind(x => Result<string>.Success(x.ToString()));

            // Assert
            Assert.IsTrue(bound.IsSuccess);
            Assert.AreEqual("42", bound.Value);
        }

        [Test]
        public void Bind_OnFailure_ShouldPreserveError()
        {
            // Arrange
            var result = Result<int>.Failure("Error occurred");

            // Act
            var bound = result.Bind(x => Result<string>.Success(x.ToString()));

            // Assert
            Assert.IsFalse(bound.IsSuccess);
            Assert.AreEqual("Error occurred", bound.Error);
        }

        [Test]
        public void OnSuccess_ShouldExecuteAction()
        {
            // Arrange
            var result = Result<int>.Success(42);
            var executed = false;
            var capturedValue = 0;

            // Act
            result.OnSuccess(value =>
            {
                executed = true;
                capturedValue = value;
            });

            // Assert
            Assert.IsTrue(executed);
            Assert.AreEqual(42, capturedValue);
        }

        [Test]
        public void OnSuccess_OnFailure_ShouldNotExecuteAction()
        {
            // Arrange
            var result = Result<int>.Failure("Error");
            var executed = false;

            // Act
            result.OnSuccess(value => executed = true);

            // Assert
            Assert.IsFalse(executed);
        }

        [Test]
        public void OnFailure_ShouldExecuteAction()
        {
            // Arrange
            var result = Result<int>.Failure("Error occurred");
            var executed = false;
            var capturedError = string.Empty;

            // Act
            result.OnFailure(error =>
            {
                executed = true;
                capturedError = error;
            });

            // Assert
            Assert.IsTrue(executed);
            Assert.AreEqual("Error occurred", capturedError);
        }

        [Test]
        public void OnFailure_OnSuccess_ShouldNotExecuteAction()
        {
            // Arrange
            var result = Result<int>.Success(42);
            var executed = false;

            // Act
            result.OnFailure(error => executed = true);

            // Assert
            Assert.IsFalse(executed);
        }

        [Test]
        public void GetValueOrDefault_OnSuccess_ShouldReturnValue()
        {
            // Arrange
            var result = Result<int>.Success(42);

            // Act
            var value = result.GetValueOrDefault();

            // Assert
            Assert.AreEqual(42, value);
        }

        [Test]
        public void GetValueOrDefault_OnFailure_ShouldReturnDefault()
        {
            // Arrange
            var result = Result<int>.Failure("Error");

            // Act
            var value = result.GetValueOrDefault();

            // Assert
            Assert.AreEqual(default(int), value);
        }

        [Test]
        public void GetValueOrDefault_OnFailure_WithDefaultValue_ShouldReturnDefault()
        {
            // Arrange
            var result = Result<int>.Failure("Error");

            // Act
            var value = result.GetValueOrDefault(100);

            // Assert
            Assert.AreEqual(100, value);
        }

        [Test]
        public void GetValueOrThrow_OnSuccess_ShouldReturnValue()
        {
            // Arrange
            var result = Result<int>.Success(42);

            // Act
            var value = result.GetValueOrThrow();

            // Assert
            Assert.AreEqual(42, value);
        }

        [Test]
        public void GetValueOrThrow_OnFailure_ShouldThrowException()
        {
            // Arrange
            var result = Result<int>.Failure("Error occurred");

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => result.GetValueOrThrow());
            Assert.AreEqual("Cannot get value from failed result: Error occurred", ex.Message);
        }

        [Test]
        public void NonGenericResult_Success_ShouldCreateSuccessfulResult()
        {
            // Arrange & Act
            var result = Result.Success();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.IsEmpty(result.Error);
            Assert.IsEmpty(result.Errors);
        }

        [Test]
        public void NonGenericResult_Failure_ShouldCreateFailedResult()
        {
            // Arrange & Act
            var result = Result.Failure("Error occurred");

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("Error occurred", result.Error);
        }

        [Test]
        public void NonGenericResult_ImplicitConversion_FromErrorString_ShouldCreateFailedResult()
        {
            // Arrange & Act
            Result result = "Error occurred";

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Error occurred", result.Error);
        }

        [Test]
        public void NonGenericResult_Map_ShouldMapToGenericResult()
        {
            // Arrange
            var result = Result.Success();

            // Act
            var mapped = result.Map(() => 42);

            // Assert
            Assert.IsTrue(mapped.IsSuccess);
            Assert.AreEqual(42, mapped.Value);
        }

        [Test]
        public void ToString_OnSuccess_ShouldReturnSuccessString()
        {
            // Arrange
            var result = Result<int>.Success(42);

            // Act
            var str = result.ToString();

            // Assert
            Assert.AreEqual("Success: 42", str);
        }

        [Test]
        public void ToString_OnFailure_ShouldReturnFailureString()
        {
            // Arrange
            var result = Result<int>.Failure("Error occurred");

            // Act
            var str = result.ToString();

            // Assert
            Assert.AreEqual("Failure: Error occurred", str);
        }
    }
}

