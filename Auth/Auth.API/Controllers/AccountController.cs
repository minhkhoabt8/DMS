using Auth.API.ResponseWrapper;
using Auth.Infrastructure.DTOs;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Get all accounts
    /// </summary>
    /// <returns></returns>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AccountReadDTO>>))]
    public async Task<IActionResult> GetAll()
    {
        var accountDTOs = await _accountService.GetAllAccountsAsync();

        return ResponseFactory.Ok(accountDTOs);
    }

    /// <summary>
    /// Query accounts
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<AccountReadDTO>))]
    public async Task<IActionResult> QueryAccounts([FromQuery] AccountQuery query)
    {
        var accounts = await _accountService.QueryAccountsAsync(query);

        return ResponseFactory.PaginatedOk(accounts);
    }

    /// <summary>
    /// Get account
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AccountReadDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> Get(Guid id)
    {
        var accountDTOs = await _accountService.GetAccountAsync(id);

        return ResponseFactory.Ok(accountDTOs);
    }
}