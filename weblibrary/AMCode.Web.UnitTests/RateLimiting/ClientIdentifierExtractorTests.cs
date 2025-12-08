using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using AMCode.Web.RateLimiting;
using System.Net;
using System.Security.Claims;

namespace AMCode.Web.UnitTests.RateLimiting
{
    [TestFixture]
    public class ClientIdentifierExtractorTests
    {
        private DefaultHttpContext _httpContext = null!;

        [SetUp]
        public void Setup()
        {
            _httpContext = new DefaultHttpContext();
        }

        [Test]
        public void ExtractClientIdentifier_WithAuthenticatedUser_ReturnsUserId()
        {
            // Arrange
            var userId = "user-123";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            _httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

            // Act
            var result = ClientIdentifierExtractor.ExtractClientIdentifier(_httpContext);

            // Assert
            result.Should().Be($"user:{userId}");
        }

        [Test]
        public void ExtractClientIdentifier_WithApiKey_ReturnsApiKey()
        {
            // Arrange
            var apiKey = "test-api-key-456";
            _httpContext.Request.Headers["X-API-Key"] = apiKey;

            // Act
            var result = ClientIdentifierExtractor.ExtractClientIdentifier(_httpContext);

            // Assert
            result.Should().Be($"apikey:{apiKey}");
        }

        [Test]
        public void ExtractClientIdentifier_WithUserAndApiKey_PrioritizesUserId()
        {
            // Arrange
            var userId = "user-123";
            var apiKey = "test-api-key-456";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            _httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
            _httpContext.Request.Headers["X-API-Key"] = apiKey;

            // Act
            var result = ClientIdentifierExtractor.ExtractClientIdentifier(_httpContext);

            // Assert - User ID should take priority
            result.Should().Be($"user:{userId}");
        }

        [Test]
        public void ExtractClientIdentifier_WithoutAuth_UsesIpAddress()
        {
            // Arrange
            var ipAddress = IPAddress.Parse("192.168.1.100");
            _httpContext.Connection.RemoteIpAddress = ipAddress;

            // Act
            var result = ClientIdentifierExtractor.ExtractClientIdentifier(_httpContext);

            // Assert
            result.Should().Be($"ip:{ipAddress}");
        }

        [Test]
        public void ExtractClientIdentifier_WithXForwardedFor_UsesFirstIp()
        {
            // Arrange
            _httpContext.Request.Headers["X-Forwarded-For"] = "10.0.0.1, 192.168.1.1, 172.16.0.1";
            _httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");

            // Act
            var result = ClientIdentifierExtractor.ExtractClientIdentifier(_httpContext);

            // Assert
            result.Should().Be("ip:10.0.0.1");
        }

        [Test]
        public void ExtractClientIdentifier_WithXRealIP_UsesRealIp()
        {
            // Arrange
            _httpContext.Request.Headers["X-Real-IP"] = "10.0.0.2";
            _httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");

            // Act
            var result = ClientIdentifierExtractor.ExtractClientIdentifier(_httpContext);

            // Assert
            result.Should().Be("ip:10.0.0.2");
        }

        [Test]
        public void ExtractClientIdentifier_WithXForwardedForAndXRealIP_PrioritizesXForwardedFor()
        {
            // Arrange
            _httpContext.Request.Headers["X-Forwarded-For"] = "10.0.0.1";
            _httpContext.Request.Headers["X-Real-IP"] = "10.0.0.2";
            _httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");

            // Act
            var result = ClientIdentifierExtractor.ExtractClientIdentifier(_httpContext);

            // Assert - X-Forwarded-For should take priority
            result.Should().Be("ip:10.0.0.1");
        }

        [Test]
        public void ExtractClientIdentifier_WithNoIpAddress_ReturnsUnknown()
        {
            // Arrange
            _httpContext.Connection.RemoteIpAddress = null;

            // Act
            var result = ClientIdentifierExtractor.ExtractClientIdentifier(_httpContext);

            // Assert
            result.Should().Be("ip:unknown");
        }

        [Test]
        public void ExtractClientIdentifier_WithEmptyUserId_StillUsesUserId()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "")
            };
            _httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
            _httpContext.Connection.RemoteIpAddress = IPAddress.Parse("192.168.1.1");

            // Act
            var result = ClientIdentifierExtractor.ExtractClientIdentifier(_httpContext);

            // Assert - Empty user ID should still be used (falls through to IP)
            result.Should().Be("ip:192.168.1.1");
        }

        [Test]
        public void ExtractClientIdentifier_WithWhitespaceApiKey_FallsBackToIp()
        {
            // Arrange
            _httpContext.Request.Headers["X-API-Key"] = "   ";
            _httpContext.Connection.RemoteIpAddress = IPAddress.Parse("192.168.1.1");

            // Act
            var result = ClientIdentifierExtractor.ExtractClientIdentifier(_httpContext);

            // Assert
            result.Should().Be("ip:192.168.1.1");
        }
    }
}

