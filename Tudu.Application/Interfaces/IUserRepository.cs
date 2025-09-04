using Tudu.Domain.Entities;

namespace Tudu.Application.Interfaces;

public interface IUserRepository
    {
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();

    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    }

