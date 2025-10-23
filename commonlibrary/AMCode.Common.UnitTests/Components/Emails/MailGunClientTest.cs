using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AMCode.Common.Email;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.Strings;
using AMCode.Common.Util;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Emails.MailGunClientTests
{
    [TestFixture]
    public class MailGunClientTest
    {
        private string exHeader;
        private string exConstructorHeader;
        private string exConstructorHeader2;
        private HttpClient httpClient;
        private Mock<HttpMessageHandler> httpMessageHandlerMock;
        private MailGunClient mailGunClient;

        [SetUp]
        public void SetUp()
        {
            httpMessageHandlerMock = new();
            httpClient = new HttpClient(httpMessageHandlerMock.Object);
            mailGunClient = new MailGunClient(httpClient, "test-key", "test.com");

            exHeader = ExceptionUtil.CreateExceptionHeader<MailMessage, CancellationToken, Task>(mailGunClient.SendMessageAsync);
            exConstructorHeader = ExceptionUtil.CreateConstructorExceptionHeader<MailGunClient, HttpClient, string, string>();
            exConstructorHeader2 = ExceptionUtil.CreateConstructorExceptionHeader<MailGunClient, HttpClient, string, string, string>();
        }

        [TearDown]
        public void TearDown() => httpClient.Dispose();

        [Test]
        public async Task ShouldSetBody()
        {
            setAssert(request =>
            {
                var content = getContentString(request.Content);
                StringAssert.Contains("TestingBody", content);
                StringAssert.Contains("name=text", content);
            });
            var message = new MailMessage("test@test.com", "test2@test.com")
            {
                Subject = "Testing",
                Body = "TestingBody"
            };

            await mailGunClient.SendMessageAsync(message);
        }

        [Test]
        public async Task ShouldSetFromAddress()
        {
            setAssert(request =>
            {
                var content = getContentString(request.Content);
                StringAssert.Contains("test@test.com", content);
                StringAssert.Contains("name=from", content);
            });
            var message = new MailMessage("test@test.com", "test2@test.com")
            {
                Subject = "Testing",
                Body = "Testing"
            };

            await mailGunClient.SendMessageAsync(message);
        }

        [Test]
        public async Task ShouldSetSubject()
        {
            setAssert(request =>
            {
                var content = getContentString(request.Content);
                StringAssert.Contains("TestingSubject", content);
                StringAssert.Contains("name=subject", content);
            });
            var message = new MailMessage("test@test.com", "test2@test.com")
            {
                Subject = "TestingSubject",
                Body = "Testing"
            };

            await mailGunClient.SendMessageAsync(message);
        }

        [Test]
        public async Task ShouldSetHtmlOnly()
        {
            var expectedHtml = "<html>Hello</html>";

            setAssert(request =>
            {
                var content = getContentString(request.Content);
                StringAssert.Contains(expectedHtml, content);
                StringAssert.Contains("name=html", content);
                StringAssert.DoesNotContain("name=text", content);
            });
            var message = new MailMessage("test@test.com", "test2@test.com")
            {
                Subject = "TestingSubject",
                Body = "Hello",
                IsBodyHtml = true,
            };

            await mailGunClient.SendMessageAsync(message);
        }

        [Test]
        public async Task ShouldSanitizeHtml()
        {
            var expectedHtml = "<div style=\"padding-left: 1px; margin: 2px;\">Hello</div>";
            setAssert(request =>
            {
                var content = getContentString(request.Content);
                StringAssert.Contains(expectedHtml, content);
                StringAssert.Contains("name=html", content);
            });
            var message = new MailMessage("test@test.com", "test2@test.com")
            {
                Subject = "TestingSubject",
                Body = expectedHtml,
                IsBodyHtml = true,
            };

            await mailGunClient.SendMessageAsync(message);
        }

        // Have to pass in the strings as an object because NUnit doesn't allow arrays of string as constant values.
        [TestCase(new object[] { "test2@test.com" })]
        [TestCase(new object[] { "test2@test.com", "test3@test.com" })]
        [TestCase(new object[] { "test2@test.com", "test3@test.com", "test4@test.com", "test5@test.com" })]
        public async Task ShouldSetToAddress(object[] addressObjs)
        {
            var addresses = addressObjs.Select(obj => obj.ToString());
            var addressStr = string.Join(",", addresses);
            setAssert(request =>
            {
                var content = getContentString(request.Content);
                StringAssert.Contains(addressStr, content);
                StringAssert.Contains("name=to", content);
            });
            var message = new MailMessage()
            {
                From = new MailAddress("test@test.com"),
                Subject = "subject",
                Body = "body"
            };
            addresses.ForEach(address => message.To.Add(new MailAddress(address)));

            await mailGunClient.SendMessageAsync(message);
        }

        [Test]
        public async Task ShouldSendTwoFiles()
        {
            setAssert(request =>
            {
                var form = (MultipartFormDataContent)request.Content;
                Assert.That(form.Where(content => content.Headers.ContentDisposition.Name.Equals("attachment")).Count(), Is.EqualTo(2));
            });
            var message = new MailMessage("test@test.com", "test2@test.com")
            {
                Subject = "subject",
                Body = "body"
            };

            message.Attachments.Add(new Attachment("Hello".GetStream(), "test.txt", "text/plain"));
            message.Attachments.Add(new Attachment("Hello there!!!".GetStream(), "test.txt", "text/plain"));

            await mailGunClient.SendMessageAsync(message);
        }

        [Test]
        public async Task ShouldSendFileWithCorrectContents()
        {
            setAssert(async request =>
            {
                var form = (MultipartFormDataContent)request.Content;
                var fileContents = await form
                    .Where(content => content.Headers.ContentDisposition.Name.Equals("attachment"))
                    .First()
                    .ReadAsStringAsync();
                Assert.That(fileContents, Is.EqualTo("Hello there!!!"));
            });
            var message = new MailMessage("test@test.com", "test2@test.com")
            {
                Subject = "subject",
                Body = "body"
            };

            message.Attachments.Add(new Attachment("Hello there!!!".GetStream(), "test.txt", "text/plain"));

            await mailGunClient.SendMessageAsync(message);
        }

        [Test]
        public void ShouldFailToInstantiateWithoutHttpClient()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MailGunClient(null, null, null));
            var exMessage = $"{exHeader} Error: The provided argument \"httpClient\" is not valid.";
            StringAssert.Contains(exConstructorHeader, exception.Message);
        }

        [Test]
        public void ShouldFailToInstantiateWithoutApiKey()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MailGunClient(new(httpMessageHandlerMock.Object), null, null));
            var exMessage = $"{exHeader} Error: The provided argument \"apiKey\" is not valid.";
            StringAssert.Contains(exConstructorHeader, exception.Message);
        }

        [Test]
        public void ShouldFailToInstantiateWithoutDomain()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MailGunClient(new(httpMessageHandlerMock.Object), "secret-key", null));
            var exMessage = $"{exHeader} Error: The provided argument \"domain\" is not valid.";
            StringAssert.Contains(exConstructorHeader, exception.Message);
        }

        [Test]
        public void ShouldFailToInstantiateWithoutHttpRoute()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MailGunClient(new(httpMessageHandlerMock.Object), null, "secret-key", "test.com"));
            var exMessage = $"{exHeader} Error: The provided argument \"httpRoute\" is not valid.";
            StringAssert.Contains(exConstructorHeader2, exception.Message);
        }

        [Test]
        public void ShouldFailToSetFromAddress()
        {
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => mailGunClient.SendMessageAsync(new()));
            var exMessage = $"{exHeader} Error: The provided \"From\" address is not valid.";
            StringAssert.Contains(exMessage, exception.Message);
        }

        [Test]
        public void ShouldFailToSetToAddress()
        {
            var message = new MailMessage
            {
                From = new MailAddress("test@test.com")
            };

            var exception = Assert.ThrowsAsync<ArgumentException>(() => mailGunClient.SendMessageAsync(message));
            var exMessage = $"{exHeader} Error: The provided \"To\" address collection has no destination addresses.";
            StringAssert.Contains(exMessage, exception.Message);
        }

        [Test]
        [Ignore("This is only for developers to run when developing to confirm the Mailgun client works. Not for automated test runs.")]
        public async Task ShouldIgnoreRunRealMailGunClientAsync()
        {
            // NOTE: You have to supply the secret API key.
            // !!!!!DO NOT COMMIT THE API KEY TO SOURCE CONTROL!!!!!
            var mailgun = new MailGunClient(new HttpClient(), "<secret-api-key-here>", "alexmolodyh.com");
            var message = new MailMessage("alexandermolodyh9@gmail.com", "alexandermolodyh9@gmail.com")
            {
                Subject = "Testing AMCode.Common",
                Body = "<div style=\"color: green; background: yellow;\">Hello!!!</div>",
                IsBodyHtml = true,
            };

            message.Attachments.Add(new Attachment("Hello".GetStream(), "test.txt", "text/plain"));
            message.Attachments.Add(new Attachment("Hello there!!!".GetStream(), "test.txt", "text/plain"));

            await mailgun.SendMessageAsync(message);
        }

        private string getContentString(HttpContent content)
        {
            var sr = Encoding.UTF8.GetString(content.ReadAsByteArrayAsync().GetAwaiter().GetResult());
            return sr;
        }

        private void setAssert(Action<HttpRequestMessage> assert)
        {
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, _) => assert(request))
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string>()))
                });
        }
    }
}