using appEventos.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace appEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string? Local { get; set; }
        public string? DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"),
            StringLength(50, MinimumLength = 3, ErrorMessage = "Intervalo permitido de 3 a 50 caracteres.")]
        public string? Tema { get; set; }
        public int QtdPessoas { get; set; }
        public string? ImagemUrl { get; set; }
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "O campo {0} precisa ser um e-mail válido ")]
        public string? Email { get; set; }
        public IEnumerable<LoteDto>? Lotes { get; set; }
        public IEnumerable<RedeSocialDto>? RedesSociais { get; set; }
        public IEnumerable<PalestranteDto>? Palestrantes { get; set; }
    }
}
