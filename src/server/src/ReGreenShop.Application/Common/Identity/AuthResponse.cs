namespace ReGreenShop.Application.Common.Identity;
public class AuthResponse
{
    public bool IsAdmin { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;
}
