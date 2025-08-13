namespace CVGeneratorAPI.Settings;

public class JwtSettings
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Secret { get; set; } = default!; // 32+ chars for HS256
    public int ExpMinutes { get; set; } = 60;
}
