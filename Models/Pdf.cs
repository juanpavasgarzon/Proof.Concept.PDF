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
    /// Returns object representation of the record
    /// </summary>
    /// <returns>Object representation of the record</returns>
    public object ToJson()
    {
        return new { Name, Status = Status.ToString(), Link };
    }
}