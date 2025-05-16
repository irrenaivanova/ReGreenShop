using Microsoft.Extensions.Logging;
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
    private readonly INotifier adminNotifier;
    private readonly AsyncRetryPolicy retryPolicy;
    private readonly ILogger<SendGridEmailSender> logger;

    public SendGridEmailSender(IOptions<SendGridSettings> options,
                                INotifier adminNotifier,
                                ILogger<SendGridEmailSender> logger)
    {
        var apiKey = options?.Value?.ApiKey ?? throw new ArgumentException("API Key is missing.");
        this.client = new SendGridClient(apiKey);
        this.adminNotifier = adminNotifier;
        this.logger = logger;

        this.retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(attempt * 10),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        logger.LogWarning(exception, "Retry {@RetryCount} after {@TimeSpan} due to: {@Message}", retryCount, timeSpan, exception.Message);
                    });
    }

    public async Task SendEmailAsync(string from, string fromName, string to, string subject, string htmlContent, IEnumerable<EmailAttachment>? attachments = null)
    {
        if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
        {
            throw new ArgumentException("Subject and message should be provided.");
        }

        var message = MailHelper.CreateSingleEmail(
            new EmailAddress(from, fromName),
            new EmailAddress(to),
            subject,
            plainTextContent: null,
            htmlContent: htmlContent
        );

        AddAttachments(message, attachments);

        await SendWithPolicyAsync(message, to);
    }

    public async Task SendTemplateEmailAsync(string from, string fromName, string to,
                                string templateId, Dictionary<string, object> dynamicData,
                                IEnumerable<EmailAttachment>? attachments = null)
    {
        var message = new SendGridMessage
        {
            From = new EmailAddress(from, fromName),
            TemplateId = templateId
        };

        message.AddTo(new EmailAddress(to));
        message.SetTemplateData(dynamicData);

        AddAttachments(message, attachments);

        await SendWithPolicyAsync(message, to);
    }


    private async Task SendWithPolicyAsync(SendGridMessage message, string recipientEmail)
    {
        try
        {
            await this.retryPolicy.ExecuteAsync(async () =>
            {
                this.logger.LogInformation("Sending email to {@Email}", recipientEmail);

                var response = await this.client.SendEmailAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    this.logger.LogError("SendGrid failed with status code {StatusCode}. Body: {Body}", response.StatusCode, responseBody);
                    throw new InvalidOperationException($"SendGrid failed with status code {response.StatusCode}. Body: {responseBody}");
                }

                this.logger.LogInformation("Email sent to {@Email} successfully.", recipientEmail);
            });
        }
        catch (Exception ex)
        {
            string title = "Email not sent!";
            string body = $"Email to {recipientEmail} failed. Reason: {ex.Message}";

            this.logger.LogError(ex, "Failed to send email. Notifying admin...");
            await this.adminNotifier.NotifyAdminAsync(title, body);
        }
    }

    private void AddAttachments(SendGridMessage message, IEnumerable<EmailAttachment>? attachments)
    {
        if (attachments?.Any() == true)
        {
            foreach (var attachment in attachments)
            {
                message.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Content), attachment.MimeType);
            }
        }
    }


    //public async Task SendEmailAsync(string from, string fromName, string to, string subject, string htmlContent, IEnumerable<EmailAttachment>? attachments = null)
    //{
    //    if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
    //    {
    //        throw new ArgumentException("Subject and message should be provided.");
    //    }

    //    var fromAddress = new EmailAddress(from, fromName);
    //    var toAddress = new EmailAddress(to);
    //    var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);
    //    if (attachments?.Any() == true)
    //    {
    //        foreach (var attachment in attachments)
    //        {
    //            message.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Content), attachment.MimeType);
    //        }
    //    }

    //    try
    //    {
    //        await this.retryPolicy.ExecuteAsync(async () =>
    //        {
    //            this.logger.LogInformation("Sending email to {@Email} with subject '{@Subject}'", to, subject);
    //            var response = await this.client.SendEmailAsync(message);

    //            if (!response.IsSuccessStatusCode)
    //            {
    //                string responseBody = await response.Body.ReadAsStringAsync();
    //                this.logger.LogError("SendGrid failed with status code {@StatusCode}. Response body: {@Body}", response.StatusCode, responseBody);
    //                throw new InvalidOperationException($"SendGrid failed with status code {response.StatusCode}. Body: {responseBody}");
    //            }

    //            this.logger.LogInformation("Email to {@Email} sent successfully with subject '{@Subject}'", to, subject);
    //        });
    //    }
    //    catch (Exception e)
    //    {
    //        string title = "Email not send!";
    //        string text = $"Email to {to} with {subject} was not send, reason:{e.Message}!";
    //        this.logger.LogError(e, "Failed to send email to {Email}. Notifying admin...", to);
    //        await this.adminNotifier.NotifyAsync(title, text);
    //    }
    //}
}
