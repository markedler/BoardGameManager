using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BoardGameManager.Application.DTOs.Auth;
using BoardGameManager.Application.Services;
using BoardGameManager.Domain.Entities;
using BoardGameManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BoardGameManager.Infrastructure.Services;

public class AuthService : IAuthService
{
  private readonly ApplicationDbContext _context;
  private readonly IConfiguration _configuration;

  public AuthService(ApplicationDbContext context, IConfiguration configuration)
  {
    _context = context;
    _configuration = configuration;
  }

  public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
  {
    // Check if username already exists
    if (await _context.Users.AnyAsync(u => u.Username == request.Username))
    {
      throw new InvalidOperationException("Username already exists");
    }

    // Check if email already exists
    if (await _context.Users.AnyAsync(u => u.Email == request.Email))
    {
      throw new InvalidOperationException("Email already exists");
    }

    // Validate password strength (basic validation)
    if (request.Password.Length < 8)
    {
      throw new InvalidOperationException("Password must be at least 8 characters");
    }

    // Hash password
    var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

    // Create user
    var user = new User
    {
      Username = request.Username,
      Email = request.Email,
      PasswordHash = passwordHash,
      DateCreated = DateTime.UtcNow
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    // Generate JWT token
    var token = GenerateJwtToken(user);

    return new AuthResponse
    {
      UserId = user.Id,
      Username = user.Username,
      Email = user.Email,
      Token = token
    };
  }

  public async Task<AuthResponse> LoginAsync(LoginRequest request)
  {
    // Find user by username
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

    if (user == null)
    {
      throw new UnauthorizedAccessException("Invalid username or password");
    }

    // Verify password
    if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
    {
      throw new UnauthorizedAccessException("Invalid username or password");
    }

    // Update last login
    user.LastLogin = DateTime.UtcNow;
    await _context.SaveChangesAsync();

    // Generate JWT token
    var token = GenerateJwtToken(user);

    return new AuthResponse
    {
      UserId = user.Id,
      Username = user.Username,
      Email = user.Email,
      Token = token
    };
  }

  public async Task<UserResponse?> GetUserByIdAsync(int userId)
  {
    var user = await _context.Users.FindAsync(userId);

    if (user == null)
    {
      return null;
    }

    return new UserResponse
    {
      Id = user.Id,
      Username = user.Username,
      Email = user.Email,
      DateCreated = user.DateCreated,
      LastLogin = user.LastLogin
    };
  }

  private string GenerateJwtToken(User user)
  {
    var jwtSettings = _configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
    var issuer = jwtSettings["Issuer"] ?? "BoardGameManager";
    var audience = jwtSettings["Audience"] ?? "BoardGameManager";

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}