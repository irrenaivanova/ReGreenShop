using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Infrastructure.Services;
public class ImageDownloaderService : IImageDownloader
{
    public async Task<(byte[] ImageBytes, string Extension)> DownloadImageAsync(string imageUrl)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(imageUrl);
        var contentType = response.Content.Headers.ContentType?.MediaType;
        var imageBytes = await response.Content.ReadAsByteArrayAsync();
        string extension = contentType switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            _ => throw new NotSupportedException($"Unsupported image type: {contentType}")
        };

        return (imageBytes, extension);
    }
}
