using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.File;
using Metadata.Infrastructure.DTOs.Folder;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using File = Metadata.Core.Entities.File;

namespace Metadata.Infrastructure.Services.Implementations;

public class FileService : IFileService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public FileService(IMapper mapper, IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<FileReadDTO> GetFileDetails(Guid id)
    {
        var file = await _unitOfWork.FileRepository.FindAsync(id);

        if (file == null)
        {
            throw new EntityWithIDNotFoundException<File>(id);
        }

        return _mapper.Map<FileReadDTO>(file);
    }

    public async Task<FileReadDTO> CreateFileAsync(FileCreateDTO dto)
    {
        if (dto.ParentFolderID.HasValue)
        {
            // Check parent exists
            if ((await _unitOfWork.FolderRepository.FindAsync(dto.ParentFolderID)) == null)
            {
                throw new EntityWithIDNotFoundException<Folder>(dto.ParentFolderID);
            }
        }
        // Create in root folder
        else
        {
            dto.ParentFolderID = (await _unitOfWork.FolderRepository.QuerySingleAsync(
                new FolderQuerySingle
                {
                    IsRoot = true,
                    OwnerID = _userContextService.ID
                }
            ))!.ID;
        }

        // Check file name not taken
        var fileWithSameName = await _unitOfWork.FileRepository.QuerySingleAsync(
            new FileQuerySingle
            {
                ParentFolderID = dto.ParentFolderID,
                OwnerID = _userContextService.ID,
                SearchText = dto.Name
            }
        );

        if (fileWithSameName != null)
        {
            throw new UniqueConstraintException<File>(nameof(File.Name), dto.Name);
        }

        var file = File.Create(dto.Name, dto.ParentFolderID.Value);
        file.OwnerID = _userContextService.ID;

        await _unitOfWork.FileRepository.AddAsync(file);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<FileReadDTO>(file);
    }

    public Task<FileReadDTO> UpdateFileAsync(Guid id, FileUpdateDTO dto)
    {
        throw new NotImplementedException();
    }
}