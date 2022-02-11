namespace Auth.Infrastructure.DTOs.Authentication;

public class LoginOutputDTO
{
    public string UserID { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string SysToken { get; set; }
    public int SysTokenExpires { get; set; }
    public string DMSToken { get; set; }
    public int DMSTokenExpires { get; set; }
    public string RefreshToken { get; set; }
    public int RefreshTokenExpires { get; set; }
    public string[] Roles { get; set; }
}