using System.Net;
using Studently.Common.Exception;

namespace Studently.User.Entity;

public class UserErrorCode : IErrorCode
{
    public static readonly UserErrorCode UserNotFound =
        new("USER_001", "User not found with {0}: {1}", HttpStatusCode.NotFound);

    public static readonly UserErrorCode UserAlreadyExists =
        new("USER_002", "User already exists with email: {0}", HttpStatusCode.Conflict);

    public static readonly UserErrorCode InvalidUserData =
        new("USER_003", "Invalid user data: {0}", HttpStatusCode.BadRequest);

    public static readonly UserErrorCode UserCreationFailed =
        new("USER_004", "Failed to create user: {0}", HttpStatusCode.InternalServerError);

    public static readonly UserErrorCode UserUpdateFailed =
        new("USER_005", "Failed to update user: {0}", HttpStatusCode.InternalServerError);

    public static readonly UserErrorCode UserDeletionFailed =
        new("USER_006", "Failed to delete user: {0}", HttpStatusCode.InternalServerError);

    public static readonly UserErrorCode InvalidEmailFormat =
        new("USER_007", "Invalid email format: {0}", HttpStatusCode.BadRequest);

    public static readonly UserErrorCode InvalidName =
        new("USER_008", "Invalid name: name cannot be null or empty", HttpStatusCode.BadRequest);

    public static readonly UserErrorCode EmailAlreadyInUse =
        new("USER_009", "Email already in use: {0}", HttpStatusCode.Conflict);

    public string Code { get; }
    public string MessageTemplate { get; }
    public HttpStatusCode HttpStatusCode { get; }

    private UserErrorCode(string code, string messageTemplate, HttpStatusCode httpStatusCode)
    {
        Code = code;
        MessageTemplate = messageTemplate;
        HttpStatusCode = httpStatusCode;
    }
}
