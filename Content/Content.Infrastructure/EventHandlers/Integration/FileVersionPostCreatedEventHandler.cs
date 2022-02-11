using AutoMapper;
using Content.Core.Events.Common;
using Content.Core.Events.Integration;
using Content.Infrastructure.EventHandlers.Common;
using MassTransit;
using MessageContracts;

namespace Content.Infrastructure.EventHandlers.Integration;

public class FileVersionPostCreatedEventHandler : IEventHandler<FileVersionPostCreatedEvent>
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public FileVersionPostCreatedEventHandler(IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task HandleAsync(IEvent evt)
    {
        var version = ((FileVersionPostCreatedEvent) evt).Version;

        await _publishEndpoint.Publish(_mapper.Map<FileVersionCreated>(version));
    }
}