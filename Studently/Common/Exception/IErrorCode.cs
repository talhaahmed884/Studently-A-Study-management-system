namespace Studently.Common.Exception;

public interface IErrorCode
{
    protected string Code { public get; }
    protected string MessageTemplate { public get; }
    protected int HttpStatusCode { public get; }

    public string GetMessage(params object[] args)
    {
        return string.Format(MessageTemplate, args);
    }
}
