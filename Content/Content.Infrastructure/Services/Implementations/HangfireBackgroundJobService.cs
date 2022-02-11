using Content.Infrastructure.Services.Interfaces;
using Content.Infrastructure.UOW;
using VCDiff.Encoders;

namespace Content.Infrastructure.Services.Implementations;

public class HangfireBackgroundJobService : IBackgroundJobService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobService _blobService;
    private readonly IFileService _fileService;

    public HangfireBackgroundJobService(IUnitOfWork unitOfWork, IBlobService blobService, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _blobService = blobService;
        _fileService = fileService;
    }

    public async Task SaveDeltaAsync(int versionID, byte[] fileBytes)
    {
        var version = await _unitOfWork.FileVersionRepository.FindAsync(versionID);

        var baseVersionContent = await _fileService.GetContentAsync(version!.FileID, version.BaseVersionID);

        await using var srcStream = new MemoryStream(baseVersionContent);
        await using var targetStream = new MemoryStream(fileBytes);
        await using var deltaStream = new MemoryStream();

        await new VcEncoder(srcStream, targetStream, deltaStream).EncodeAsync();
        deltaStream.Seek(0, SeekOrigin.Begin);

        var uploadedDeltaUrl = await _blobService.UploadAsync($"{versionID}-delta-{Guid.NewGuid()}", deltaStream);

        version.SetDelta(deltaStream.Length, uploadedDeltaUrl);

        await _unitOfWork.CommitAsync();
    }

    public async Task SaveFileAsync(int versionID, byte[] fileBytes)
    {
        var uploadedFileUrl = await _blobService.UploadAsync($"{versionID}-file-{Guid.NewGuid()}", fileBytes);

        var version = await _unitOfWork.FileVersionRepository.FindAsync(versionID);
        version!.SetFileUrl(uploadedFileUrl);

        await _unitOfWork.CommitAsync();
    }
}