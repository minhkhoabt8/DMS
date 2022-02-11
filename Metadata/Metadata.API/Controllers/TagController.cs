using Metadata.API.Filters;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Metadata.API.Controllers;

[ApiController]
[Route("api/tags")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    // [HttpGet]
    // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TagReadDTO>))]
    // public async Task<IActionResult> GetAllAsync()
    // {
    //     throw new NotImplementedException();
    // }

    // [HttpPost]
    // [ServiceFilter(typeof(AutoValidateModelState))]
    // public async Task<IActionResult> CreateAsync(TagWriteDTO dto)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // [HttpPut("{id}")]
    // [ServiceFilter(typeof(AutoValidateModelState))]
    // async Task<IActionResult> UpdateAsync(int id, TagWriteDTO dto)
    // {
    //     throw new NotImplementedException();
    // }

    [HttpDelete("{id}")]
    async Task<IActionResult> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}