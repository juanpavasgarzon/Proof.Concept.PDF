namespace File.Api.Models;

public enum PdfGenerationStatus
{
    Queued,
    Processing,
    Completed,
    Failed
}