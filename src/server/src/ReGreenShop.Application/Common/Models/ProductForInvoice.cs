namespace ReGreenShop.Application.Common.Models;

public class ProductForInvoice
{
    public string Name { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal PricePerUnit { get; set; }

    public decimal TotalPrice { get; set; }

}
