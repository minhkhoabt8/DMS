using Content.Core.Exceptions.Common;

namespace Content.Core.Exceptions;

public class NoFileVersionException : HandledException
{
    public NoFileVersionException() : base(404, "No suitable version of this file was found, please upload first")
    {
    }
}