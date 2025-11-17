using BoardGameManager.Application.DTOs.Auth;

namespace BoardGameManager.Application.Services;

public interface IAuthService
{
  Task<AuthResponse> RegisterAsync(RegisterRequest request);
  Task<AuthResponse> LoginAsync(LoginRequest request);
  Task<UserResponse?> GetUserByIdAsync(int userId);
}