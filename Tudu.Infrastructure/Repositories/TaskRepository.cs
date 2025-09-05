
using Microsoft.EntityFrameworkCore;
using Tudu.Application.Interfaces;
using Tudu.Domain.Entities;
using Tudu.Infrastructure.Context;

namespace Tudu.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
    {
    private readonly TuduDbContext _context;

    public TaskRepository(TuduDbContext context)
        {
        _context = context;
        }

    public async Task<IEnumerable<UserTask>> GetUserTasksAsync(int userId)
        {
        return await _context.UserTasks
                             .Where(t => t.UserId == userId)
                             .ToListAsync();
        }

    public async Task<UserTask?> GetTaskByIdAsync(int id, int userId)
        {
        return await _context.UserTasks
                             .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

    public async Task AddTaskAsync(UserTask task)
        {
        await _context.UserTasks.AddAsync(task);
        }

    public void UpdateTask(UserTask task)
        {
        _context.UserTasks.Update(task);
        }

    public void DeleteTask(UserTask task)
        {
        _context.UserTasks.Remove(task);
        }

    public async Task<bool> SaveChangesAsync()
        {
        return await _context.SaveChangesAsync() >= 0;
        }
    }
