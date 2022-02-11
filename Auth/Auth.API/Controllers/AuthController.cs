using Auth.API.Filters;
using Auth.API.ResponseWrapper;
using Auth.Core.Entities;
using Auth.Infrastructure.DTOs;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Real Login
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LoginOutputDTO>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
    [ServiceFilter(typeof(AutoValidateModelState))]
    public async Task<IActionResult> Login(LoginInputDTO input)
    {
        var result = await _authService.LoginWithUsernamePasswordAsync(input);

        SetRefreshTokenCookie(result);

        return ResponseFactory.Ok(result);
    }

    /// <summary>
    /// Refresh token
    /// </summary>
    /// <returns></returns>
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LoginOutputDTO>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
    public async Task<IActionResult> Refresh(RefreshDTO? dto)
    {
        var result = await _authService.LoginWithRefreshTokenAsync(dto?.Token ?? Request.Cookies[nameof(RefreshToken)]);

        SetRefreshTokenCookie(result);

        return ResponseFactory.Ok(result);
        ;
    }

    private void SetRefreshTokenCookie(LoginOutputDTO result)
    {
        // Set refresh token cookies
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddSeconds(result.RefreshTokenExpires),
            SameSite = SameSiteMode.None,
            Secure = true
        };
        Response.Cookies.Append(nameof(RefreshToken), result.RefreshToken, cookieOptions);
    }
}