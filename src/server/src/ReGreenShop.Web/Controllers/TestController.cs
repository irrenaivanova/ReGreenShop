using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Models;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IImageDownloader downloader;
    private readonly IStorage storage;
    private readonly IEmailSender sender;
    private readonly IPdfGenerator pdfGenerator;
    private readonly IWebHostEnvironment web;

    public TestController(IImageDownloader downloader, IStorage storage, IEmailSender sender, IPdfGenerator pdfGenerator,IWebHostEnvironment web)
    {
        this.downloader = downloader;
        this.storage = storage;
        this.sender = sender;
        this.pdfGenerator = pdfGenerator;
        this.web = web;
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

        var path = Path.Combine(this.web.WebRootPath, "invoices", "invoice.pdf");
        var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
        var attachment = new EmailAttachment
        {
            FileName = "invoice.pdf",
            Content = fileBytes,
            MimeType = "application/pdf"
        };
        await this.sender.SendEmailAsync(SystemEmailSender, SystemEmailSenderName, email, "Test", html.ToString(),new List<EmailAttachment> { attachment });
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


    [HttpPost(nameof(MakePdf))]
    public async Task MakePdf(InvoiceItem model)
    {
        var bytes = this.pdfGenerator.GenerateReceiptPdfAsync(model);
        await this.storage.SaveInvoicesAsync(bytes,"Invoice");
    }
}
