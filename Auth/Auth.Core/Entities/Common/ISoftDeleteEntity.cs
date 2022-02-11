namespace Auth.Core.Entities.Common;

public interface ISoftDeleteEntity
{
    public bool IsActive { get; set; }
}

public abstract class SoftDeleteEntity : ISoftDeleteEntity
{
    public virtual bool IsActive { get; set; } = true;
}