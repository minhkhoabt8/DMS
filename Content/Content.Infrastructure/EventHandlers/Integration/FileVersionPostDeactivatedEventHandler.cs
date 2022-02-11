using Content.Core.Events.Common;
using Content.Core.Events.Integration;
using Content.Infrastructure.EventHandlers.Common;
using MassTransit;
using MessageContracts;

namespace Content.Infrastructure.EventHandlers.Integration;

public class FileVersionPostDeactivatedEventHandler : IEventHandler<FileVersionPostDeactivatedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public FileVersionPostDeactivatedEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task HandleAsync(IEvent evt)
    {
        var version = ((FileVersionPostDeactivatedEvent) evt).Version;

        await _publishEndpoint.Publish(new FileVersionDeactivated(version.ID));
    }
}