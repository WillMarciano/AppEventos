using AppEventos.Domain.Models;

namespace AppEventos.Application.Dtos
{
    public class PalestranteUpdateDto
    {
        public string? MiniCurriculo { get; set; }
        public int UserId { get; set; }
    }
}
