using Tudu.Application.DTOs;

namespace Tudu.Application.Interfaces;

public interface IUserService
    {
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse?> GetUserByIdAsync(int id);
    }
