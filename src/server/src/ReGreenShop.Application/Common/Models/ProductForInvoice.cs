namespace ReGreenShop.Application.Common.Models;

// To Add if there is promotion TwoForOne
public class ProductForInvoice
{
    public string  Name { get; set; } = string.Empty;
     
    public int Quantity { get; set; }

    public decimal PricePerUnit { get; set; }

    public decimal TotalPrice => Quantity * PricePerUnit;
}
