using Application.Interfaces;
using Tudu.Application.DTOs;
using Tudu.Application.Interfaces;
using Tudu.Application.Mappers;
using Tudu.Domain.Entities;

namespace Tudu.Application.Services;

public class UserService : IUserService
    {
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
        var existingUserName = await _userRepository.GetByUsernameAsync(request.UserName);
        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUserName != null )
            return new AuthResponse { Success = false, message = "Username already exists." };
        if (existingEmail != null)
            return new AuthResponse { Success = false, message = "Email already in use" };

        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        var user = new User
            {
            UserName = request.UserName,
            Password = hashedPassword, // store hash instead of raw password
            ProfilePicturePath = request.ProfilePicturePath,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
            };

        await _userRepository.AddAsync(user);
        return user.ToAuthResponse("User registered successfully.");
        }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
        var user = await _userRepository.GetByUsernameAsync(request.UserName);
        if (user == null)
            return new AuthResponse { Success = false, message = "Invalid credentials." };

        var isValidPassword = _passwordHasher.VerifyPassword(user.Password, request.Password);
        if (!isValidPassword)
            return new AuthResponse { Success = false, message = "Invalid credentials." };

        return user.ToAuthResponse("Login successful.");
        }

    public async Task<AuthResponse?> GetUserByIdAsync(int id)
        {
        var user = await _userRepository.GetByIdAsync(id);
        return user?.ToAuthResponse();
        }
    }

