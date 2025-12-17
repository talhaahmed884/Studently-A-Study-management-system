namespace Studently.Common.Exception;

public abstract class BaseException : System.Exception
{
    public IErrorCode ErrorCode { get; }
    public object[] Args { get; }

    public string GetCode() {
        return ErrorCode.GetCode();
    }

    protected BaseException(IErrorCode errorCode, params object[] args)
        : base(errorCode.GetMessage(args))
    {
        ErrorCode = errorCode;
        Args = args;
    }

    protected BaseException(IErrorCode errorCode, System.Exception innerException, params object[] args)
        : base(errorCode.GetMessage(args), innerException)
    {
        ErrorCode = errorCode;
        Args = args;
    }
}
