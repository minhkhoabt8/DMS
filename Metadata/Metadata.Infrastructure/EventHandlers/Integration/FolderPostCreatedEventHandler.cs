using AutoMapper;
using MassTransit;
using MessageContracts;
using Metadata.Core.Events.Common;
using Metadata.Core.Events.Integration;
using Metadata.Infrastructure.EventHandlers.Common;

namespace Metadata.Infrastructure.EventHandlers.Integration;

public class FolderPostCreatedEventHandler : IEventHandler<FolderPostCreatedEvent>
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public FolderPostCreatedEventHandler(IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    public async Task HandleAsync(IEvent evt)
    {
        var folder = ((FolderPostCreatedEvent) evt).Folder;

        await _publishEndpoint.Publish(_mapper.Map<FolderCreated>(folder));
    }
}