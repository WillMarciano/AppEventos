using Microsoft.AspNetCore.Identity;

namespace AppEventos.Domain.Identity
{
    public class Role : IdentityRole<int>
    {
        public IEnumerable<UserRole>? UserRoles { get; set; }
    }
}
