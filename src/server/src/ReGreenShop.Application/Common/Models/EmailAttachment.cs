namespace ReGreenShop.Application.Common.Models;
public class EmailAttachment
{
    public byte[] Content { get; set; } = default!;

    public string FileName { get; set; } = string.Empty;

    public string MimeType { get; set; } = string.Empty;
}

