using Tudu.Application.Dtos;
using Tudu.Application.Interfaces;
using Tudu.Application.Mappers;
using Tudu.Application.Validation;

namespace Tudu.Application.Services;

public class UserTaskService : IUserTaskService
    {
    private readonly ITaskRepository _taskRepository;
    private readonly TaskCreateDtoValidator _createValidator;
    private readonly TaskUpdateDtoValidator _updateValidator;

    public UserTaskService(ITaskRepository taskRepository,
                           TaskCreateDtoValidator createValidator,
                           TaskUpdateDtoValidator updateValidator)
        {
        _taskRepository = taskRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        }

    public async Task<TaskReadDto?> GetTaskByIdAsync(int id, int userId)
        {
        var task = await _taskRepository.GetTaskByIdAsync(id, userId);
        return task?.ToReadDto();
        }

    public async Task<IEnumerable<TaskReadDto>> GetUserTasksAsync(int userId)
        {
        var tasks = await _taskRepository.GetUserTasksAsync(userId);
        return tasks.Select(t => t.ToReadDto());
        }

    public async Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto, int userId)
        {
        var validationResult = _createValidator.Validate(taskDto);
        if (!validationResult.IsValid)
            {
            throw new FluentValidation.ValidationException(validationResult.Errors);
            }

        var task = taskDto.ToEntity(userId);
        await _taskRepository.AddTaskAsync(task);
        await _taskRepository.SaveChangesAsync();
        return task.ToReadDto();
        }

    public async Task<bool> UpdateTaskAsync(TaskUpdateDto taskDto, int userId)
        {
        var validationResult = _updateValidator.Validate(taskDto);
        if (!validationResult.IsValid)
            {
            throw new FluentValidation.ValidationException(validationResult.Errors);
            }

        var taskToUpdate = await _taskRepository.GetTaskByIdAsync(taskDto.Id, userId);
        if (taskToUpdate == null) return false;

        taskDto.ToEntity(taskToUpdate);
        _taskRepository.UpdateTask(taskToUpdate);
        return await _taskRepository.SaveChangesAsync();
        }

    public async Task<bool> DeleteTaskAsync(int id, int userId)
        {
        var taskToDelete = await _taskRepository.GetTaskByIdAsync(id, userId);
        if (taskToDelete == null) return false;

        _taskRepository.DeleteTask(taskToDelete);
        return await _taskRepository.SaveChangesAsync();
        }
    }
