using QuestPDF.Fluent;
using QuestPDF.Helpers;
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
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Invoice")
                    .SemiBold().FontSize(20).AlignCenter();

                page.Content().Column(column =>
                {
                    column.Spacing(5);

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Product");
                            header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                            header.Cell().Element(CellStyle).AlignRight().Text("Unit Price");
                            header.Cell().Element(CellStyle).AlignRight().Text("Total");
                        });

                        foreach (var product in model.Products)
                        {
                            table.Cell().Element(CellStyle).Text(product.Name);
                            table.Cell().Element(CellStyle).AlignRight().Text(product.Quantity.ToString());
                            table.Cell().Element(CellStyle).AlignRight().Text($"{product.PricePerUnit} lv");
                            table.Cell().Element(CellStyle).AlignRight().Text($"{product.TotalPrice} lv");
                        }
                    });

                    column.Item().LineHorizontal(1);

                    column.Item().AlignRight().Text($"Subtotal: {model.TotalPriceProducts} lv");
                    column.Item().AlignRight().Text($"Delivery: {model.DeliveryPrice} lv");
                    column.Item().AlignRight().Text($"Discount: {model.Discount} lv");
                    column.Item().AlignRight().Text($"Total: {model.TotalPrice} lv").Bold();
                });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }

    static IContainer CellStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(5);
    }
}


