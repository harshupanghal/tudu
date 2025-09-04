using System.ComponentModel.DataAnnotations;

namespace Tudu.Application.DTOs;
public class RegisterRequest
    
    {
    //[Required(ErrorMessage = "Username is required.")]
    //[StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
    //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
    public string UserName { get; set; } = string.Empty;

    //[Required(ErrorMessage = "Password is required.")]
    //[StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    //[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#\$%\^&\*\!\?\-\+=]).+$",
    //ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, 1 digit, and 1 special character (@# $% ^& * ! ? - + =).")]
    public string Password { get; set; } = string.Empty;
    public string Email {  get; set; } = string.Empty;
    public string ProfilePicturePath { get; set; } = string.Empty;
    }

public class LoginRequest
    {
    //[Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; } = string.Empty;
    //[Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
    }

public class AuthResponse
    {
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? ProfilePicturePath { get; set; }
    public string? Email { get; set; }
    public string? message { get; set; }
    public bool Success { get; set; }
    }
