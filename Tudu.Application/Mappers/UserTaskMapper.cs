// Application/Mappers/UserTaskMappers.cs

using Tudu.Application.Dtos;
using Tudu.Domain.Entities;

namespace Tudu.Application.Mappers;

public static class UserTaskMappers
    {
    public static TaskReadDto ToReadDto(this UserTask task)
        {
        return new TaskReadDto
            {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            Category = task.Category,
            IsCompleted = task.IsCompleted,
            HasReminder = task.HasReminder,
            ReminderTime = task.ReminderTime
            };
        }

    public static UserTask ToEntity(this TaskCreateDto dto, int userId)
        {
        return new UserTask
            {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Category = dto.Category,
            HasReminder = dto.HasReminder,
            ReminderTime = dto.ReminderTime,
            UserId = userId,
            IsCompleted = false
            };
        }

    public static void ToEntity(this TaskUpdateDto dto, UserTask taskToUpdate)
        {
        taskToUpdate.Title = dto.Title;
        taskToUpdate.Description = dto.Description;
        taskToUpdate.DueDate = dto.DueDate;
        taskToUpdate.Category = dto.Category;
        taskToUpdate.IsCompleted = dto.IsCompleted;
        taskToUpdate.HasReminder = dto.HasReminder;
        taskToUpdate.ReminderTime = dto.ReminderTime;
        }
    }
