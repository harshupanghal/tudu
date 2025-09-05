namespace Tudu.Application.Dtos;

public class TaskCreateDto
    {
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Category { get; set; } = string.Empty;
    public bool HasReminder { get; set; }
    public DateTime? ReminderTime { get; set; }
    }

public class TaskUpdateDto
    {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public bool HasReminder { get; set; }
    public DateTime? ReminderTime { get; set; }
    }

public class TaskReadDto
    {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public bool HasReminder { get; set; }
    public DateTime? ReminderTime { get; set; }
    }
