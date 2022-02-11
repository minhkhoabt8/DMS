namespace Content.Core.Events.Common;

public interface IEvent
{
    public bool IsPublished { get; set; }
}