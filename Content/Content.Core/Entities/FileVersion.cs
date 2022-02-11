using Content.Core.Entities.Common;
using Content.Core.Enums;
using Content.Core.Events.Integration;
using Content.Core.Extensions;

namespace Content.Core.Entities;

/// <summary>
/// Represents a version of a file
/// </summary>
public class FileVersion : EntityWithEvents
{
    private FileVersion()
    {
    }

    public static FileVersion Create(long size, string fileName, FileType type, Guid uploaderID)
    {
        var version = new FileVersion
        {
            VersionNumber = DateTime.Now.Ticks,
            Size = size,
            FileName = fileName,
            Type = type,
            UploaderID = uploaderID
        };

        version.Events.Add(new FileVersionPostCreatedEvent(version));

        return version;
    }

    public int ID { get; set; }
    public Guid FileID { get; set; }
    public File File { get; set; }
    public string FileName { get; set; }
    public string? FileUrl { get; private set; }

    /// <summary>
    /// Url of the delta file
    /// </summary>
    /// <remarks>Delta file is a file containing instructions for transforming the base version's file to this version</remarks>
    public string? DeltaUrl { get; private set; }

    public long DeltaSize { get; private set; }

    /// <summary>
    /// Higher means newer version
    /// </summary>
    public long VersionNumber { get; set; }

    /// <summary>
    /// Whether the version's file data is done uploading and can be accessed
    /// </summary>
    public bool IsReady { get; private set; } = false;

    /// <summary>
    /// Whether the version is deleted
    /// </summary>
    public bool IsActive { get; private set; } = true;

    public DateTime UploadedAt { get; set; } = DateTime.Now.SetKindUtc();
    public Guid UploaderID { get; set; }
    public Account Uploader { get; set; }
    public int? BaseVersionID { get; set; }
    public FileVersion? BaseVersion { get; set; }

    /// <summary>
    /// File size, measured in bytes
    /// </summary>
    public long Size { get; set; }

    public FileType Type { get; set; }

    private void SetReady()
    {
        if (!IsReady)
        {
            IsReady = true;

            // Only send event if version is still active
            if (IsActive)
            {
                Events.Add(new FileVersionPostReadyEvent(this));
            }
        }
    }

    public void Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            Events.Add(new FileVersionPostDeactivatedEvent(this));
        }
    }

    public void BaseOn(FileVersion version)
    {
        BaseVersion = version;
    }

    /// <summary>
    /// Whether version can be viewed
    /// </summary>
    /// <returns></returns>
    public bool IsAvailableForViewing()
    {
        return IsReady && IsActive;
    }

    public void SetFileUrl(string url)
    {
        FileUrl = url;
        SetReady();
    }

    public void SetDelta(long size, string url)
    {
        DeltaSize = size;
        DeltaUrl = url;
        SetReady();
    }
}