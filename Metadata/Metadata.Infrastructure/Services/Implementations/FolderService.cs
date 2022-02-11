using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.Folder;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;

namespace Metadata.Infrastructure.Services.Implementations;

public class FolderService : IFolderService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public FolderService(IUserContextService userContextService, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userContextService = userContextService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<FolderReadDTO> CreateAsync(FolderCreateDTO dto)
    {
        // TODO: Check write permission on parent folder

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

        // Check folder name not taken
        var folderWithSameName = await _unitOfWork.FolderRepository.QuerySingleAsync(
            new FolderQuerySingle
            {
                ParentFolderID = dto.ParentFolderID,
                OwnerID = _userContextService.ID,
                SearchText = dto.Name
            }
        );

        if (folderWithSameName != null)
        {
            throw new UniqueConstraintException<Folder>(nameof(Folder.Name), dto.Name);
        }

        var folder = Folder.Create(dto.Name, dto.ParentFolderID);
        folder.OwnerID = _userContextService.ID;

        await _unitOfWork.FolderRepository.AddAsync(folder);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<FolderReadDTO>(folder);
    }

    public async Task<FolderReadDTO> UpdateAsync(Guid id, FolderUpdateDTO dto)
    {
        var folder = await _unitOfWork.FolderRepository.FindAsync(id);

        if (folder == null)
        {
            throw new EntityWithIDNotFoundException<Folder>(id);
        }

        _mapper.Map(dto, folder);

        await _unitOfWork.CommitAsync();

        return _mapper.Map<FolderReadDTO>(folder);
    }

    public async Task<FolderReadDTO> GetDetailsAsync(Guid id)
    {
        var folder = await _unitOfWork.FolderRepository.FindAsync(id);

        if (folder == null)
        {
            throw new EntityWithIDNotFoundException<Folder>(id);
        }

        return _mapper.Map<FolderReadDTO>(folder);
    }

    public async Task<IEnumerable<FolderContentItem>> GetContentsAsync(Guid id)
    {
        var folder = await _unitOfWork.FolderRepository.QuerySingleAsync(
            new FolderQuerySingle
            {
                ID = id,
                Include = "Files.Versions, Subfolders"
            });

        if (folder == null)
        {
            throw new EntityWithIDNotFoundException<Folder>(id);
        }

        var files = _mapper.Map<IEnumerable<FolderContentItem>>(folder.Files);
        var subFolders = _mapper.Map<IEnumerable<FolderContentItem>>(folder.SubFolders);

        return files.Concat(subFolders);
    }

    public async Task<IEnumerable<FolderContentItem>> GetRootContentsAsync()
    {
        var accountID = _userContextService.ID;

        if ((await _unitOfWork.AccountRepository.FindAsync(accountID)) == null)
        {
            throw new EntityWithIDNotFoundException<Account>(accountID);
        }

        var rootFolder = await _unitOfWork.FolderRepository.QuerySingleAsync(
            new FolderQuerySingle
            {
                OwnerID = accountID,
                IsRoot = true,
                Include = "Files.Versions, Subfolders"
            }
        );

        var files = _mapper.Map<IEnumerable<FolderContentItem>>(rootFolder!.Files);
        var subFolders = _mapper.Map<IEnumerable<FolderContentItem>>(rootFolder.SubFolders);

        return files.Concat(subFolders);
    }

    public Task<IEnumerable<FolderContentItem>> GetSharedContentsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<FolderBreadcrumbDTO>> GetPathAsync(Guid id)
    {
        var folder = await _unitOfWork.FolderRepository.FindAsync(id);

        if (folder == null)
        {
            throw new EntityWithIDNotFoundException<Folder>(id);
        }

        var pathFolders = new List<Folder> {folder};

        while (folder!.ParentFolderID.HasValue)
        {
            folder = await _unitOfWork.FolderRepository.FindAsync(folder.ParentFolderID);
            pathFolders.Add(folder!);
        }

        pathFolders.Reverse();

        return _mapper.Map<IEnumerable<FolderBreadcrumbDTO>>(pathFolders);
    }
}