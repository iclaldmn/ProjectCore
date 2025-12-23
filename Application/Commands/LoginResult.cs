namespace Application.Commands;

public class LoginResult
{
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}