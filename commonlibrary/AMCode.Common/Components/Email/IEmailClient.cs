using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace AMCode.Common.Email
{
    /// <summary>
    /// An interface designed for sending email messages.
    /// </summary>
    public interface IEmailClient
    {
        /// <summary>
        /// Send a <see cref="MailMessage"/>.
        /// </summary>
        /// <param name="message">An instance of the <see cref="MailMessage"/> class.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> for canceling the request.</param>
        /// <returns>A void <see cref="Task"/>/.</returns>
        Task SendMessageAsync(MailMessage message, CancellationToken cancellationToken = default);
    }
}