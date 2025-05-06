using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Models;

namespace ReGreenShop.Infrastructure.Services;
public class PdfGeneratorService : IPdfGenerator
{
    public byte[] GenerateReceiptPdfAsync(InvoiceItem model)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(QuestPDF.Helpers.PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Content().Column(col =>
                {
                    col.Item().Text("Order Receipt").FontSize(20).Bold().AlignCenter();
                    col.Item().Text($"Customer: {model.ProductName}");
                    col.Item().Text($"Order Date: {model.Quantity:d}");
                    col.Item().LineHorizontal(1);

                });
            });
        });

        return document.GeneratePdf();
    }
}

