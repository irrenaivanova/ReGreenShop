using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface IImageStorage : IService
{
    Task<string> SaveImageAsync(byte[] imageBytes, string name, string extension);
}
