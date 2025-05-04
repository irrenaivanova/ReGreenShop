using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IImageDownloader downloader;
    private readonly IImageStorage storage;

    public TestController(IImageDownloader downloader, IImageStorage storage)
    {
        this.downloader = downloader;
        this.storage = storage;
    }

    [HttpGet(nameof(DownloadImage))]
    public async Task DownloadImage()
    {
        string imageUrl = @"https://static-new.kolichka.bg/k3wCdnContainerk3w-static-ne-bg-prod/images/thumbs/mg/300x200x1_mg4t5udji0to.jpg";
        string name = "test";
        var bytesExtension = await this.downloader.DownloadImageAsync(imageUrl);
        await this.storage.SaveImageAsync(bytesExtension.ImageBytes, name, bytesExtension.Extension);
    }
}
