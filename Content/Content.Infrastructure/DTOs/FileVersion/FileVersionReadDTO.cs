namespace Content.Infrastructure.DTOs.FileVersion;

public class FileVersionReadDTO
{
    public int ID { get; set; }
    public string FileName { get; set; }
    public long Size { get; set; }
    public string Type { get; set; }
    public bool IsReady { get; set; }
    public DateTime UploadedAt { get; set; }
    public Guid UploaderID { get; set; }
    public string UploaderFullName { get; set; }
}