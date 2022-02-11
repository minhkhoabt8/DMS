using Auth.Core.Extensions;

namespace Auth.Core.Entities;

public class RefreshToken
{
    public int ID { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.Now >= Expires;
    public int ExpiresIn => (int) Expires.Subtract(DateTime.Now).TotalSeconds;
    public Account Account { get; set; }
    public Guid AccountID { get; set; }
    public bool IsRevoked { get; set; } = false;
    public string? ReplacedByToken { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now.SetKindUtc();

    public void Revoke()
    {
        IsRevoked = true;
    }

    public void ReplaceWith(RefreshToken replacement)
    {
        ReplacedByToken = replacement.Token;
        Revoke();
    }
}