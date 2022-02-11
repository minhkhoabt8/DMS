using Content.Core.Events.Common;
using Content.Core.Events.Integration;
using Content.Infrastructure.EventHandlers.Common;
using Content.Infrastructure.Services.Interfaces;
using Content.Infrastructure.UOW;
using MassTransit;
using MessageContracts;

namespace Content.Infrastructure.EventHandlers.Integration;

public class FileVersionPostReadyEventHandler : IEventHandler<FileVersionPostReadyEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobService _blobService;
    private readonly IPublishEndpoint _publishEndpoint;

    public FileVersionPostReadyEventHandler(IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork,
        IBlobService blobService)
    {
        _publishEndpoint = publishEndpoint;
        _unitOfWork = unitOfWork;
        _blobService = blobService;
    }

    public async Task HandleAsync(IEvent evt)
    {
        var version = ((FileVersionPostReadyEvent) evt).Version;

        await _publishEndpoint.Publish(new FileVersionReady(version.ID));

        if (version.BaseVersionID.HasValue)
        {
            var baseVersion = await _unitOfWork.FileVersionRepository.FindAsync(version.BaseVersion!);

            // Remove File when delta is significantly smaller than file (<=10%)
            // Assumptions made: base version file finished uploading and fileUrl was set
            // before this version is ready
            if (baseVersion!.DeltaUrl != null && baseVersion.FileUrl != null &&
                baseVersion.DeltaSize <= baseVersion.Size / 10)
            {
                await _blobService.DeleteAsync(baseVersion.FileUrl);
            }
        }
    }
}