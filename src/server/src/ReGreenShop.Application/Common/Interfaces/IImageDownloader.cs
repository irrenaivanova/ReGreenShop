using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface IImageDownloader : IService
{
    Task<byte[]> DownloadImageAsync(string imageUrl);
}
