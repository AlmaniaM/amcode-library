# Email

**Location:** `AMCode.Common/Components/Email/`  
**Last Updated:** 2025-01-27  
**Purpose:** Email client interface and Mailgun implementation for sending email messages

---

## Overview

The Email component provides email sending functionality through an interface-based design. It includes an IEmailClient interface for abstraction and a MailGunClient implementation that uses the Mailgun API service. The component supports both plain text and HTML emails, attachments, multiple recipients, and async operations with cancellation token support.

## Responsibilities

- **Email Interface**: Define email client interface for abstraction and testability
- **Mailgun Integration**: Implement email sending via Mailgun API
- **Message Support**: Support plain text, HTML, and attachments
- **Async Operations**: Provide async email sending with cancellation support
- **Validation**: Validate email messages before sending

## Class Catalog

### Interfaces

#### IEmailClient

**File:** `IEmailClient.cs`

**Purpose:** Interface for sending email messages, providing abstraction for different email service implementations.

**Key Members:**
```csharp
public interface IEmailClient
{
    Task SendMessageAsync(MailMessage message, CancellationToken cancellationToken = default);
}
```

**Usage:**
```csharp
IEmailClient emailClient = new MailGunClient(httpClient, apiKey, domain);
var message = new MailMessage("from@example.com", "to@example.com", "Subject", "Body");
await emailClient.SendMessageAsync(message);
```

**Implementations:**
- [MailGunClient](#mailgunclient) - Mailgun API implementation

---

### Classes

#### MailGunClient

**File:** `MailGunClient.cs`

**Purpose:** Implementation of IEmailClient that sends emails using the Mailgun API service. Supports plain text, HTML, attachments, and multiple recipients.

**Key Responsibilities:**
- Send emails via Mailgun API
- Support plain text and HTML email bodies
- Handle email attachments
- Support multiple recipients
- Validate email messages before sending
- Provide async operations with cancellation support

**Key Members:**
```csharp
public class MailGunClient : IEmailClient
{
    public MailGunClient(HttpClient httpClient, string apiKey, string domain);
    public MailGunClient(HttpClient httpClient, string httpRoute, string apiKey, string domain);
    public async Task SendMessageAsync(MailMessage message, CancellationToken cancellationToken = default);
}
```

**Usage:**
```csharp
using (var httpClient = new HttpClient())
{
    var emailClient = new MailGunClient(
        httpClient, 
        apiKey: "your-api-key",
        domain: "your-domain.com"
    );

    var message = new MailMessage
    {
        From = new MailAddress("sender@example.com"),
        Subject = "Test Email",
        Body = "This is a test email",
        IsBodyHtml = false
    };
    message.To.Add("recipient@example.com");

    await emailClient.SendMessageAsync(message);
}
```

**Dependencies:**
- `AMCode.Common.Extensions.Streams` - For stream extensions used in attachment handling
- `AMCode.Common.Extensions.Strings` - For string validation
- `AMCode.Common.Util` - For exception utilities
- `System.Net.Http` - For HTTP client operations
- `System.Net.Mail` - For MailMessage class

**Related Components:**
- [IEmailClient](#iemailclient) - Interface definition
- [Extensions](../Extensions/README.md) - Uses string and stream extensions

---

## Architecture Patterns

- **Interface-Based Design**: IEmailClient interface provides abstraction for different email service implementations
- **Dependency Injection**: HttpClient and configuration passed via constructor
- **Async/Await Pattern**: All operations are asynchronous with cancellation token support
- **Validation Pattern**: Email messages are validated before sending
- **Factory Pattern**: Can be instantiated with different Mailgun API routes

## Usage Patterns

### Pattern 1: Basic Email Sending

```csharp
using (var httpClient = new HttpClient())
{
    var emailClient = new MailGunClient(httpClient, apiKey, domain);
    
    var message = new MailMessage
    {
        From = new MailAddress("sender@example.com"),
        Subject = "Hello",
        Body = "This is a plain text email",
        IsBodyHtml = false
    };
    message.To.Add("recipient@example.com");
    
    await emailClient.SendMessageAsync(message);
}
```

### Pattern 2: HTML Email

```csharp
var message = new MailMessage
{
    From = new MailAddress("sender@example.com"),
    Subject = "HTML Email",
    Body = "<h1>Hello</h1><p>This is an HTML email.</p>",
    IsBodyHtml = true
};
message.To.Add("recipient@example.com");

await emailClient.SendMessageAsync(message);
```

### Pattern 3: Email with Attachments

```csharp
var message = new MailMessage
{
    From = new MailAddress("sender@example.com"),
    Subject = "Email with Attachment",
    Body = "Please find the attached file.",
    IsBodyHtml = false
};
message.To.Add("recipient@example.com");

// Add attachment
var attachment = new Attachment("path/to/file.pdf");
message.Attachments.Add(attachment);

await emailClient.SendMessageAsync(message);
```

### Pattern 4: Multiple Recipients

```csharp
var message = new MailMessage
{
    From = new MailAddress("sender@example.com"),
    Subject = "Multiple Recipients",
    Body = "This email goes to multiple people."
};
message.To.Add("recipient1@example.com");
message.To.Add("recipient2@example.com");
message.CC.Add("cc@example.com");
message.Bcc.Add("bcc@example.com");

await emailClient.SendMessageAsync(message);
```

### Pattern 5: Custom Mailgun Route

```csharp
var emailClient = new MailGunClient(
    httpClient,
    httpRoute: "https://api.eu.mailgun.net/v3/",  // EU region
    apiKey: "your-api-key",
    domain: "your-domain.com"
);
```

### Pattern 6: With Cancellation Token

```csharp
var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

try
{
    await emailClient.SendMessageAsync(message, cancellationToken);
}
catch (OperationCanceledException)
{
    // Email sending was cancelled
}
```

### Pattern 7: Dependency Injection

```csharp
// In Startup.cs or similar
services.AddSingleton<IEmailClient>(provider =>
{
    var httpClient = provider.GetRequiredService<HttpClient>();
    var config = provider.GetRequiredService<IConfiguration>();
    return new MailGunClient(
        httpClient,
        config["Mailgun:ApiKey"],
        config["Mailgun:Domain"]
    );
});

// In service
public class MyService
{
    private readonly IEmailClient _emailClient;
    
    public MyService(IEmailClient emailClient)
    {
        _emailClient = emailClient;
    }
    
    public async Task SendNotificationAsync(string email, string message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress("noreply@example.com"),
            Subject = "Notification",
            Body = message
        };
        mailMessage.To.Add(email);
        
        await _emailClient.SendMessageAsync(mailMessage);
    }
}
```

### Pattern 8: Error Handling

```csharp
try
{
    await emailClient.SendMessageAsync(message);
}
catch (ArgumentNullException ex)
{
    // Handle missing required fields (From, To, etc.)
    Console.WriteLine($"Invalid email message: {ex.Message}");
}
catch (ArgumentException ex)
{
    // Handle invalid email addresses
    Console.WriteLine($"Invalid email address: {ex.Message}");
}
catch (HttpRequestException ex)
{
    // Handle Mailgun API errors
    Console.WriteLine($"Email sending failed: {ex.Message}");
}
```

---

## Dependencies

### Internal Dependencies

- `AMCode.Common.Extensions.Streams` - For stream extensions used in attachment handling
- `AMCode.Common.Extensions.Strings` - For string validation (IsNullEmptyOrWhiteSpace)
- `AMCode.Common.Util` - For exception utility methods

### External Dependencies

- `System.Net.Http` - For HTTP client operations
- `System.Net.Mail` - For MailMessage class
- `System.Web` - For URL encoding (if needed)

---

## Related Components

### Within Same Library

- [Extensions](../Extensions/README.md) - Uses string and stream extensions
- [Util](../Util/README.md) - Uses exception utilities

### In Other Libraries

- None (Email is a standalone component)

---

## Testing

### Test Coverage

- Unit tests: `AMCode.Common.UnitTests/Components/Email/Tests/`
- Integration tests: Not recommended (requires Mailgun API access)

### Example Test

```csharp
[Test]
public async Task MailGunClient_SendMessageAsync_SendsEmail()
{
    // Arrange
    var mockHttpClient = new Mock<HttpClient>();
    var emailClient = new MailGunClient(mockHttpClient.Object, "api-key", "domain.com");
    var message = new MailMessage
    {
        From = new MailAddress("from@example.com"),
        Subject = "Test",
        Body = "Test body"
    };
    message.To.Add("to@example.com");

    // Act
    await emailClient.SendMessageAsync(message);

    // Assert
    // Verify HTTP request was made
}
```

---

## Notes

- **Mailgun API Key**: Requires a valid Mailgun API key and domain
- **HTTP Client**: HttpClient should be reused (don't create new instances for each email)
- **Attachments**: Attachments are converted to byte arrays and sent as multipart form data
- **HTML Emails**: HTML emails are wrapped in `<html>` tags automatically
- **Validation**: Messages are validated before sending (From, To addresses required)
- **Error Handling**: Throws ArgumentNullException for null/invalid parameters, ArgumentException for empty To addresses
- **Cancellation**: Supports cancellation tokens for async operations
- **Custom Routes**: Can specify custom Mailgun API routes (e.g., EU region)
- **Multiple Recipients**: Supports To, CC, and Bcc recipients
- **Basic Authentication**: Uses Basic authentication with API key

---

**See Also:**
- [Library README](../../README.md) - AMCode.Common library overview
- [Extensions README](../Extensions/README.md) - Extension methods used
- [Root README](../../../../README.md) - AMCode library overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

