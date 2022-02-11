using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Infrastructure.DTOs.Role;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using AutoMapper;

namespace Auth.Infrastructure.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task AssignRoleToAccountAsync(int roleID, Guid accountID)
    {
        var account = await _unitOfWork.AccountRepository.FindIncludeRolesAsync(accountID);

        if (account == null)
        {
            throw new EntityWithIDNotFoundException<Account>(accountID);
        }

        var role = await _unitOfWork.RoleRepository.FindAsync(roleID);

        if (role == null)
        {
            throw new EntityWithIDNotFoundException<Role>(roleID);
        }

        // Already has role
        if (account.Roles.Contains(role))
        {
            return;
        }

        account.Roles.Add(role);
        await _unitOfWork.CommitAsync();
    }

    public async Task RemoveRoleFromAccountAsync(int roleID, Guid accountID)
    {
        var account = await _unitOfWork.AccountRepository.FindIncludeRolesAsync(accountID);

        if (account == null)
        {
            throw new EntityWithIDNotFoundException<Account>(accountID);
        }

        var role = await _unitOfWork.RoleRepository.FindAsync(roleID);

        if (role == null)
        {
            throw new EntityWithIDNotFoundException<Role>(roleID);
        }

        // Account doesn't have the specified role
        if (!account.Roles.Contains(role))
        {
            return;
        }

        account.Roles.Remove(role);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<RoleReadDTO>> GetAccountRolesAsync(Guid accountID)
    {
        var roles = await _unitOfWork.RoleRepository.GetAccountRolesAsync(accountID);

        return _mapper.Map<IEnumerable<RoleReadDTO>>(roles);
    }

    public async Task<RoleReadDTO> CreateRoleAsync(RoleWriteDTO writeDTO)
    {
        await EnsureNameNotTakenByAnotherRole(writeDTO.Name);

        var role = _mapper.Map<Role>(writeDTO);

        await _unitOfWork.RoleRepository.AddAsync(role);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<RoleReadDTO>(role);
    }

    public async Task<RoleReadDTO> UpdateRoleAsync(int roleID, RoleWriteDTO writeDTO)
    {
        var role = await _unitOfWork.RoleRepository.FindAsync(roleID);

        if (role == null)
        {
            throw new EntityWithIDNotFoundException<Role>(roleID);
        }

        await EnsureNameNotTakenByAnotherRole(writeDTO.Name, roleID);

        _mapper.Map(writeDTO, role);

        await _unitOfWork.CommitAsync();

        return _mapper.Map<RoleReadDTO>(role);
    }

    private async Task EnsureNameNotTakenByAnotherRole(string name, int? roleID = null)
    {
        var roleWithSameName = await _unitOfWork.RoleRepository.FindByNameIgnoreCaseAsync(name);

        if (roleWithSameName != null && (!roleID.HasValue || roleWithSameName.ID != roleID))
        {
            throw new UniqueConstraintException<Role>(nameof(Role.Name), name);
        }
    }

    public async Task<IEnumerable<RoleReadDTO>> GetAllRolesAsync()
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<RoleReadDTO>>(roles);
    }

    public async Task<RoleReadDTO> GetRoleAsync(int roleID)
    {
        var role = await _unitOfWork.RoleRepository.FindAsync(roleID);

        if (role == null)
        {
            throw new EntityWithIDNotFoundException<Role>(roleID);
        }

        return _mapper.Map<RoleReadDTO>(role);
    }
}