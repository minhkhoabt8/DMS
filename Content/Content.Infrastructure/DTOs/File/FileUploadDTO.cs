using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Content.Infrastructure.DTOs.File;

public class FileUploadDTO
{
    [Required] public IFormFile File { get; set; }
}