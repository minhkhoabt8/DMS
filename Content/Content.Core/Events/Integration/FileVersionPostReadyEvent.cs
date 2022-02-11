using Content.Core.Entities;
using Content.Core.Events.Common;

namespace Content.Core.Events.Integration;

public class FileVersionPostReadyEvent : IntegrationEvent
{
    public FileVersionPostReadyEvent(FileVersion version)
    {
        Version = version;
    }

    public FileVersion Version { get; }
}