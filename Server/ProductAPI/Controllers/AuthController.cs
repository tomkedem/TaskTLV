using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductAPI.DTOs;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates the user and generates a JWT token.
        /// </summary>
        /// <param name="loginDto">The user's login credentials.</param>
        /// <returns>Returns a JWT token if authentication is successful; otherwise, returns 401 Unauthorized.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation("Attempting to authenticate user: {Username}", loginDto.Username);

            // Authenticate user and generate JWT token
            var token = _authService.AuthenticateUser(loginDto.Username, loginDto.Password);

            if (token == null)
            {
                _logger.LogWarning("Authentication failed for user: {Username}", loginDto.Username);
                return Unauthorized("Invalid username or password");
            }

            var userRole = _authService.GetUserRole(loginDto.Username); // Assumes this method exists to get the user role

            _logger.LogInformation("User {Username} authenticated successfully", loginDto.Username);
            return Ok(new { Token = token, Role = userRole });
        }
    }
}
