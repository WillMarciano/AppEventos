using AppEventos.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace AppEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string? Local { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string? DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Intervalo permitido de 3 a 50 caracteres.")]
        public string? Tema { get; set; }
        [Display(Name = "Qtd Pessoas")]
        [Range(1, 120000, ErrorMessage = "{0} não pode ser menor que 1 e maior que 120.000")]
        public int QtdPessoas { get; set; }
        [Display(Name = "Imagem Url")]
        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "{0} em formato inválido")]
        public string? ImagemUrl { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        //[RegularExpression(@"^\([1-9]{2}\)(?:[2-8]|9[0-9])[0-9]{3}\-[0-9]{4}$", ErrorMessage = "{0} em formato inválido")]
        [Phone(ErrorMessage = "O campo {0} está em formato inválido")]
        public string? Telefone { get; set; }

        [Display(Name = "e-mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "É necessário ser um {0} válido ")]
        public string? Email { get; set; }
        public int UserId { get; set; }
        public UserDto? User { get; set; }
        public IEnumerable<LoteDto>? Lotes { get; set; }
        public IEnumerable<RedeSocialDto>? RedesSociais { get; set; }
        public IEnumerable<PalestranteDto>? Palestrantes { get; set; }
    }
}
