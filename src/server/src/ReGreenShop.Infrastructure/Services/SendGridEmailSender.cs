using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Models;
using ReGreenShop.Infrastructure.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ReGreenShop.Infrastructure.Services;
public class SendGridEmailSender : IEmailSender
{
    private readonly SendGridClient client;
    private readonly IAdminNotifier adminNotifier;
    private readonly AsyncRetryPolicy retryPolicy;

    public SendGridEmailSender(IOptions<SendGridSettings> options, IAdminNotifier adminNotifier)
    {
        var apiKey = options?.Value?.ApiKey ?? throw new ArgumentException("API Key is missing.");
        this.client = new SendGridClient(apiKey);
        this.adminNotifier = adminNotifier;

        this.retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(attempt * 10));
    }

    public async Task SendEmailAsync(string from, string fromName, string to, string subject, string htmlContent, IEnumerable<EmailAttachment>? attachments = null)
    {
        if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
        {
            throw new ArgumentException("Subject and message should be provided.");
        }

        var fromAddress = new EmailAddress(from, fromName);
        var toAddress = new EmailAddress(to);
        var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);
        if (attachments?.Any() == true)
        {
            foreach (var attachment in attachments)
            {
                message.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Content), attachment.MimeType);
            }
        }

        try
        {
            await this.retryPolicy.ExecuteAsync(async () => await this.client.SendEmailAsync(message));
        }
        catch (Exception e)
        {
            string title = "Email not send!";
            string text = $"Email to {toAddress} with {subject} was not send, reason:{e.Message}!";
            await this.adminNotifier.NotifyAsync(title, text);
        }
    }
}
