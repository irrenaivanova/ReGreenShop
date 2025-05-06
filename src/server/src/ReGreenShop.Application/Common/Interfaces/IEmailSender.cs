using ReGreenShop.Application.Common.Models;

namespace ReGreenShop.Application.Common.Interfaces;
public interface IEmailSender
{
    Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment>? attachments = null);

    Task SendTemplateEmailAsync(
            string from,
            string fromName,
            string to,
            string templateId,
            Dictionary<string, object> dynamicData,
            IEnumerable<EmailAttachment>? attachments = null);
}
