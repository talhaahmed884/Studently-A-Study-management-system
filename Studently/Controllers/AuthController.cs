using Microsoft.AspNetCore.Mvc;
using Studently.User.DTO;
using Studently.Authentication.Service;
using Studently.User.Entity;
using Studently.UserCredential.Entity;

namespace Studently.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthenticationService authenticationService,
        ILogger<AuthController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO request)
    {
        try
        {
            _logger.LogInformation("Sign-up attempt for email: {Email}", request.Email);

            var userDto = await _authenticationService.SignUpAsync(request);

            _logger.LogInformation("User registered successfully: {UserId}", userDto.Id);

            return Ok(new SignUpResponse
            {
                Success = true,
                Message = "User registered successfully",
                User = userDto
            });
        }
        catch (UserException ex)
        {
            _logger.LogWarning("Sign-up failed - {Error}", ex.Message);

            return BadRequest(new ErrorResponse
            {
                Success = false,
                Error = ex.Message,
                Code = ex.ErrorCode.Code,
                StatusCode = (int)ex.ErrorCode.HttpStatusCode
            });
        }
        catch (UserCredentialException ex)
        {
            _logger.LogWarning("Sign-up failed - Credential validation error: {Error}", ex.Message);

            return BadRequest(new ErrorResponse
            {
                Success = false,
                Error = ex.Message,
                Code = ex.ErrorCode.Code,
                StatusCode = (int)ex.ErrorCode.HttpStatusCode
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during sign-up for email: {Email}", request.Email);

            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Success = false,
                Error = "An unexpected error occurred during registration",
                Code = "INTERNAL_ERROR"
            });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        try
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);

            bool isAuthenticated = await _authenticationService.LoginAsync(request);

            if (!isAuthenticated)
            {
                _logger.LogWarning("Login failed - Invalid credentials for email: {Email}", request.Email);

                return Unauthorized(new ErrorResponse
                {
                    Success = false,
                    Error = "Invalid email or password",
                    Code = "INVALID_CREDENTIALS"
                });
            }

            // Get user details after successful authentication
            var user = await _authenticationService.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                _logger.LogError("User not found after successful authentication: {Email}", request.Email);

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Success = false,
                    Error = "An unexpected error occurred",
                    Code = "INTERNAL_ERROR"
                });
            }

            _logger.LogInformation("Login successful for user: {UserId}", user.Id);

            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Login successful",
                User = user,
                // TODO: Add JWT token generation here
                // Token = GenerateJwtToken(user)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login for email: {Email}", request.Email);

            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Success = false,
                Error = "An unexpected error occurred during login",
                Code = "INTERNAL_ERROR"
            });
        }
    }

    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "healthy",
            service = "authentication",
            timestamp = DateTime.UtcNow
        });
    }
}

public class SignUpResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public User.DTO.UserDTO? User { get; set; }
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public User.DTO.UserDTO? User { get; set; }
    public string? Token { get; set; } // For future JWT implementation
}

public class ErrorResponse
{
    public bool Success { get; set; }
    public string Error { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int StatusCode { get; set; }
}
