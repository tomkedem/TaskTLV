using Microsoft.IdentityModel.Tokens;
using ProductAPI.Data;
using ProductAPI.Entities;
using ProductAPI.Entities.ProductAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService
{
    private readonly ApplicationDbContext _context; // Add DbContext for user validation
    private readonly IConfiguration _config;

    public AuthService(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    // Method to authenticate the user
    public string AuthenticateUser(string username, string password)
    {
        // Check if the user exists in the database
        var user = _context.Users.SingleOrDefault(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password)) // Verify password
        {
            return null; // Return null if invalid credentials
        }

        // If credentials are valid, generate a JWT token
        return GenerateJwtToken(user);
    }

    // Validate user credentials by checking if the username exists and the password matches
    public bool ValidateUserCredentials(string username, string password)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == username); // Check if the user exists
        if (user == null)
            return false;  // Return false if user doesn't exist

        // Use BCrypt to verify if the entered password matches the hashed password stored in the database
        return BCrypt.Net.BCrypt.Verify(password, user.Password);  // Verifies password against the hashed password
    }

    // Generate JWT Token after user authentication
    public string GenerateJwtToken(User user)
    {
        // Ensure JWT configuration is available
        if (string.IsNullOrEmpty(_config["Jwt:Key"]) ||
            string.IsNullOrEmpty(_config["Jwt:Issuer"]) ||
            string.IsNullOrEmpty(_config["Jwt:Audience"]))
        {
            throw new InvalidOperationException("JWT configuration is missing.");
        }

        var tokenExpirationHours = _config.GetValue<int>("Jwt:TokenExpirationHours", 1);
        var expirationTime = DateTime.UtcNow.AddHours(tokenExpirationHours);

        // Define claims for the token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
        };

        // Create security key using the key from appsettings
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])); // make sure this key is same as in appsettings.json

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Generate JWT token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expirationTime,  // Set expiration time for the token
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GetUserRole(string username)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == username);
        return user?.Role;
    }
}
