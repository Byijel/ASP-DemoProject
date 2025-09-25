using System;
using System.Threading.Tasks;
using Ap.Demo.Application.Interfaces;
using Ap.Demo.Infrastructure.Configuration;
using Ap.Demo.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AP.Demo.Unittests.Services
{
    [TestClass]
    public class EmailServiceTests
    {
        private Mock<IOptions<SmtpSettings>> _smtpOptionsMock = null!;
        private Mock<ILogger<EmailService>> _loggerMock = null!;
        private EmailService _emailService = null!;
        private SmtpSettings _smtpSettings = null!;

        [TestInitialize]
        public void Setup()
        {
            _smtpSettings = new SmtpSettings
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Username = "test@gmail.com",
                Password = "testpassword",
                FromEmail = "test@gmail.com",
                FromName = "Test Application",
                EnableSsl = true
            };

            _smtpOptionsMock = new Mock<IOptions<SmtpSettings>>();
            _smtpOptionsMock.Setup(o => o.Value).Returns(_smtpSettings);
            
            _loggerMock = new Mock<ILogger<EmailService>>();
            
            _emailService = new EmailService(_smtpOptionsMock.Object, _loggerMock.Object);
        }

        [DataTestMethod]
        [DataRow("Host", "")]
        [DataRow("Username", "")]
        [DataRow("Password", "")]
        public async Task SendEmailAsync_MissingSmtpSettings_FallsBackToConsoleLogging(string settingName, string emptyValue)
        {
            // Arrange
            switch (settingName)
            {
                case "Host":
                    _smtpSettings.Host = emptyValue;
                    break;
                case "Username":
                    _smtpSettings.Username = emptyValue;
                    break;
                case "Password":
                    _smtpSettings.Password = emptyValue;
                    break;
            }
            
            var to = "admin@test.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            await _emailService.SendEmailAsync(to, subject, body);

            // Assert
            VerifyLoggerWarning("SMTP settings not configured properly. Falling back to console logging.");
        }

        [TestMethod]
        public async Task SendEmailAsync_ValidSmtpSettings_LogsAttempt()
        {
            // Arrange
            var to = "admin@test.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            try
            {
                await _emailService.SendEmailAsync(to, subject, body);
            }
            catch
            {
                // Expected to fail in unit test environment without real SMTP server
            }

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task SendEmailAsync_SmtpException_FallsBackToConsoleLogging()
        {
            // Arrange
            var to = "admin@test.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            await _emailService.SendEmailAsync(to, subject, body);

            // Assert 
            VerifyLoggerError("Failed to send email to admin@test.com with subject 'Test Subject'. Falling back to console logging.");
        }

        [TestMethod]
        public async Task SendEmailAsync_NullToAddress_HandlesGracefully()
        {
            // Arrange
            string? to = null;
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            await _emailService.SendEmailAsync(to!, subject, body);

            // Assert - fall back to console logging
            VerifyLoggerError("Failed to send email to (null) with subject 'Test Subject'. Falling back to console logging.");
        }

        [DataTestMethod]
        [DataRow("admin@test.com", "", "Test Body", "EmptySubject")]
        [DataRow("admin@test.com", "Test Subject", "", "EmptyBody")]
        public async Task SendEmailAsync_EmptyParameters_HandlesGracefully(string to, string subject, string body, string testCase)
        {
            // Arrange - data row

            // Act
            await _emailService.SendEmailAsync(to, subject, body);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }

        private void VerifyLoggerWarning(string expectedMessage)
        {
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(expectedMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }

        private void VerifyLoggerError(string expectedMessage)
        {
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(expectedMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }
    }
}
