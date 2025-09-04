using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tudu.Application.Interfaces;

namespace Tudu.Infrastructure.Services;

public class ReminderBackgroundService : BackgroundService
    {
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ReminderBackgroundService> _logger;

    public ReminderBackgroundService(IServiceScopeFactory scopeFactory, ILogger<ReminderBackgroundService> logger)
        {
        _scopeFactory = scopeFactory;
        _logger = logger;
        }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
        while (!stoppingToken.IsCancellationRequested)
            {
            using var scope = _scopeFactory.CreateScope();
            var reminderService = scope.ServiceProvider.GetRequiredService<IReminderService>();

            try
                {
                await reminderService.SendDueRemindersAsync();
                _logger.LogInformation("checking for due tasks");
                }
            catch (Exception ex)
                {
                _logger.LogError(ex, "Error sending reminders");
                }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // check every minute
            }
        }
    }
