using Content.API.ResponseWrapper;
using Content.Infrastructure.DTOs.File;
using Content.Infrastructure.DTOs.FileVersion;
using Content.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content.API.Controllers;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Get file's content
    /// </summary>
    /// <param name="id"></param>
    /// <param name="versionID"></param>
    /// <returns></returns>
    [HttpGet("{id}/content")]
    public async Task<IActionResult> GetFileContent(Guid id, int? versionID)
    {
        var fileBytes = await _fileService.GetContentAsync(id, versionID);

        return File(fileBytes, "application/octet-stream", "File.docx");
    }


    /// <summary>
    /// Upload file
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("{id}/content")]
    public async Task<IActionResult> Upload(Guid id, [FromForm] FileUploadDTO dto)
    {
        await _fileService.UploadAsync(id, dto);

        return ResponseFactory.Accepted();
    }

    /// <summary>
    /// Get file's versions
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/versions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<FileVersionReadDTO>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> GetFileVersions(Guid id)
    {
        var versions = await _fileService.GetVersionsAsync(id);

        return ResponseFactory.Ok(versions);
    }

    /// <summary>
    /// Delete file's version
    /// </summary>
    /// <param name="id"></param>
    /// <param name="versionID"></param>
    /// <returns></returns>
    [HttpDelete("{id}/versions/{versionID}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> GetFileVersions(Guid id, int versionID)
    {
        await _fileService.DeactivateVersionAsync(id, versionID);

        return ResponseFactory.NoContent();
    }
}