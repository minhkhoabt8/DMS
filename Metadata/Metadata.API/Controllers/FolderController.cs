using Metadata.API.Filters;
using Metadata.API.ResponseWrapper;
using Metadata.Infrastructure.DTOs.Folder;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metadata.API.Controllers;

[ApiController]
[Route("api/folders")]
public class FolderController : ControllerBase
{
    private readonly IFolderService _folderService;

    public FolderController(IFolderService folderService)
    {
        _folderService = folderService;
    }

    /// <summary>
    /// Create a new folder
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<FolderReadDTO>))]
    public async Task<IActionResult> CreateFolder(FolderCreateDTO dto)
    {
        var createdFolder = await _folderService.CreateAsync(dto);

        return ResponseFactory.Created(createdFolder);
    }

    /// <summary>
    /// Update a folder
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<FolderReadDTO>))]
    public async Task<IActionResult> UpdateFolder(Guid id, FolderUpdateDTO dto)
    {
        var updatedFolder = await _folderService.UpdateAsync(id, dto);

        return ResponseFactory.Ok(updatedFolder);
    }

    /// <summary>
    /// Get folder details
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<FolderReadDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> GetFolderDetails(Guid id)
    {
        var contents = await _folderService.GetDetailsAsync(id);

        return ResponseFactory.Ok(contents);
    }

    /// <summary>
    /// Get contents of folder
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/content")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<FolderContentItem>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> GetFolderContents(Guid id)
    {
        var contents = await _folderService.GetContentsAsync(id);

        return ResponseFactory.Ok(contents);
    }

    /// <summary>
    /// Get path to folder
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/path")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<FolderBreadcrumbDTO>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> GetFolderPathAsync(Guid id)
    {
        var pathFolders = await _folderService.GetPathAsync(id);

        return ResponseFactory.Ok(pathFolders);
    }

    /// <summary>
    /// Get contents of root folder
    /// </summary>
    /// <returns></returns>
    [HttpGet("root/content")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<FolderContentItem>>))]
    public async Task<IActionResult> GetRootFolderContents()
    {
        var contents = await _folderService.GetRootContentsAsync();

        return ResponseFactory.Ok(contents);
    }
}