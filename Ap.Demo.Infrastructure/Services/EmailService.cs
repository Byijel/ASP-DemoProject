using Ap.Demo.Application.Interfaces;
using Ap.Demo.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Ap.Demo.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                if (string.IsNullOrEmpty(_smtpSettings.Host) || string.IsNullOrEmpty(_smtpSettings.Username) || string.IsNullOrEmpty(_smtpSettings.Password))
                {
                    _logger.LogWarning("SMTP settings not configured properly. Falling back to console logging.");
                    await LogEmailToConsole(to, subject, body);
                    return;
                }

                //BYPASS SSL CERTIFICATE VALIDATION to send email (for development)
                ServicePointManager.ServerCertificateValidationCallback = 
                    (sender, certificate, chain, sslPolicyErrors) => true;

                using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = _smtpSettings.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.FromEmail ?? _smtpSettings.Username, _smtpSettings.FromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
                
                _logger.LogInformation("Email sent successfully to {To} with subject '{Subject}'", to, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To} with subject '{Subject}'. Falling back to console logging.", to, subject);
                await LogEmailToConsole(to, subject, body);
            }
        }

        private async Task LogEmailToConsole(string to, string subject, string body)
        {
            await Task.Delay(100); // Simulate network delay
            
            Console.WriteLine($"=== EMAIL NOTIFICATION (Console Fallback) ===");
            Console.WriteLine($"To: {to}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body: {body}");
            Console.WriteLine($"Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
            Console.WriteLine($"============================================");
        }
    }
}
