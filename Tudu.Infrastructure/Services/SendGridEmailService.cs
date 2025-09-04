using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Tudu.Application.Interfaces;

namespace Tudu.Infrastructure.Services;

public class SendGridEmailService : IEmailService
    {
    private readonly SendGridSettings _settings;
    private readonly ILogger<SendGridEmailService> _logger;


    public SendGridEmailService(IOptions<SendGridSettings> settings, ILogger<SendGridEmailService> logger)
        {
        _settings = settings.Value;
          _logger = logger;
        }

    public async Task SendEmailAsync(string to, string subject, string body)
        {
        try
            {
            var client = new SendGridClient(_settings.ApiKey);
            var from = new EmailAddress(_settings.SenderEmail, _settings.SenderName);
            var toEmail = new EmailAddress(to);

            var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, body, null);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
                {
                _logger.LogInformation("Email sent successfully to {Email} with subject '{Subject}'", to, subject);
                Console.WriteLine($"[Tudu Email] Sent to {to} | Subject: {subject}");
                }
            else
                {
                _logger.LogWarning("Failed to send email to {Email}. Status Code: {StatusCode}", to, response.StatusCode);
                Console.WriteLine($"[Tudu Email] Failed to send to {to}. Status: {response.StatusCode}");
                }
            }
        catch (Exception ex)
            {
            _logger.LogError(ex, "Error sending email to {Email}", to);
            Console.WriteLine($"[Tudu Email] Exception sending to {to}: {ex.Message}");
            }
    }
    }

