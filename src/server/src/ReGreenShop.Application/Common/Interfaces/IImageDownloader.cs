namespace ReGreenShop.Application.Common.Interfaces;
public interface IImageDownloader
{
    Task<string> DownloadImageAsync(string imageUrl, string fileName);
}
