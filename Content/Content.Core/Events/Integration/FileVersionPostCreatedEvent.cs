using Content.Core.Entities;
using Content.Core.Events.Common;

namespace Content.Core.Events.Integration;

public class FileVersionPostCreatedEvent: IntegrationEvent
{
    public FileVersionPostCreatedEvent(FileVersion version)
    {
        Version = version;
    }

    public FileVersion Version { get; }
}