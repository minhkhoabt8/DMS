using AutoMapper;
using MassTransit;
using MessageContracts;
using Metadata.Core.Events.Common;
using Metadata.Core.Events.Integration;
using Metadata.Infrastructure.EventHandlers.Common;

namespace Metadata.Infrastructure.EventHandlers.Integration;

public class FilePostCreatedEventHandler : IEventHandler<FilePostCreatedEvent>
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public FilePostCreatedEventHandler(IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    public async Task HandleAsync(IEvent evt)
    {
        var file = ((FilePostCreatedEvent) evt).File;

        await _publishEndpoint.Publish(_mapper.Map<FileCreated>(file));
    }
}