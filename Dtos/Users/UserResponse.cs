namespace CVGeneratorAPI.Dtos;

public class UserResponse
{
    public string? Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
}
