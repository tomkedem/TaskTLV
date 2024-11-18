using Microsoft.IdentityModel.Tokens;
using ProductAPI.Entities.ProductAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    // Authenticate the user and generate a JWT token if valid
    public string AuthenticateUser(string username, string password)
    {
        // Validate user credentials
        var user = _userRepository.GetUserByUsername(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null; // Return null for invalid credentials
        }

        // Generate JWT token
        return GenerateJwtToken(user);
    }

    // Generate JWT Token
    private string GenerateJwtToken(User user)
    {
        // Validate JWT configuration
        string jwtKey = _config["Jwt:Key"];
        string jwtIssuer = _config["Jwt:Issuer"];
        string jwtAudience = _config["Jwt:Audience"];

        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
        {
            throw new InvalidOperationException("JWT configuration is missing or invalid.");
        }

        // Token expiration time
        int tokenExpirationHours = _config.GetValue<int>("Jwt:TokenExpirationHours", 1);
        var expirationTime = DateTime.UtcNow.AddHours(tokenExpirationHours);

        // Claims for the JWT token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Create signing credentials
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create the token
        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: expirationTime,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Retrieve the user's role
    public string GetUserRole(string username)
    {
        var user = _userRepository.GetUserByUsername(username);
        return user?.Role ?? "Unknown"; // Return "Unknown" if no role is found
    }
}
