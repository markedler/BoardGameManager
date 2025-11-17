namespace BoardGameManager.Application.DTOs.Auth;

public class UserResponse
{
  public int Id { get; set; }
  public string Username { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public DateTime DateCreated { get; set; }
  public DateTime? LastLogin { get; set; }
}