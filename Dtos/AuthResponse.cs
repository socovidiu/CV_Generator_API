namespace CVGeneratorAPI.Dtos;

public class AuthResponse
{
    public required string Message { get; set; }
    public UserResponse? User { get; set; }
}
