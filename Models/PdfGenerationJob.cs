namespace File.Api.Models;

public sealed record PdfGenerationJob(string Id, string Name, byte[] Data);