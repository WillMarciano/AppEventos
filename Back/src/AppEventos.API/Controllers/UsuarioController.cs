using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private const string errorResponse = "Erro ao ao tentar * usuário";
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public UsuarioController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser/{nomeUsuario}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser(string nomeUsuario)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(nomeUsuario);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
            }
        }

        [HttpPost("Registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar(UserDto usuario)
        {
            try
            {
                if (await _accountService.UserExists(usuario.Username.ToLower()))
                    return BadRequest("Usuário já existe");

                if (await _accountService.CreateAccountAsync(usuario) != null)
                    return Ok("Usuário Cadastrado");

                return BadRequest("Usuário não criado, tente novamente mais tarde!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "registrar")}: {ex.Message}");
            }
        }
    }
}
