using Content.Core.Entities;
using Content.Core.Events.Common;

namespace Content.Core.Events.Integration;

public class FileVersionPostDeactivatedEvent : IntegrationEvent
{
    public FileVersionPostDeactivatedEvent(FileVersion version)
    {
        Version = version;
    }

    public FileVersion Version { get; }
}