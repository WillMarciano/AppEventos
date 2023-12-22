using AppEventos.Domain.Identity;

namespace AppEventos.Repository.Interfaces
{
    public interface IUserRepository : IGeralRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserbyIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
