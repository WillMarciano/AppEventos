using AppEventos.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace AppEventos.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public required string Nome { get; set; }
        public required string Sobrenome { get; set; }
        public required Titulo Titulo { get; set; }
        public string? Descricao { get; set; }
        public Funcao Funcao { get; set; }
        public string? ImagemUrl { get; set; }
        public IEnumerable<UserRole>? UserRoles { get; set; }

    }
}
