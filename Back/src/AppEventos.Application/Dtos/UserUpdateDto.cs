﻿namespace AppEventos.Application.Dtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? UserName { get; set; }
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Funcao { get; set; }
        public string? Descricao { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
    }
}
