using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AMCode.Common.Extensions.Streams;
using AMCode.Common.Extensions.Strings;
using AMCode.Common.Util;

namespace AMCode.Common.Email
{
    /// <summary>
    /// A class designed to send emails using the Mailgun service.
    /// </summary>
    public class MailGunClient : IEmailClient
    {
        private readonly string apiKey;
        private readonly string domain;
        private readonly HttpClient httpClient;
        private readonly string httpRoute = "https://api.mailgun.net/v3/";

        /// <summary>
        /// Create an instance of the <see cref="MailGunClient"/> class.
        /// </summary>
        /// <param name="httpClient">Provide an instance of a <see cref="HttpClient"/> class.</param>
        /// <param name="apiKey">Provide a string Mailgun API key.</param>
        /// <param name="domain">Provide the Mailgun domain to use.</param>
        public MailGunClient(HttpClient httpClient, string apiKey, string domain)
        {
            string getConstructorHeader(string argumentName)
                => $"{ExceptionUtil.CreateConstructorExceptionHeader<MailGunClient, HttpClient, string, string>()} Error: The provided argument \"{argumentName}\" is not valid.";

            this.httpClient = httpClient ?? throw new ArgumentNullException(getConstructorHeader(nameof(httpClient)));
            this.apiKey = !apiKey.IsNullEmptyOrWhiteSpace() ? apiKey : throw new ArgumentNullException(getConstructorHeader(nameof(apiKey)));
            this.domain = !domain.IsNullEmptyOrWhiteSpace() ? domain : throw new ArgumentNullException(getConstructorHeader(nameof(domain)));
        }

        /// <summary>
        /// Create an instance of the <see cref="MailGunClient"/> class.
        /// </summary>
        /// <param name="httpClient">Provide an instance of a <see cref="HttpClient"/> class.</param>
        /// <param name="httpRoute">Provide the Mailgun api route to use. This must be a complete route such as {https://api.mailgun.net/v3/}.</param>
        /// <param name="apiKey">Provide a string Mailgun API key.</param>
        /// <param name="domain">Provide the Mailgun domain to use.</param>
        public MailGunClient(HttpClient httpClient, string httpRoute, string apiKey, string domain)
            : this(httpClient, apiKey, domain)
        {
            string getConstructorHeader(string argumentName)
                => $"{ExceptionUtil.CreateConstructorExceptionHeader<MailGunClient, HttpClient, string, string, string>()} Error: The provided argument \"{argumentName}\" is not valid.";

            if (httpRoute.IsNullEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(getConstructorHeader(nameof(httpRoute)));
            }

            this.httpRoute = !httpRoute.Last().Equals("/") ? $"{httpRoute}/" : httpRoute;
        }

        /// <summary>
        /// Get the auth token for a Basic authentication scheme.
        /// </summary>
        private string authToken
        {
            get
            {
                var basicAuthByteArray = Encoding.ASCII.GetBytes($"api:{apiKey}");
                return Convert.ToBase64String(basicAuthByteArray);
            }
        }

        /// <summary>
        /// Send an email using Mailgun with the provided <see cref="MailMessage"/> object.
        /// </summary>
        /// <param name="message">An instance of a <see cref="MailMessage"/> class.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> for canceling the request.</param>
        /// <returns>A void <see cref="Task"/>.</returns>
        public async Task SendMessageAsync(MailMessage message, CancellationToken cancellationToken = default)
        {
            validateMessage(message, () => ExceptionUtil.CreateExceptionHeader<MailMessage, CancellationToken, Task>(SendMessageAsync));

            var request = new HttpRequestMessage(HttpMethod.Post, createUrl());
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var form = createFormContent(message);

            if (message.Attachments != null && message.Attachments.Count > 0)
            {
                foreach (var attachment in message.Attachments)
                {
                    form.Add(new ByteArrayContent(await attachment.ContentStream.ToByteArrayAsync()), "attachment", attachment.Name);
                }
            }

            request.Content = form;

            await httpClient.SendAsync(request, cancellationToken);
        }

        private MultipartFormDataContent createFormContent(MailMessage message)
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent(message.From.Address), "from" },
                { new StringContent(string.Join(",", message.To.Select(address => address.Address))), "to" },
                { new StringContent(message.Subject), "subject" }
            };

            if (!message.IsBodyHtml)
            {
                form.Add(new StringContent(message.Body), "text");
            }
            else
            {
                form.Add(new StringContent($"<html>{message.Body}</html>"), "html");
            }

            return form;
        }

        /// <summary>
        /// Create the HTTP URL to use for creating an email request. This will contain all necessary
        /// query parameters within the URL.
        /// </summary>
        /// <returns>A URL <see cref="string"/>.</returns>
        private string createUrl()
        {
            var builder = new StringBuilder()
                .Append(httpRoute)
                .Append(domain)
                .Append("/messages");

            return builder.ToString();
        }

        /// <summary>
        /// Validate that the necessary arguments have been supplied to the <see cref="MailMessage"/> object.
        /// </summary>
        /// <param name="message">The <see cref="MailMessage"/> being sent.</param>
        /// <param name="createHeader">Provide a <see cref="Func{TResult}"/> that returns <see cref="string"/> representing the
        /// exception header you wish to provide to the exception.</param>
        /// <exception cref="ArgumentNullException">Thrown whenever an argument is null.</exception>
        /// <exception cref="ArgumentException">Thrown whenever no To addresses have been provided.</exception>
        private void validateMessage(MailMessage message, Func<string> createHeader)
        {
            string header() => $"{createHeader()} Error:";

            if (message == null)
            {
                throw new ArgumentNullException($"{header()} The provided \"{nameof(message)}\" parameter is not valid.");
            }

            if (message?.From?.Address == null || message.From.Address.IsNullEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException($"{header()} The provided \"From\" address is not valid.");
            }

            if (message.To == null)
            {
                throw new ArgumentNullException($"{header()} The provided \"To\" address is not valid.");
            }

            if (message.To.Count == 0)
            {
                throw new ArgumentException($"{header()} The provided \"To\" address collection has no destination addresses.");
            }
        }
    }
}