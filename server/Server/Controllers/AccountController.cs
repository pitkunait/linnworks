using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Server.Authentication;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/auth")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(ILogger<AccountController> logger, IUserService userService, IJwtAuthManager jwtAuthManager, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _httpContextAccessor = httpContextAccessor;

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await _userService.IsValidUserCredentials(request.Username, request.Password))
            {
                return Unauthorized();
            }
            
            var role = await _userService.GetUserRole(request.Username);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.Username),
                new Claim(ClaimTypes.Role, role)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.Username, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.Username}] logged in the system.");
            return Ok(new LoginResult
            {
                UserName = request.Username,
                Role = role,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }
        
        
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<ActionResult> Register([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _userService.CreateUser(request.Username, request.Password);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("user")]
        [Authorize]
        public ActionResult GetCurrentUser()
        {
            return Ok(new LoginResult
            {
                UserName = User.Identity?.Name,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                OriginalUserName = User.FindFirst("OriginalUserName")?.Value
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            var userName = User.Identity?.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
            _logger.LogInformation($"User [{userName}] logged out the system.");
            return Ok();
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var userName = User.Identity?.Name;
                _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }
                string header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                header = header.Remove(0, 7);

                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, header, DateTime.Now);
                _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
                return Ok(new LoginResult
                {
                    UserName = userName,
                    Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpPost("impersonation")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> Impersonate([FromBody] ImpersonationRequest request)
        {
            var userName = User.Identity?.Name;
            _logger.LogInformation($"User [{userName}] is trying to impersonate [{request.UserName}].");

            var impersonatedRole = await _userService.GetUserRole(request.UserName);
            if (string.IsNullOrWhiteSpace(impersonatedRole))
            {
                _logger.LogInformation($"User [{userName}] failed to impersonate [{request.UserName}] due to the target user not found.");
                return BadRequest($"The target user [{request.UserName}] is not found.");
            }
            if (impersonatedRole == UserRoles.Admin)
            {
                _logger.LogInformation($"User [{userName}] is not allowed to impersonate another Admin.");
                return BadRequest("This action is not supported.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.UserName),
                new Claim(ClaimTypes.Role, impersonatedRole),
                new Claim("OriginalUserName", userName ?? string.Empty)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.UserName}] is impersonating [{request.UserName}] in the system.");
            return Ok(new LoginResult
            {
                UserName = request.UserName,
                Role = impersonatedRole,
                OriginalUserName = userName,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        [HttpPost("stop-impersonation")]
        public async Task<ActionResult> StopImpersonation()
        {
            var userName = User.Identity?.Name;
            var originalUserName = User.FindFirst("OriginalUserName")?.Value;
            if (string.IsNullOrWhiteSpace(originalUserName))
            {
                return BadRequest("You are not impersonating anyone.");
            }
            _logger.LogInformation($"User [{originalUserName}] is trying to stop impersonate [{userName}].");

            var role = await _userService.GetUserRole(originalUserName);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,originalUserName),
                new Claim(ClaimTypes.Role, role)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);
            _logger.LogInformation($"User [{originalUserName}] has stopped impersonation.");
            return Ok(new LoginResult
            {
                UserName = originalUserName,
                Role = role,
                OriginalUserName = null,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }
    }

    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class LoginResult
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("originalUserName")]
        public string OriginalUserName { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

    public class ImpersonationRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}