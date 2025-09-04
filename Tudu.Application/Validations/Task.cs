// Application/Validation/TaskValidator.cs

using FluentValidation;
using Tudu.Application.Dtos;

namespace Tudu.Application.Validation
    {
    public class TaskCreateDtoValidator : AbstractValidator<TaskCreateDto>
        {
        public TaskCreateDtoValidator()
            {
            RuleFor(t => t.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title can't be longer than 100 characters.");

            RuleFor(t => t.DueDate)
                .GreaterThan(DateTime.Now.AddMinutes(-1)).WithMessage("Due date must be in the future.");

            RuleFor(t => t.Category)
                .NotEmpty().WithMessage("Category is required.");

            When(t => t.HasReminder, () => {
                RuleFor(t => t.ReminderTime)
                    .NotNull().WithMessage("Reminder time is required if a reminder is set.")
                    .GreaterThan(t => DateTime.Now.AddMinutes(-1)).WithMessage("Reminder time must be in the future.");
            });
            }
        }

    public class TaskUpdateDtoValidator : AbstractValidator<TaskUpdateDto>
        {
        public TaskUpdateDtoValidator()
            {
            RuleFor(t => t.Id).NotEmpty().WithMessage("Task Id is required for update.");

            RuleFor(t => t.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title can't be longer than 100 characters.");

            RuleFor(t => t.DueDate)
                .GreaterThan(t => DateTime.Now.AddDays(-365)).WithMessage("Due date must be a valid date.");
            }
        }
    }