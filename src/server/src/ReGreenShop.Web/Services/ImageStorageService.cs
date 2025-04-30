using Microsoft.AspNetCore.Hosting;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Utilities;

namespace ReGreenShop.Web.Services;
public class ImageStorageService : IImageStorage
{
    private readonly IWebHostEnvironment env;
    public ImageStorageService(IWebHostEnvironment env)
    {
        this.env = env;
    }

    public async Task<string> SaveImageAsync(byte[] imageBytes, string name)
    {
        var slugGenerator = new SlugGenerator();
        var fileName = slugGenerator.GenerateSlug(name) + ".png";
        var folderPath = Path.Combine(this.env.WebRootPath, "images");

        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);
        await File.WriteAllBytesAsync(filePath, imageBytes);

        return $"/images/{fileName}.png";
    }
}
