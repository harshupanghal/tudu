using Tudu.Application.Dtos;

namespace Tudu.Application.Interfaces;

public interface IUserTaskService
    {
    Task<IEnumerable<TaskReadDto>> GetUserTasksAsync(int userId);
    Task<TaskReadDto?> GetTaskByIdAsync(int id, int userId);
    Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto, int userId);
    Task<bool> UpdateTaskAsync(TaskUpdateDto taskDto, int userId);
    Task<bool> DeleteTaskAsync(int id, int userId);
    }
