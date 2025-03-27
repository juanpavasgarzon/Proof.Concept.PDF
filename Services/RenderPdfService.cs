using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace File.Api.Services;

internal sealed class RenderPdfService
{
    public static void Render(string output)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(50);

                page.Header()
                    .PaddingBottom(20)
                    .AlignCenter()
                    .Text("Sales Report")
                    .FontSize(24)
                    .Bold()
                    .FontColor(Colors.Blue.Medium);

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.ConstantColumn(80);
                        columns.ConstantColumn(80);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(handler => handler
                                .Padding(5)
                                .Background(Colors.Grey.Lighten3)
                                .BorderBottom(1))
                            .Text("Product")
                            .FontSize(14)
                            .Bold();

                        header.Cell().Element(handler => handler
                                .Padding(5)
                                .Background(Colors.Grey.Lighten3)
                                .BorderBottom(1))
                            .Text("Category")
                            .FontSize(14)
                            .Bold();

                        header.Cell().Element(handler => handler
                                .Padding(5)
                                .Background(Colors.Grey.Lighten3)
                                .BorderBottom(1))
                            .Text("Price")
                            .FontSize(14)
                            .Bold();

                        header.Cell().Element(handler => handler
                                .Padding(5)
                                .Background(Colors.Grey.Lighten3)
                                .BorderBottom(1))
                            .Text("Stock")
                            .FontSize(14)
                            .Bold();
                    });

                    string[,] products =
                    {
                        { "Laptop", "Electronics", "$1200", "15" },
                        { "Headphones", "Accessories", "$200", "50" },
                        { "Smartphone", "Electronics", "$800", "30" },
                        { "Desk Chair", "Furniture", "$150", "10" }
                    };

                    foreach (var product in products)
                    {
                        foreach (var value in product)
                            table.Cell()
                                .Element(handler => handler
                                    .Padding(5)
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2))
                                .Text(value)
                                .FontSize(14)
                                .Bold();
                    }
                });

                page.Footer()
                    .AlignCenter()
                    .Text("Generated on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            });
        });

        document.GeneratePdf(output);
    }
}