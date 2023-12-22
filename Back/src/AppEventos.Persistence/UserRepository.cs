using AppEventos.Domain.Identity;
using AppEventos.Repository.Context;
using AppEventos.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppEventos.Repository
{
    public class UserRepository : GeralRepository, IUserRepository
    {
        private new readonly AppEventosContext _context;

        public UserRepository(AppEventosContext context) : base(context) 
            => _context = context;

        public async Task<User?> GetUserbyIdAsync(int id) 
            => await _context.Users.FindAsync(id);

        public async Task<User?> GetUserByUsernameAsync(string username) 
            => await _context.Users.SingleOrDefaultAsync(x => x.UserName == username.ToLower());

        public async Task<IEnumerable<User>> GetUsersAsync() 
            => await _context.Users.ToListAsync();
    }
}
