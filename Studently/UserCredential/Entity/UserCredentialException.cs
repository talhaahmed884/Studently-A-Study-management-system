using Studently.Common.Exception;

namespace Studently.UserCredential.Entity;

public class UserCredentialException : BaseException
{
    public UserCredentialException(IErrorCode errorCode, params object[] args)
        : base(errorCode, args)
    {
    }

    public UserCredentialException(IErrorCode errorCode, System.Exception innerException, params object[] args)
        : base(errorCode, innerException, args)
    {
    }
}
