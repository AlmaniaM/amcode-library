using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using AMCode.Web.Middleware;
using AMCode.Web.RateLimiting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AMCode.Web.UnitTests.Middleware
{
    [TestFixture]
    public class RateLimitingMiddlewareTests
    {
        private Mock<RequestDelegate> _mockNext = null!;
        private Mock<ILogger<RateLimitingMiddleware>> _mockLogger = null!;
        private Mock<IRateLimitingService> _mockRateLimitingService = null!;
        private Mock<IOptions<RateLimitingSettings>> _mockSettings = null!;
        private RateLimitingMiddleware _middleware = null!;
        private DefaultHttpContext _httpContext = null!;

        [SetUp]
        public void Setup()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockLogger = new Mock<ILogger<RateLimitingMiddleware>>();
            _mockRateLimitingService = new Mock<IRateLimitingService>();
            _mockSettings = new Mock<IOptions<RateLimitingSettings>>();

            var defaultSettings = new RateLimitingSettings
            {
                DefaultPolicy = new RateLimitPolicy
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(15),
                    Algorithm = RateLimitAlgorithm.SlidingWindow
                },
                ExcludedPaths = new List<string> { "/health", "/swagger" }
            };

            _mockSettings.Setup(x => x.Value).Returns(defaultSettings);

            _middleware = new RateLimitingMiddleware(
                _mockNext.Object,
                _mockLogger.Object,
                _mockSettings.Object);

            _httpContext = new DefaultHttpContext();
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IRateLimitingService))).Returns(_mockRateLimitingService.Object);
            mockServiceProvider.Setup(x => x.GetRequiredService(typeof(IRateLimitingService))).Returns(_mockRateLimitingService.Object);
            _httpContext.RequestServices = mockServiceProvider.Object;
        }

        [Test]
        public async Task InvokeAsync_WithExcludedPath_SkipsRateLimiting()
        {
            // Arrange
            _httpContext.Request.Path = "/health";
            _mockNext.Setup(x => x(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _mockRateLimitingService.Verify(
                x => x.CheckRateLimitAsync(It.IsAny<string>(), It.IsAny<RateLimitPolicy>(), It.IsAny<CancellationToken>()),
                Times.Never);
            _mockNext.Verify(x => x(_httpContext), Times.Once);
        }

        [Test]
        public async Task InvokeAsync_WithAllowedRequest_ContinuesPipeline()
        {
            // Arrange
            _httpContext.Request.Path = "/api/test";
            _httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.1");

            var rateLimitResult = new RateLimitResult
            {
                IsAllowed = true,
                Limit = 100,
                Remaining = 99,
                ResetTime = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds(),
                RetryAfter = 0
            };

            _mockRateLimitingService
                .Setup(x => x.CheckRateLimitAsync(It.IsAny<string>(), It.IsAny<RateLimitPolicy>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rateLimitResult);

            _mockNext.Setup(x => x(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _mockNext.Verify(x => x(_httpContext), Times.Once);
            _httpContext.Response.Headers["X-RateLimit-Limit"].ToString().Should().Be("100");
            _httpContext.Response.Headers["X-RateLimit-Remaining"].ToString().Should().Be("99");
            _httpContext.Response.Headers["X-RateLimit-Reset"].ToString().Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task InvokeAsync_WithRateLimitedRequest_Returns429()
        {
            // Arrange
            _httpContext.Request.Path = "/api/test";
            _httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.1");

            var rateLimitResult = new RateLimitResult
            {
                IsAllowed = false,
                Limit = 100,
                Remaining = 0,
                ResetTime = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds(),
                RetryAfter = 900
            };

            _mockRateLimitingService
                .Setup(x => x.CheckRateLimitAsync(It.IsAny<string>(), It.IsAny<RateLimitPolicy>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rateLimitResult);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.TooManyRequests);
            _httpContext.Response.ContentType.Should().Be("application/json");
            _httpContext.Response.Headers["Retry-After"].ToString().Should().Be("900");
            _httpContext.Response.Headers["X-RateLimit-Limit"].ToString().Should().Be("100");
            _httpContext.Response.Headers["X-RateLimit-Remaining"].ToString().Should().Be("0");
            _httpContext.Response.Headers["X-RateLimit-Reset"].ToString().Should().NotBeNullOrEmpty();

            _mockNext.Verify(x => x(It.IsAny<HttpContext>()), Times.Never);

            // Verify response body
            _httpContext.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            var reader = new System.IO.StreamReader(_httpContext.Response.Body);
            var responseBody = await reader.ReadToEndAsync();
            responseBody.Should().Contain("Rate limit exceeded");
        }

        [Test]
        public async Task InvokeAsync_WithAuthenticatedUser_UsesUserIdAsKey()
        {
            // Arrange
            _httpContext.Request.Path = "/api/test";
            var userId = "user-123";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            _httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

            var rateLimitResult = new RateLimitResult
            {
                IsAllowed = true,
                Limit = 100,
                Remaining = 99,
                ResetTime = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds()
            };

            _mockRateLimitingService
                .Setup(x => x.CheckRateLimitAsync(It.IsAny<string>(), It.IsAny<RateLimitPolicy>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rateLimitResult);

            _mockNext.Setup(x => x(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _mockRateLimitingService.Verify(
                x => x.CheckRateLimitAsync(
                    It.Is<string>(k => k == $"user:{userId}"),
                    It.IsAny<RateLimitPolicy>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task InvokeAsync_WithApiKey_UsesApiKeyAsKey()
        {
            // Arrange
            _httpContext.Request.Path = "/api/test";
            var apiKey = "test-api-key-123";
            _httpContext.Request.Headers["X-API-Key"] = apiKey;

            var rateLimitResult = new RateLimitResult
            {
                IsAllowed = true,
                Limit = 100,
                Remaining = 99,
                ResetTime = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds()
            };

            _mockRateLimitingService
                .Setup(x => x.CheckRateLimitAsync(It.IsAny<string>(), It.IsAny<RateLimitPolicy>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rateLimitResult);

            _mockNext.Setup(x => x(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _mockRateLimitingService.Verify(
                x => x.CheckRateLimitAsync(
                    It.Is<string>(k => k == $"apikey:{apiKey}"),
                    It.IsAny<RateLimitPolicy>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task InvokeAsync_WithoutAuth_UsesIpAddressAsKey()
        {
            // Arrange
            _httpContext.Request.Path = "/api/test";
            var ipAddress = System.Net.IPAddress.Parse("192.168.1.100");
            _httpContext.Connection.RemoteIpAddress = ipAddress;

            var rateLimitResult = new RateLimitResult
            {
                IsAllowed = true,
                Limit = 100,
                Remaining = 99,
                ResetTime = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds()
            };

            _mockRateLimitingService
                .Setup(x => x.CheckRateLimitAsync(It.IsAny<string>(), It.IsAny<RateLimitPolicy>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rateLimitResult);

            _mockNext.Setup(x => x(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _mockRateLimitingService.Verify(
                x => x.CheckRateLimitAsync(
                    It.Is<string>(k => k == $"ip:{ipAddress}"),
                    It.IsAny<RateLimitPolicy>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task InvokeAsync_WithEndpointOverride_UsesOverridePolicy()
        {
            // Arrange
            _httpContext.Request.Path = "/api/auth/login";
            _httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.1");

            var settings = new RateLimitingSettings
            {
                DefaultPolicy = new RateLimitPolicy
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(15)
                },
                EndpointOverrides = new Dictionary<string, EndpointRateLimitOverride>
                {
                    ["/api/auth/login"] = new EndpointRateLimitOverride
                    {
                        PolicyName = "Strict"
                    }
                },
                Policies = new Dictionary<string, RateLimitPolicy>
                {
                    ["Strict"] = new RateLimitPolicy
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1)
                    }
                }
            };

            _mockSettings.Setup(x => x.Value).Returns(settings);
            _middleware = new RateLimitingMiddleware(
                _mockNext.Object,
                _mockLogger.Object,
                _mockSettings.Object);

            var rateLimitResult = new RateLimitResult
            {
                IsAllowed = true,
                Limit = 5,
                Remaining = 4,
                ResetTime = DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds()
            };

            _mockRateLimitingService
                .Setup(x => x.CheckRateLimitAsync(It.IsAny<string>(), It.IsAny<RateLimitPolicy>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rateLimitResult);

            _mockNext.Setup(x => x(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _mockRateLimitingService.Verify(
                x => x.CheckRateLimitAsync(
                    It.IsAny<string>(),
                    It.Is<RateLimitPolicy>(p => p.PermitLimit == 5 && p.Window == TimeSpan.FromMinutes(1)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task InvokeAsync_WithDirectEndpointOverride_UsesOverrideValues()
        {
            // Arrange
            _httpContext.Request.Path = "/api/test/endpoint";
            _httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.1");

            var settings = new RateLimitingSettings
            {
                DefaultPolicy = new RateLimitPolicy
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(15)
                },
                EndpointOverrides = new Dictionary<string, EndpointRateLimitOverride>
                {
                    ["/api/test/endpoint"] = new EndpointRateLimitOverride
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromHours(1)
                    }
                }
            };

            _mockSettings.Setup(x => x.Value).Returns(settings);
            _middleware = new RateLimitingMiddleware(
                _mockNext.Object,
                _mockLogger.Object,
                _mockSettings.Object);

            var rateLimitResult = new RateLimitResult
            {
                IsAllowed = true,
                Limit = 20,
                Remaining = 19,
                ResetTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()
            };

            _mockRateLimitingService
                .Setup(x => x.CheckRateLimitAsync(It.IsAny<string>(), It.IsAny<RateLimitPolicy>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rateLimitResult);

            _mockNext.Setup(x => x(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _mockRateLimitingService.Verify(
                x => x.CheckRateLimitAsync(
                    It.IsAny<string>(),
                    It.Is<RateLimitPolicy>(p => p.PermitLimit == 20 && p.Window == TimeSpan.FromHours(1)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task InvokeAsync_WithXForwardedForHeader_UsesForwardedIp()
        {
            // Arrange
            _httpContext.Request.Path = "/api/test";
            _httpContext.Request.Headers["X-Forwarded-For"] = "10.0.0.1, 192.168.1.1";
            _httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");

            var rateLimitResult = new RateLimitResult
            {
                IsAllowed = true,
                Limit = 100,
                Remaining = 99,
                ResetTime = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds()
            };

            _mockRateLimitingService
                .Setup(x => x.CheckRateLimitAsync(It.IsAny<string>(), It.IsAny<RateLimitPolicy>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rateLimitResult);

            _mockNext.Setup(x => x(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _mockRateLimitingService.Verify(
                x => x.CheckRateLimitAsync(
                    It.Is<string>(k => k == "ip:10.0.0.1"), // Should use first IP from X-Forwarded-For
                    It.IsAny<RateLimitPolicy>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task InvokeAsync_SetsRateLimitHeaders_OnSuccess()
        {
            // Arrange
            _httpContext.Request.Path = "/api/test";
            _httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.1");

            var rateLimitResult = new RateLimitResult
            {
                IsAllowed = true,
                Limit = 100,
                Remaining = 75,
                ResetTime = 1738000000L,
                RetryAfter = 0
            };

            _mockRateLimitingService
                .Setup(x => x.CheckRateLimitAsync(It.IsAny<string>(), It.IsAny<RateLimitPolicy>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rateLimitResult);

            _mockNext.Setup(x => x(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.Headers["X-RateLimit-Limit"].ToString().Should().Be("100");
            _httpContext.Response.Headers["X-RateLimit-Remaining"].ToString().Should().Be("75");
            _httpContext.Response.Headers["X-RateLimit-Reset"].ToString().Should().Be("1738000000");
        }

        [Test]
        public async Task InvokeAsync_WithRateLimitExceeded_ReturnsJsonError()
        {
            // Arrange
            _httpContext.Request.Path = "/api/test";
            _httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.1");

            var rateLimitResult = new RateLimitResult
            {
                IsAllowed = false,
                Limit = 100,
                Remaining = 0,
                ResetTime = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds(),
                RetryAfter = 900
            };

            _mockRateLimitingService
                .Setup(x => x.CheckRateLimitAsync(It.IsAny<string>(), It.IsAny<RateLimitPolicy>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rateLimitResult);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.TooManyRequests);
            _httpContext.Response.ContentType.Should().Be("application/json");

            _httpContext.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            var reader = new System.IO.StreamReader(_httpContext.Response.Body);
            var responseBody = await reader.ReadToEndAsync();
            var errorResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);

            errorResponse.GetProperty("error").GetString().Should().Be("Rate limit exceeded");
            errorResponse.GetProperty("retryAfter").GetInt32().Should().Be(900);
        }
    }
}

