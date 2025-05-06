using System.Text;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Common.Interfaces;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IImageDownloader downloader;
    private readonly IImageStorage storage;
    private readonly IEmailSender sender;

    public TestController(IImageDownloader downloader, IImageStorage storage, IEmailSender sender)
    {
        this.downloader = downloader;
        this.storage = storage;
        this.sender = sender;
    }

    [HttpGet(nameof(DownloadImage))]
    public async Task DownloadImage()
    {
        string imageUrl = @"https://static-new.kolichka.bg/k3wCdnContainerk3w-static-ne-bg-prod/images/thumbs/mg/300x200x1_mg4t5udji0to.jpg";
        string name = "test";
        var bytesExtension = await this.downloader.DownloadImageAsync(imageUrl);
        await this.storage.SaveImageAsync(bytesExtension.ImageBytes, name, bytesExtension.Extension);
    }

    [HttpGet(nameof(SendEmail))]
    public async Task SendEmail(string email)
    {
        var html = new StringBuilder();
        html.AppendLine($"<h3>Thank you for contacting us!</h3>");
        html.AppendLine($"<p>Dear Irena,</p>");
        html.AppendLine($"<p>Thanks for getting in touch! We’ve received your message and " +
            $"will get back to you as soon as we can — usually within 3 days.</p>");
        await this.sender.SendEmailAsync(SystemEmailSender, SystemEmailSenderName, email, "Test", html.ToString());
    }


    [HttpGet(nameof(SendEmailTemplate))]
    public async Task SendEmailTemplate(string email)
    {
        string templateId = "d-5e226b6ae4c4434cadfee761ff05ea51";
        var dynamicDta = new Dictionary<string, object>
        {
            { "name1", "John Doe" },
            { "name2", "12345" },
        };
        await this.sender.SendTemplateEmailAsync(SystemEmailSender, SystemEmailSenderName, email, templateId, dynamicDta);
    }
}
