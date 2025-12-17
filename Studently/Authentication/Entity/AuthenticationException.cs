using Studently.Common.Exception;

namespace Studently.Authentication.Entity;

public class AuthenticationException : BaseException
{
    public AuthenticationException(IErrorCode errorCode, params object[] args)
        : base(errorCode, args)
    {
    }

    public AuthenticationException(IErrorCode errorCode, System.Exception innerException, params object[] args)
        : base(errorCode, innerException, args)
    {
    }
}
