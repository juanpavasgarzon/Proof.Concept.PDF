namespace File.Api.Models;

/// <summary>
/// Represents a PDF generation job.
/// </summary>
/// <param name="Id">The id of the job</param>
/// <param name="Name">The name of the job</param>
/// <param name="Data">The data of the job</param>
public sealed record PdfGenerationJob(string Id, string Name, byte[] Data);