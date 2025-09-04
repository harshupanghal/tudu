using System.Security.Claims;

namespace BlazorAuthNoIdentity;

public record LoggedInUserModel(int Id, string UserName, string Email, string ProfilePicturePath)
    {
    public Claim[] ToClaims() =>
        [
            new Claim(ClaimTypes.NameIdentifier, Id.ToString() ?? string.Empty),
            new Claim(ClaimTypes.Name, UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, Email ?? string.Empty),
            new Claim("ProfilePicture", ProfilePicturePath ?? string.Empty)
        ];
    }