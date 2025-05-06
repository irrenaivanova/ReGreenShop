using Microsoft.AspNetCore.Hosting;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Utilities;

namespace ReGreenShop.Web.Services;
public class StorageService : IStorage
{
    private readonly IWebHostEnvironment env;
    public StorageService(IWebHostEnvironment env)
    {
        this.env = env;
    }

    public async Task<string> SaveImageAsync(byte[] imageBytes, string name, string extension)
    {
        var slugGenerator = new SlugGenerator();
        var fileName = slugGenerator.GenerateSlug(name) + extension;
        var folderPath = Path.Combine(this.env.WebRootPath, "images", "products");

        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);
        await File.WriteAllBytesAsync(filePath, imageBytes);

        return $"/images/products/{fileName}";
    }

    public async Task<string> SaveInvoicesAsync(byte[] imageBytes, string name)
    {
        var fileName = name + ".pdf";
        var folderPath = Path.Combine(this.env.WebRootPath, "invoices");
        Directory.CreateDirectory(folderPath);
        var filePath = Path.Combine(folderPath, fileName);
        await File.WriteAllBytesAsync(filePath, imageBytes);
        return $"/invoices/{fileName}";
    }
}
