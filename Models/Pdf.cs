using System.Text.Json;

namespace File.Api.Models;

public sealed class Pdf(string name, PdfGenerationStatus status, string link)
{
    /// <summary>
    /// The name of the PDF
    /// </summary>
    public string Name { get; init; } = name;

    /// <summary>
    /// The status of the PDF generation
    /// </summary>
    public PdfGenerationStatus Status { get; set; } = status;

    /// <summary>
    /// The link to the PDF
    /// </summary>
    public string Link { get; set; } = link;

    /// <summary>
    /// Returns a string representation of the record
    /// </summary>
    /// <returns>A string representation of the record</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(new { Name, Status = Status.ToString(), Link });
    }
}