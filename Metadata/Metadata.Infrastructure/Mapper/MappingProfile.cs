using AutoMapper;
using MessageContracts;
using Metadata.Core.Entities;
using Metadata.Core.Events.Integration;
using Metadata.Infrastructure.DTOs.File;
using Metadata.Infrastructure.DTOs.Folder;
using Metadata.Infrastructure.DTOs.Tag;
using File = Metadata.Core.Entities.File;

namespace Metadata.Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Account
        CreateMap<AccountCreated, Account>();

        // Folder
        CreateMap<Folder, FolderCreated>();
        CreateMap<FolderUpdateDTO, Folder>();
        CreateMap<Folder, FolderReadDTO>();
        CreateMap<Folder, FolderContentItem>()
            .ForMember(fi => fi.Type, opt => opt.MapFrom(_ => nameof(Folder)))
            .ForMember(fi => fi.IsUploading, opt => opt.MapFrom(_ => false));
        CreateMap<Folder, FolderBreadcrumbDTO>();

        // File
        CreateMap<File, FolderContentItem>();
        CreateMap<File, FileReadDTO>();
        CreateMap<File, FileCreated>();

        // File version
        CreateMap<FileVersionCreated, FileVersion>();

        // Tag
        CreateMap<Tag, TagReadDTO>();
        CreateMap<TagWriteDTO, Tag>();
    }
}