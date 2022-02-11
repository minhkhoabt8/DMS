using Auth.API.Filters;
using Auth.API.ResponseWrapper;
using Auth.Infrastructure.DTOs;
using Auth.Infrastructure.DTOs.Role;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/roles")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<RoleReadDTO>>))]
    public async Task<IActionResult> GetAllRoles()
    {
        var roleItemDTOs = await _roleService.GetAllRolesAsync();

        return ResponseFactory.Ok(roleItemDTOs);
    }

    /// <summary>
    /// Get role detail
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<RoleReadDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> GetRole(int id)
    {
        var roleItemDTOs = await _roleService.GetRoleAsync(id);

        return ResponseFactory.Ok(roleItemDTOs);
    }

    /// <summary>
    /// Create new role
    /// </summary>
    /// <param name="writeDTO"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<RoleReadDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    public async Task<IActionResult> CreateRole(RoleWriteDTO writeDTO)
    {
        var createdRoleDTO = await _roleService.CreateRoleAsync(writeDTO);

        return ResponseFactory.CreatedAt(nameof(GetRole),
            nameof(RoleController),
            new {id = createdRoleDTO.ID},
            createdRoleDTO);
    }

    /// <summary>
    /// Update role
    /// </summary>
    /// <param name="id"></param>
    /// <param name="writeDTO"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<RoleReadDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> UpdateRole(int id, RoleWriteDTO writeDTO)
    {
        var roleDTO = await _roleService.UpdateRoleAsync(id, writeDTO);

        return ResponseFactory.Ok(roleDTO);
    }

    /// <summary>
    /// Assign role to account
    /// </summary>
    /// <param name="roleID"></param>
    /// <param name="accountID"></param>
    /// <returns></returns>
    [HttpPut("{roleID}/accounts/{accountID}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> AssignRoleToAccount(int roleID, Guid accountID)
    {
        await _roleService.AssignRoleToAccountAsync(roleID, accountID);

        return ResponseFactory.NoContent();
    }

    /// <summary>
    /// Remove role from account
    /// </summary>
    /// <param name="roleID"></param>
    /// <param name="accountID"></param>
    /// <returns></returns>
    [HttpDelete("{roleID}/accounts/{accountID}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> RemoveRoleFromAccount(int roleID, Guid accountID)
    {
        await _roleService.RemoveRoleFromAccountAsync(roleID, accountID);

        return ResponseFactory.NoContent();
    }
}