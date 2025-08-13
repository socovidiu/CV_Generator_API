namespace CVGeneratorAPI.Dtos;

public class LoginRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; } // plain text for login
}
