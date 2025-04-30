using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Infrastructure.Services;
public class ImageDownloaderService : IImageDownloader
{
    public async Task<byte[]> DownloadImageAsync(string imageUrl)
    {
        using var httpClient = new HttpClient();
        return await httpClient.GetByteArrayAsync(imageUrl);
    }
}
