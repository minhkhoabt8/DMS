using System;
using System.Collections.Generic;
using Auth.Infrastructure.DTOs.Role;

namespace Auth.Infrastructure.DTOs.Account;

public class AccountReadDTO
{
    public Guid ID { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FullName { get; set; }
    public bool IsActive { get; set; }
    public IEnumerable<RoleReadDTO> Roles { get; set; }
}