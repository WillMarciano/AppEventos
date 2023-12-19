using AppEventos.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEventos.Domain.Identity
{
    public class User
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
