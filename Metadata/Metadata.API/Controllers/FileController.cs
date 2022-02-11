using Metadata.API.Filters;
using Metadata.API.ResponseWrapper;
using Metadata.Infrastructure.DTOs.File;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Metadata.API.Controllers;

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
    /// Get file details
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<FileReadDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> GetFileDetails(Guid id)
    {
        var file = await _fileService.GetFileDetails(id);

        return ResponseFactory.Ok(file);
    }

    /// <summary>
    /// Create a new file
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<FileReadDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    public async Task<IActionResult> CreateFile(FileCreateDTO dto)
    {
        var createdFile = await _fileService.CreateFileAsync(dto);

        return ResponseFactory.Created(createdFile);
    }

    /// <summary>
    /// Update a file
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<FileReadDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> UpdateFile(Guid id, FileUpdateDTO dto)
    {
        var updatedFile = await _fileService.UpdateFileAsync(id, dto);

        return ResponseFactory.Ok(updatedFile);
    }
}