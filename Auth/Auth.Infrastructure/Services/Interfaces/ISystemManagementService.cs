using System.Threading.Tasks;
using Auth.Infrastructure.DTOs;
using Auth.Infrastructure.DTOs.Authentication;

namespace Auth.Infrastructure.Services.Interfaces;

public interface ISystemManagementService
{
    public Task<SMAuthDTO> LoginAsync(LoginInputDTO inputDTO);
}