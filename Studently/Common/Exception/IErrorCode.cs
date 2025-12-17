using System.Net;

namespace Studently.Common.Exception;

public interface IErrorCode
{
    public string Code { get; }
    public string MessageTemplate { get; }
    public HttpStatusCode HttpStatusCode { get; }

    public string GetMessage(params object[] args)
    {
        return string.Format(this.MessageTemplate, args);
    }
}
