using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Domain.Entities;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Services;
public class PromoService : IPromo
{
    private readonly IData data;
    private readonly Random random = new();

    public PromoService(IData data)
    {
        this.data = data;
    }

    public async Task RefreshWeeklyPromosAsync()
    {
        // deleting all existing labels
        var existingLabelProducts = await this.data.LabelProducts.ToListAsync();
        this.data.LabelProducts.RemoveRange(existingLabelProducts);
        await this.data.SaveChangesAsync();

        var labels = await this.data.Labels.ToListAsync();

        var rootCategories = await this.data.Categories
            .Where(x => x.ParentCategoryId == null).ToListAsync();

        var labelProductsToAdd = new List<LabelProduct>();
        int countProducts = 3;

        foreach (var label in labels)
        {
            if (label.Name == TwoForOne)
            {
                countProducts = 1;
            }

            foreach (var category in rootCategories)
            {
                var products = this.data.Products
                    .Where(x => x.ProductCategories.Any(x => x.CategoryId == category.Id))
                    .OrderBy(x => Guid.NewGuid())
                    .Take(countProducts)
                    .ToList();

                foreach (var product in products)
                {
                    var labelProduct = new LabelProduct()
                    {
                        Duration = 7,
                        Product = product,
                        Label = label,
                        PercentageDiscount = label.Name == Offer
                            ? this.random.Next(10, 36)
                            : null
                    };

                    labelProductsToAdd.Add(labelProduct);
                }
            }
        }

        await this.data.LabelProducts.AddRangeAsync(labelProductsToAdd);
        await this.data.SaveChangesAsync();
    }
}
