using Content.Core.Enums;
using Content.Core.Exceptions;

namespace Content.Core.Extensions;

public static class FileTypeExtensions
{
    public static FileType ToFileType(this string type)
    {
        return type switch
        {
            "doc" => FileType.DOC,
            "docx" => FileType.DOC,
            "xls" => FileType.XLS,
            "xlsx" => FileType.XLS,
            "pdf" => FileType.PDF,
            "png" => FileType.IMG,
            "bmp" => FileType.IMG,
            "jpeg" => FileType.IMG,
            "jpg" => FileType.IMG,
            "txt" => FileType.TXT,
            "mp4" => FileType.VID,
            _ => throw new UnsupportedFileTypeException()
        };
    }
}