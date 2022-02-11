using System.ComponentModel.DataAnnotations;

namespace Auth.Infrastructure.DTOs.Authentication;

public class LoginInputDTO
{
    [Required] public string Username { get; set; }
    [Required] public string Password { get; set; }
}