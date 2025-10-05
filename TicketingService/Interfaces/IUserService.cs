using TicketingService.EntityFramework.Models;

namespace TicketingService.Interfaces
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        string GenerateJwtToken(User user);
    }
}
