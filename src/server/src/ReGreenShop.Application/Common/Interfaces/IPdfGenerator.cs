using ReGreenShop.Application.Common.Models;
using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface IPdfGenerator : IService
{
    byte[] GenerateReceiptPdfAsync(InvoiceItem model);
}
