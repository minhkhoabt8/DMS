using Auth.Core.Entities;
using Auth.Infrastructure.DTOs;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Role;
using AutoMapper;
using MessageContracts;
using File = Auth.Core.Entities.File;

namespace Auth.Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Role
        CreateMap<Role, RoleReadDTO>();
        CreateMap<RoleWriteDTO, Role>();

        // Account
        CreateMap<Account, AccountReadDTO>();
        CreateMap<Account, AccountCreated>();

        // File
        CreateMap<FileCreated, File>();

        // Folder
        CreateMap<FolderCreated, Folder>();
    }
}