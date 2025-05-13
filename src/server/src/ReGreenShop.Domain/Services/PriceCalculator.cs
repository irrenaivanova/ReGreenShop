namespace ReGreenShop.Domain.Services;
public static class PriceCalculator
{
    public static decimal CalculateDiscountedPrice(decimal price, int discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)

        {
            throw new ArgumentOutOfRangeException(nameof(discountPercentage));
        }

        return Math.Round((100 - discountPercentage) / 100m * price, 2);
    }

    public static decimal CalculateTwoForOnePrice(decimal pricePerUnit, int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity));
        }
        if (pricePerUnit < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pricePerUnit));
        }

        int paidUnits = quantity / 2 + quantity % 2;
        return Math.Round(paidUnits * pricePerUnit, 2);
    }

    public static decimal CalculateTotalPrice(decimal price, int quantity)
    {
        if (price < 0 || quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price));
        }

        return Math.Round(price * quantity, 2);
    }
}
