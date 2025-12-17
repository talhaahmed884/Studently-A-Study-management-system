using System.Net;
using Studently.Common.Exception;

namespace Studently.Features.User.Entity;

public class AuthenticationErrorCode : IErrorCode
{
    public static readonly AuthenticationErrorCode AuthenticationFailed =
        new("AUTH_001", "Authentication failed for user: {0}", HttpStatusCode.Unauthorized);

    public static readonly AuthenticationErrorCode InvalidCredentials =
        new("AUTH_002", "Invalid email or password", HttpStatusCode.Unauthorized);

    public static readonly AuthenticationErrorCode UserNotAuthenticated =
        new("AUTH_003", "User is not authenticated", HttpStatusCode.Unauthorized);

    public static readonly AuthenticationErrorCode SessionExpired =
        new("AUTH_004", "Session has expired", HttpStatusCode.Unauthorized);

    public string Code { get; }
    public string MessageTemplate { get; }
    public HttpStatusCode HttpStatusCode { get; }

    private AuthenticationErrorCode(string code, string messageTemplate, HttpStatusCode httpStatusCode)
    {
        Code = code;
        MessageTemplate = messageTemplate;
        HttpStatusCode = httpStatusCode;
    }
}
