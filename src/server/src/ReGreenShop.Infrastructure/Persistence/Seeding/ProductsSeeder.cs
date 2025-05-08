using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;
using ReGreenShop.Infrastructure.Services;

namespace ReGreenShop.Infrastructure.Persistence.Seeding;
internal class ProductsSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext data, IServiceProvider serviceProvider)
    {
        if (data.Products.Any())
        {
            return;
        }
        var folderPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "wwwroot", "seedFiles");
        var files = Directory.GetFiles(folderPath, "*.json");
        var random = new Random();

        for (int m = 0; m < files.Length; m++)
        {
            var file = files[m];

            var json = await File.ReadAllTextAsync(file);
            var products = JsonConvert.DeserializeObject<List<ProductDto>>(json)!;

            for (int j = 0; j < products.Count; j++)
            {
                var product = products[j];

                if (product.Name == null)
                {
                    continue;
                }
                if (product.Brand != null && product.Brand.Length > 50)
                {
                    continue;
                }

                if (product.Origin != null && product.Origin.Length > 50)
                {
                    continue;
                }
                var newProduct = new Product()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Packaging = product.Packaging,
                    Price = product.Price,
                    Stock = random.Next(1, 101),
                    ProductCode = $"{product.Name.ToUpper().Substring(0, 3)}{random.Next(101, 1000)}",
                    Brand = product.Brand,
                    Origin = product.Origin,
                    OriginalUrl = product.OriginalUrl,
                };

                if (product.ImageUrl == null)
                {
                    continue;
                }

                (byte[] imageBytes, string extension) = await new ImageDownloaderService().DownloadImageAsync(product.ImageUrl);
                (string path, string name) = await new StorageService().SaveImageAsync(imageBytes, product.Name, extension);

                var image = new Image()
                {
                    OriginalUrl = product.ImageUrl,
                    LocalPath = path,
                    Name = name,
                };
                await data.Images.AddAsync(image);

                newProduct.Image = image;

                await data.Products.AddAsync(newProduct);
                await data.SaveChangesAsync(CancellationToken.None);

                var categories = product.Categories.ToList();
                for (int i = 0; i < categories.Count(); i++)
                {
                    if (i == 0)
                    {
                        var mainCategory = await data.Categories.FirstOrDefaultAsync(x => x.NameInBulgarian == categories[i]);
                        if (mainCategory == null)
                        {
                            mainCategory = new Category() { NameInBulgarian = categories[i] };
                            await data.Categories.AddAsync(mainCategory);
                            await data.SaveChangesAsync(CancellationToken.None);
                        }
                        bool alreadyExists = await data.ProductCategories
                                 .AnyAsync(x => x.ProductId == newProduct.Id && x.CategoryId == mainCategory.Id);

                        if (alreadyExists)
                        {
                            continue;
                        }

                        newProduct.ProductCategories.Add(new ProductCategory { Category = mainCategory, Product = newProduct });
                        await data.SaveChangesAsync(CancellationToken.None);
                    }

                    else
                    {
                        var category = await data.Categories.FirstOrDefaultAsync(x => x.NameInBulgarian == categories[i]);
                        if (category == null)
                        {
                            var parentCategory = await data.Categories.FirstOrDefaultAsync(x => x.NameInBulgarian == categories[i - 1]);
                            category = new Category()
                            {
                                NameInBulgarian = categories[i],
                                ParentCategory = parentCategory,
                            };
                            await data.Categories.AddAsync(category);
                            await data.SaveChangesAsync(CancellationToken.None);
                        }

                        bool alreadyExists = await data.ProductCategories
                                .AnyAsync(x => x.ProductId == newProduct.Id && x.CategoryId == category.Id);

                        if (alreadyExists)
                        {
                            continue;
                        }

                        newProduct.ProductCategories.Add(new ProductCategory { Category = category, Product = newProduct });
                        await data.SaveChangesAsync(CancellationToken.None);
                    }
                }

                await data.SaveChangesAsync(CancellationToken.None);
            }
        }
    }
}
