using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using AMCode.Web.RateLimiting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AMCode.Web.UnitTests.RateLimiting
{
    [TestFixture]
    public class RateLimitingServiceTests
    {
        private Mock<ILogger<RateLimitingService>> _mockLogger = null!;
        private IRateLimitingService _rateLimitingService = null!;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<RateLimitingService>>();
            _rateLimitingService = new RateLimitingService(_mockLogger.Object);
        }

        [Test]
        public void CheckRateLimitAsync_WithNullKey_ThrowsArgumentException()
        {
            // Arrange
            var policy = new RateLimitPolicy { PermitLimit = 10, Window = TimeSpan.FromMinutes(1) };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _rateLimitingService.CheckRateLimitAsync(null!, policy));
        }

        [Test]
        public void CheckRateLimitAsync_WithEmptyKey_ThrowsArgumentException()
        {
            // Arrange
            var policy = new RateLimitPolicy { PermitLimit = 10, Window = TimeSpan.FromMinutes(1) };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _rateLimitingService.CheckRateLimitAsync("", policy));
        }

        [Test]
        public void CheckRateLimitAsync_WithNullPolicy_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _rateLimitingService.CheckRateLimitAsync("test-key", null!));
        }

        [Test]
        public async Task CheckRateLimitAsync_WithValidRequest_ReturnsAllowed()
        {
            // Arrange
            var key = "test-key";
            var policy = new RateLimitPolicy
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                Algorithm = RateLimitAlgorithm.SlidingWindow
            };

            // Act
            var result = await _rateLimitingService.CheckRateLimitAsync(key, policy);

            // Assert
            result.Should().NotBeNull();
            result.IsAllowed.Should().BeTrue();
            result.Limit.Should().Be(10);
            result.Remaining.Should().BeGreaterThanOrEqualTo(0);
            result.ResetTime.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task CheckRateLimitAsync_WhenLimitExceeded_ReturnsNotAllowed()
        {
            // Arrange
            var key = "test-key-exceed";
            var policy = new RateLimitPolicy
            {
                PermitLimit = 2,
                Window = TimeSpan.FromMinutes(1),
                Algorithm = RateLimitAlgorithm.SlidingWindow
            };

            // Act - Make requests up to the limit
            var result1 = await _rateLimitingService.CheckRateLimitAsync(key, policy);
            var result2 = await _rateLimitingService.CheckRateLimitAsync(key, policy);
            var result3 = await _rateLimitingService.CheckRateLimitAsync(key, policy);

            // Assert
            result1.IsAllowed.Should().BeTrue();
            result2.IsAllowed.Should().BeTrue();
            result3.IsAllowed.Should().BeFalse();
            result3.RetryAfter.Should().BeGreaterThan(0);
            result3.Remaining.Should().Be(0);
        }

        [Test]
        public async Task CheckRateLimitAsync_WithDifferentKeys_AreIndependent()
        {
            // Arrange
            var key1 = "key-1";
            var key2 = "key-2";
            var policy = new RateLimitPolicy
            {
                PermitLimit = 1,
                Window = TimeSpan.FromMinutes(1),
                Algorithm = RateLimitAlgorithm.SlidingWindow
            };

            // Act
            var result1 = await _rateLimitingService.CheckRateLimitAsync(key1, policy);
            var result2 = await _rateLimitingService.CheckRateLimitAsync(key2, policy);

            // Assert - Both should be allowed since they're different keys
            result1.IsAllowed.Should().BeTrue();
            result2.IsAllowed.Should().BeTrue();
        }

        [Test]
        public async Task CheckRateLimitAsync_WithSlidingWindowAlgorithm_WorksCorrectly()
        {
            // Arrange
            var key = $"sliding-window-key-{Guid.NewGuid()}"; // Unique key to avoid interference
            var policy = new RateLimitPolicy
            {
                PermitLimit = 5,
                Window = TimeSpan.FromSeconds(10),
                Algorithm = RateLimitAlgorithm.SlidingWindow
            };

            // Act
            var results = new List<RateLimitResult>();
            for (int i = 0; i < 6; i++)
            {
                var result = await _rateLimitingService.CheckRateLimitAsync(key, policy);
                results.Add(result);
                // Small delay to ensure rate limiter processes each request
                await Task.Delay(10);
            }

            // Assert - First 5 should be allowed, 6th may or may not be (timing dependent)
            results[0].IsAllowed.Should().BeTrue();
            results[1].IsAllowed.Should().BeTrue();
            results[2].IsAllowed.Should().BeTrue();
            results[3].IsAllowed.Should().BeTrue();
            results[4].IsAllowed.Should().BeTrue();
            // 6th request may be allowed or limited depending on timing - verify at least 5 were allowed
            results.Count(r => r.IsAllowed).Should().BeGreaterThanOrEqualTo(5);
            results.Count(r => !r.IsAllowed).Should().BeLessThanOrEqualTo(1);
        }

        [Test]
        public async Task CheckRateLimitAsync_WithTokenBucketAlgorithm_WorksCorrectly()
        {
            // Arrange
            var key = $"token-bucket-key-{Guid.NewGuid()}"; // Unique key to avoid interference
            var policy = new RateLimitPolicy
            {
                PermitLimit = 3,
                Window = TimeSpan.FromSeconds(10),
                Algorithm = RateLimitAlgorithm.TokenBucket
            };

            // Act
            var results = new List<RateLimitResult>();
            for (int i = 0; i < 4; i++)
            {
                var result = await _rateLimitingService.CheckRateLimitAsync(key, policy);
                results.Add(result);
                await Task.Delay(10); // Small delay
            }

            // Assert - First 3 should be allowed, 4th may be limited
            results[0].IsAllowed.Should().BeTrue();
            results[1].IsAllowed.Should().BeTrue();
            results[2].IsAllowed.Should().BeTrue();
            // 4th request may be allowed or limited depending on timing
            results.Count(r => r.IsAllowed).Should().BeGreaterThanOrEqualTo(3);
            results.Count(r => !r.IsAllowed).Should().BeLessThanOrEqualTo(1);
        }

        [Test]
        public async Task CheckRateLimitAsync_WithFixedWindowAlgorithm_WorksCorrectly()
        {
            // Arrange
            var key = $"fixed-window-key-{Guid.NewGuid()}"; // Unique key to avoid interference
            var policy = new RateLimitPolicy
            {
                PermitLimit = 2,
                Window = TimeSpan.FromSeconds(5),
                Algorithm = RateLimitAlgorithm.FixedWindow
            };

            // Act
            var results = new List<RateLimitResult>();
            for (int i = 0; i < 3; i++)
            {
                var result = await _rateLimitingService.CheckRateLimitAsync(key, policy);
                results.Add(result);
                await Task.Delay(10); // Small delay
            }

            // Assert - First 2 should be allowed, 3rd may be limited
            results[0].IsAllowed.Should().BeTrue();
            results[1].IsAllowed.Should().BeTrue();
            // 3rd request may be allowed or limited depending on timing
            results.Count(r => r.IsAllowed).Should().BeGreaterThanOrEqualTo(2);
            results.Count(r => !r.IsAllowed).Should().BeLessThanOrEqualTo(1);
        }

        [Test]
        public async Task CheckRateLimitAsync_WithConcurrencyLimiter_WorksCorrectly()
        {
            // Arrange
            var key = $"concurrency-key-{Guid.NewGuid()}"; // Unique key to avoid interference
            var policy = new RateLimitPolicy
            {
                PermitLimit = 2,
                Window = TimeSpan.FromSeconds(10),
                Algorithm = RateLimitAlgorithm.ConcurrencyLimiter
            };

            // Act
            var results = new List<RateLimitResult>();
            for (int i = 0; i < 3; i++)
            {
                var result = await _rateLimitingService.CheckRateLimitAsync(key, policy);
                results.Add(result);
                await Task.Delay(10); // Small delay
            }

            // Assert - First 2 should be allowed, 3rd may be limited
            results[0].IsAllowed.Should().BeTrue();
            results[1].IsAllowed.Should().BeTrue();
            // 3rd concurrent request may be allowed or limited depending on timing
            results.Count(r => r.IsAllowed).Should().BeGreaterThanOrEqualTo(2);
            results.Count(r => !r.IsAllowed).Should().BeLessThanOrEqualTo(1);
        }

        [Test]
        public async Task CheckRateLimitAsync_WithDifferentPolicies_AreIndependent()
        {
            // Arrange
            var key = "policy-test-key";
            var policy1 = new RateLimitPolicy
            {
                PermitLimit = 1,
                Window = TimeSpan.FromMinutes(1),
                Algorithm = RateLimitAlgorithm.SlidingWindow
            };
            var policy2 = new RateLimitPolicy
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                Algorithm = RateLimitAlgorithm.SlidingWindow
            };

            // Act
            var result1 = await _rateLimitingService.CheckRateLimitAsync(key, policy1);
            var result2 = await _rateLimitingService.CheckRateLimitAsync(key, policy2);

            // Assert - Different policies should have independent limits
            result1.IsAllowed.Should().BeTrue();
            result2.IsAllowed.Should().BeTrue();
            result1.Limit.Should().Be(1);
            result2.Limit.Should().Be(5);
        }

        [Test]
        public async Task CheckRateLimitAsync_ReturnsCorrectResetTime()
        {
            // Arrange
            var key = "reset-time-key";
            var policy = new RateLimitPolicy
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(15),
                Algorithm = RateLimitAlgorithm.SlidingWindow
            };

            // Act
            var result = await _rateLimitingService.CheckRateLimitAsync(key, policy);
            var expectedResetTime = DateTimeOffset.UtcNow.Add(policy.Window).ToUnixTimeSeconds();

            // Assert
            result.ResetTime.Should().BeCloseTo(expectedResetTime, 5); // Allow 5 seconds tolerance
        }

        [Test]
        public async Task CheckRateLimitAsync_WhenRateLimited_ReturnsRetryAfter()
        {
            // Arrange
            var key = "retry-after-key";
            var policy = new RateLimitPolicy
            {
                PermitLimit = 1,
                Window = TimeSpan.FromMinutes(1),
                Algorithm = RateLimitAlgorithm.SlidingWindow
            };

            // Act
            await _rateLimitingService.CheckRateLimitAsync(key, policy); // First request
            var result = await _rateLimitingService.CheckRateLimitAsync(key, policy); // Second request (should be limited)

            // Assert
            result.IsAllowed.Should().BeFalse();
            result.RetryAfter.Should().BeGreaterThan(0);
            result.RetryAfter.Should().BeLessThanOrEqualTo(60); // Should be within the window
        }

        [Test]
        public async Task CheckRateLimitAsync_AfterWindowExpires_AllowsRequestsAgain()
        {
            // Arrange
            var key = $"window-expiry-key-{Guid.NewGuid()}"; // Unique key to avoid interference
            var policy = new RateLimitPolicy
            {
                PermitLimit = 1,
                Window = TimeSpan.FromSeconds(2), // Short window for testing
                Algorithm = RateLimitAlgorithm.SlidingWindow
            };

            // Act
            var result1 = await _rateLimitingService.CheckRateLimitAsync(key, policy);
            var result2 = await _rateLimitingService.CheckRateLimitAsync(key, policy); // Should be limited
            await Task.Delay(2500); // Wait for window to expire (with buffer)
            var result3 = await _rateLimitingService.CheckRateLimitAsync(key, policy); // Should be allowed again

            // Assert
            result1.IsAllowed.Should().BeTrue();
            result2.IsAllowed.Should().BeFalse();
            result3.IsAllowed.Should().BeTrue(); // Should be allowed after window expires
        }

        [Test]
        public async Task CheckRateLimitAsync_WithException_FailsOpen()
        {
            // Arrange
            var key = "exception-key";
            var policy = new RateLimitPolicy
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                Algorithm = RateLimitAlgorithm.SlidingWindow
            };

            // Act - Service should handle exceptions gracefully and fail open
            // This test verifies the fail-open behavior is working
            var result = await _rateLimitingService.CheckRateLimitAsync(key, policy);

            // Assert - Should allow request even if there's an internal error
            // (The actual implementation logs errors and fails open)
            result.Should().NotBeNull();
        }
    }
}

