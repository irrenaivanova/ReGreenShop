using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Utilities;

namespace ReGreenShop.Infrastructure.Services;
public class StorageService : IStorage
{
    public async Task<(string path, string name)> SaveImageAsync(byte[] imageBytes, string name, string extension)
    {
        var slugGenerator = new SlugGenerator();
        var slugName = slugGenerator.GenerateSlug(name);
        var folderPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "wwwroot", "images", "products");

        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, slugName + extension);
        await File.WriteAllBytesAsync(filePath, imageBytes);
        var relativePath = $"/images/products/{slugName}{extension}";
        return (relativePath, slugName);
    }

    public async Task<string> SaveInvoicesAsync(byte[] imageBytes, string name)
    {
        var fileName = name + ".pdf";
        var folderPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "wwwroot", "invoices");
        Directory.CreateDirectory(folderPath);
        var filePath = Path.Combine(folderPath, fileName);
        await File.WriteAllBytesAsync(filePath, imageBytes);
        return $"/invoices/{fileName}";
    }
}
