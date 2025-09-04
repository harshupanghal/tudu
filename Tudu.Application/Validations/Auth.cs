
using FluentValidation;
using Tudu.Application.DTOs;

namespace Tudu.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
    public RegisterRequestValidator()
        {
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress().MaximumLength(256);

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 20).WithMessage("Username must be between 3 and 20 characters.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#\$%\^&\*\!\?\-\+=]).+$")
            .WithMessage("Password must contain at least 1 uppercase, 1 lowercase, 1 digit, and 1 special character.");
        }
    }

public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
    public LoginRequestValidator()
        {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
