using System.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tudu.Application.Interfaces;
using Tudu.Domain.Entities;
using Tudu.Infrastructure.Context;
using Tudu.Infrastructure.Services;
namespace Tudu.Application.Services;

public class ReminderService : IReminderService
    {
    private readonly TuduDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<IReminderService> _logger;

    public ReminderService(TuduDbContext context, IEmailService emailService, ILogger<IReminderService> logger)
        {
        _context = context;
        _emailService = emailService;
        
       
            _logger = logger;
            
        }

    public async Task SendDueRemindersAsync()
        {
        var now = DateTime.Now;

        var dueTasks = await _context.UserTasks
            .Include(t => t.User)
            .Where(t => t.HasReminder
                        && t.ReminderTime <= now
                        && !t.IsCompleted
                        && !t.ReminderSent) 
            .ToListAsync();

        _logger.LogInformation("{Count} tasks found for reminders", dueTasks.Count);

        foreach (var task in dueTasks)
            {
            if (task.User.Email is not null)
                {
                var subject = $"Reminder: {task.Title}";
                var body = $"Hi {task.User.UserName},\n\n" +
                           $"This is a reminder for your task:\n" +
                           $"{task.Title}\n\n" +
                           $"Due on: {task.DueDate}\n\n" +
                           "Good luck! \n- Tudu Team";

                await _emailService.SendEmailAsync(task.User.Email, subject, body);

                _logger.LogInformation("Reminder sent for Task ID {TaskId} to user {Email}", task.Id, task.User.Email);
                task.ReminderSent = true;
                }
            }

        await _context.SaveChangesAsync();
        }

    }
