using Studently.Common.Exception;

namespace Studently.User.Entity;

public class UserException : BaseException
{
    public UserException(IErrorCode errorCode, params object[] args)
        : base(errorCode, args)
    {
    }

    public UserException(IErrorCode errorCode, System.Exception innerException, params object[] args)
        : base(errorCode, innerException, args)
    {
    }
}
