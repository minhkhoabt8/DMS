using Metadata.Core.Events.Common;
using File = Metadata.Core.Entities.File;

namespace Metadata.Core.Events.Integration;

public class FilePostCreatedEvent : IntegrationEvent
{
    public File File { get; }

    public FilePostCreatedEvent(File file)
    {
        File = file;
    }
}