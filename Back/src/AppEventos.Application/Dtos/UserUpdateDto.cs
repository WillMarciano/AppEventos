﻿namespace AppEventos.Application.Dtos
{
    public class UserUpdateDto
    {
        public required int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Username { get; set; }
        public required string Nome { get; set; }
        public required string Sobrenome { get; set; }
        public required string Email { get; set; }
        public required string Telefone { get; set; }
        public required string Funcao { get; set; }
        public required string Descricao { get; set; }
        public required string Password { get; set; }
        public required string Token { get; set; }
    }
}