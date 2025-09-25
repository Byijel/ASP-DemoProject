using System.Threading.Tasks;

namespace Ap.Demo.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
