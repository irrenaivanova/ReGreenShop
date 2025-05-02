namespace ReGreenShop.Infrastructure.Settings;
public class JwtSettings
{
    public string SignKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
