using Auth.Core.Exceptions.Common;

namespace Auth.Core.Exceptions;

public class UnauthorizedException : HandledException
{
    public UnauthorizedException() : base(403, "Unauthorized")
    {
    }

    public UnauthorizedException(string resourceName) : base(403,
        $"User is not authorized to access this {resourceName}")
    {
    }
}

public class UnauthorizedException<T> : UnauthorizedException
{
    public UnauthorizedException() : base(typeof(T).Name)
    {
    }
}