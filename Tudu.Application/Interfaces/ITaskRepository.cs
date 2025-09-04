// Application/Interfaces/ITaskRepository.cs

using Tudu.Domain.Entities;

namespace Tudu.Application.Interfaces;

public interface ITaskRepository
    {
    Task<IEnumerable<UserTask>> GetUserTasksAsync(int userId);
    Task<UserTask?> GetTaskByIdAsync(int id, int userId);
    Task AddTaskAsync(UserTask task);
    void UpdateTask(UserTask task);
    void DeleteTask(UserTask task);
    Task<bool> SaveChangesAsync();
    }
