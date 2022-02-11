using AutoMapper;
using Content.Core.Entities;
using Content.Core.Enums;
using Content.Core.Exceptions;
using Content.Core.Extensions;
using Content.Infrastructure.DTOs.File;
using Content.Infrastructure.DTOs.FileVersion;
using Content.Infrastructure.Extensions;
using Content.Infrastructure.Services.Interfaces;
using Content.Infrastructure.UOW;
using FileSignatures;
using Hangfire;
using VCDiff.Decoders;
using File = Content.Core.Entities.File;

namespace Content.Infrastructure.Services.Implementations;

public class FileService : IFileService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobService _blobService;
    private readonly IUserContextService _userContextService;

    public FileService(IUnitOfWork unitOfWork, IUserContextService userContextService, IBlobService blobService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
        _blobService = blobService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FileVersionReadDTO>> GetVersionsAsync(Guid id)
    {
        var file = await _unitOfWork.FileRepository.QuerySingleAsync(
            new FileQuerySingle
            {
                ID = id,
                Include = "Versions.Uploader"
            }
        );

        if (file == null)
        {
            throw new EntityWithIDNotFoundException<File>(id);
        }

        // Show active versions and orderby version from old to new
        return _mapper.Map<IEnumerable<FileVersionReadDTO>>(
            file.Versions
                .Where(v => v.IsActive)
                .OrderBy(v => v.VersionNumber)
                .ToList()
        );
    }

    public async Task DeactivateVersionAsync(Guid id, int versionID)
    {
        var file = await _unitOfWork.FileRepository.QuerySingleAsync(
            new FileQuerySingle
            {
                ID = id,
                Include = "Versions"
            },
            trackChanges: true
        );

        if (file == null)
        {
            throw new EntityWithIDNotFoundException<File>(id);
        }

        var version = file.Versions.FirstOrDefault(v => v.ID == versionID);

        if (version == null)
        {
            throw new EntityWithIDNotFoundException<FileVersion>(versionID);
        }

        version.Deactivate();

        await _unitOfWork.CommitAsync();
    }

    public async Task<byte[]> GetContentAsync(Guid id, int? versionID)
    {
        var version = await GetFileVersion(id, versionID);

        return await BuildContent(version);
    }

    private async Task<byte[]> BuildContent(FileVersion? version)
    {
        var deltaUrls = new List<string>();

        // Find a version that has file
        while (version!.FileUrl == null)
        {
            deltaUrls.Add(version.DeltaUrl!);
            version = await _unitOfWork.FileVersionRepository.FindAsync(version.BaseVersionID!);
        }

        // Reverse delta to order from old to new
        deltaUrls.Reverse();

        var srcStream = await _blobService.DownloadAsync(version.FileUrl);
        var destStream = new MemoryStream();

        // Apply deltas
        foreach (var url in deltaUrls)
        {
            await using var deltaStream = await _blobService.DownloadAsync(url);
            await new VcDecoder(srcStream, deltaStream, destStream).DecodeAsync();
            srcStream = destStream;
            srcStream.Seek(0, SeekOrigin.Begin);
            destStream = new MemoryStream();
        }

        await srcStream.CopyToAsync(destStream);

        return destStream.ToArray();
    }

    private async Task<FileVersion?> GetFileVersion(Guid id, int? versionID)
    {
        // Get latest active and ready version
        if (!versionID.HasValue)
        {
            var file = await _unitOfWork.FileRepository.QuerySingleAsync(
                new FileQuerySingle
                {
                    ID = id,
                    Include = "Versions"
                }
            );

            if (file == null)
            {
                throw new EntityWithIDNotFoundException<File>(id);
            }

            return file.GetNewestVersion(ready: true, active: true) ?? throw new NoFileVersionException();
        }
        else
        {
            var version = await _unitOfWork.FileVersionRepository.FindAsync(versionID) ??
                          throw new EntityWithIDNotFoundException<FileVersion>(versionID);

            if (!version.IsAvailableForViewing())
            {
                throw new NoFileVersionException();
            }

            return version;
        }
    }

    public async Task UploadAsync(Guid id, FileUploadDTO dto)
    {
        var file = await _unitOfWork.FileRepository.QuerySingleAsync(
            new FileQuerySingle
            {
                ID = id,
                Include = "Versions"
            },
            trackChanges: true
        );

        if (file == null)
        {
            throw new EntityWithIDNotFoundException<File>(id);
        }

        await using var uploadFileStream = dto.File.OpenReadStream();
        var uploadFileBytes = uploadFileStream.ToBytes();

        var lastActiveVersion = file.GetNewestVersion(active: true);

        var newVersion = FileVersion.Create(
            size: dto.File.Length,
            fileName: dto.File.Name,
            type: GetFileType(uploadFileStream),
            uploaderID: _userContextService.ID
        );

        // Not the first version
        if (lastActiveVersion != null)
        {
            newVersion.BaseOn(lastActiveVersion);
        }

        file.Versions.Add(newVersion);
        await _unitOfWork.CommitAsync();

        if (newVersion.BaseVersion != null)
        {
            BackgroundJob.Enqueue<IBackgroundJobService>(bgs => bgs.SaveDeltaAsync(newVersion.ID, uploadFileBytes));
        }

        BackgroundJob.Enqueue<IBackgroundJobService>(bgs => bgs.SaveFileAsync(newVersion.ID, uploadFileBytes));
    }

    private FileType GetFileType(Stream fileStream)
    {
        var inspector = new FileFormatInspector();
        var format = inspector.DetermineFileFormat(fileStream) ?? throw new UnsupportedFileTypeException();
        fileStream.Seek(0, SeekOrigin.Begin);

        return format.Extension.ToFileType();
    }
}