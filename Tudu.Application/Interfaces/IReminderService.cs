namespace Tudu.Application.Interfaces;

public interface IReminderService
    {
    Task SendDueRemindersAsync();
    }
