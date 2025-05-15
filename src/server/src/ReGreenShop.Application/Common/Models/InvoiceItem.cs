namespace ReGreenShop.Application.Common.Models;
public class InvoiceItem
{
    public IList<ProductForInvoice> Products { get; set; } = new List<ProductForInvoice>();

    public decimal TotalPriceProducts { get; set; }

    public decimal DeliveryPrice { get; set; }

    public decimal Discount { get; set; }

    public decimal TotalPrice { get; set; }
}
