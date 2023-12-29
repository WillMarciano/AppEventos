namespace AppEventos.Application.Dtos
{
    public class UserDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Nome { get; set; }
        public required string Sobrenome { get; set; }
    }
}
