namespace CVGeneratorAPI.Dtos;

public class UpdateUserRequest
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string? Password { get; set; } // optional if user doesn't want to change it
}
