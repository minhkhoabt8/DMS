using System;
using System.Collections.Generic;

namespace Auth.Infrastructure.DTOs.Authentication;

public class SMAuthDTO
{
    public string Access_Token { get; set; }
    public Guid UserID { get; set; }
    public string Username { get; set; }
    public int Expires_In { get; set; }
    public IEnumerable<SMModuleDTO> SystemModules { get; set; }
}