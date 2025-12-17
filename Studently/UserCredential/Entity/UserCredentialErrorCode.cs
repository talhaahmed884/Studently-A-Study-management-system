using System.Net;
using Studently.Common.Exception;

namespace Studently.UserCredential.Entity;

public class UserCredentialErrorCode : IErrorCode
{
    public static readonly UserCredentialErrorCode CredentialNotFound = new("CRED_001", "Credentials not found for user: %s", HttpStatusCode.NotFound);
    public static readonly UserCredentialErrorCode InvalidPassword = new("CRED_002", "Invalid password provided", HttpStatusCode.BadRequest);
    public static readonly UserCredentialErrorCode PasswordHashFailed = new("CRED_003", "Failed to hash password: %s", HttpStatusCode.InternalServerError);
    public static readonly UserCredentialErrorCode PasswordVerificationFailed = new("CRED_004", "Failed to verify password: %s", HttpStatusCode.InternalServerError);
    public static readonly UserCredentialErrorCode CredentialCreationFailed = new("CRED_005", "Failed to create credentials: %s", HttpStatusCode.InternalServerError);
    public static readonly UserCredentialErrorCode CredentialUpdateFailed = new("CRED_006", "Failed to update credentials: %s", HttpStatusCode.InternalServerError);
    public static readonly UserCredentialErrorCode InvalidAlgorithm = new("CRED_007", "Invalid hashing algorithm: %s", HttpStatusCode.BadRequest);
    public static readonly UserCredentialErrorCode PasswordRequired = new("CRED_008", "Password is required", HttpStatusCode.BadRequest);
    public static readonly UserCredentialErrorCode PasswordHashEmpty = new("CRED_009", "Password hash cannot be null or empty", HttpStatusCode.BadRequest);
    public static readonly UserCredentialErrorCode InvalidPasswordHash = new("CRED_010", "Invalid password hash: %s", HttpStatusCode.BadRequest);
    public static readonly UserCredentialErrorCode CredentialAlreadyExists = new("CRED_011", "Credential already exists for user: %s", HttpStatusCode.Conflict);

    public string Code { get; }
    public string MessageTemplate { get; }
    public HttpStatusCode HttpStatusCode { get; }

    private UserCredentialErrorCode(string code, string messageTemplate, HttpStatusCode httpStatusCode)
    {
        Code = code;
        MessageTemplate = messageTemplate;
        HttpStatusCode = httpStatusCode;
    }
}
