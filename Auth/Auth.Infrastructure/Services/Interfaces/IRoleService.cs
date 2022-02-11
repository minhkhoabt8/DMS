using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Infrastructure.DTOs;
using Auth.Infrastructure.DTOs.Role;

namespace Auth.Infrastructure.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleReadDTO>> GetAllRolesAsync();
    Task<RoleReadDTO> GetRoleAsync(int roleID);
    Task<IEnumerable<RoleReadDTO>> GetAccountRolesAsync(Guid accountID);
    Task<RoleReadDTO> CreateRoleAsync(RoleWriteDTO writeDTO);
    Task<RoleReadDTO> UpdateRoleAsync(int roleID, RoleWriteDTO writeDTO);
    Task AssignRoleToAccountAsync(int roleID, Guid accountID);
    Task RemoveRoleFromAccountAsync(int roleID, Guid accountID);
}