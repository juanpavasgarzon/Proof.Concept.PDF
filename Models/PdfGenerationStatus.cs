namespace File.Api.Models;

public enum PdfGenerationStatus
{
    /// <summary>
    /// The job is queued.
    /// </summary>
    Queued,
    
    /// <summary>
    /// The job is processing.
    /// </summary>
    Processing,
    
    /// <summary>
    /// The job is completed.
    /// </summary>
    Completed,
    
    /// <summary>
    /// The job has failed.
    /// </summary>
    Failed
}