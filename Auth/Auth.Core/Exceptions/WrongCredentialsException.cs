using Auth.Core.Exceptions.Common;

namespace Auth.Core.Exceptions;

public class WrongCredentialsException : HandledException
{
    public WrongCredentialsException() : base(403, "Username or Password is incorrect")
    {
    }
}