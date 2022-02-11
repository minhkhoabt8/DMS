using Metadata.Core.Entities;
using Metadata.Core.Events.Common;

namespace Metadata.Core.Events.Integration;

public class FolderPostCreatedEvent : IntegrationEvent
{
    public FolderPostCreatedEvent(Folder folder)
    {
        Folder = folder;
    }

    public Folder Folder { get; set; }
}