using System.ComponentModel.DataAnnotations.Schema;
using Tudu.Domain.Entities;

namespace Tudu.Domain.Entities;

public class UserTask
    {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public bool HasReminder { get; set; }
    public DateTime? ReminderTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string? ReminderJobId { get; set; }        
    public bool ReminderSent { get; set; } = false;   
    public DateTime? ReminderSentAt { get; set; }
    }
