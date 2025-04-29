using Microsoft.AspNetCore.Hosting;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Infrastructure.Services;
public class ImageDownloader : IImageDownloader
{
    private readonly IWebHostEnvironment env;

    public ImageDownloader(IWebHostEnvironment env)
    {
        this.env = env;
    }

    public async Task<string> DownloadImageAsync(string imageUrl, string name)
    {
        using var httpClient = new HttpClient();
        var bytes = await httpClient.GetByteArrayAsync(imageUrl);

        var imagesPath = Path.Combine(this.env.WebRootPath, "images");
        Directory.CreateDirectory(imagesPath);

        var filePath = Path.Combine(imagesPath, name);
        await File.WriteAllBytesAsync(filePath, bytes);

        return $"/images/products/{name}";
    }
}
