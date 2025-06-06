using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface IStorage : IService
{
    Task<(string path, string name)> SaveImageAsync(byte[] imageBytes, string name, string extension);

    Task<string> SaveInvoicesAsync(byte[] imageBytes, string name);
}
