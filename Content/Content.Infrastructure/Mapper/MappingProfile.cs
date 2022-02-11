using AutoMapper;
using Content.Core.Entities;
using Content.Infrastructure.DTOs.FileVersion;
using MessageContracts;
using File = Content.Core.Entities.File;

namespace Content.Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Account
        CreateMap<AccountCreated, Account>();

        // File
        CreateMap<FileCreated, File>();

        // File version
        CreateMap<FileVersion, FileVersionCreated>();
        CreateMap<FileVersion, FileVersionReadDTO>();
    }
}