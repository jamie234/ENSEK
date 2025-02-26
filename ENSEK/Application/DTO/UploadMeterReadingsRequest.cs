using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class UploadMeterReadingsRequest
{
    public required IFormFile FileStream { get; set; }

    public bool ValidFileStream() => FileStream != null && FileStream.Length > 0;
}
