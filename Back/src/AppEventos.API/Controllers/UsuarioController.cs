using AppEventos.API.Extensions;
using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        /// <summary>
        /// Busca usuário
        /// </summary>
        /// <returns></returns>
        [HttpGet("Buscar")]
        public async Task<IActionResult> BuscarUsuario()
        {
            try
            {
                var nomeUsuario = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(nomeUsuario);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "recuperar")}: {ex.Message}");
            }
        }

        /// <summary>
        /// Efetua Login
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLogin.UserName);
                if (user == null) return Unauthorized("Usuário ou Senha inválidos");

                var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);
                if (!result.Succeeded)
                    return Unauthorized("Usuário ou Senha inválidos");

                return Ok(new
                {
                    username = user.UserName,
                    Nome = user.Nome,
                    token = _tokenService.CreateToken(user).Result,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "realizar login")}: {ex.Message}");
            }
        }

        /// <summary>
        /// Registra usuário
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost("Registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar(UserDto usuario)
        {
            try
            {
                if (await _accountService.UserExists(usuario.Username.ToLower()))
                    return BadRequest("Usuário já existe");

                if (await _accountService.CreateAccountAsync(usuario) != null)
                    return Ok(usuario);

                return BadRequest("Usuário não criado, tente novamente mais tarde!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "registrar")}: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualizar Usuário
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPut("Atualizar")]
        public async Task<IActionResult> AtualizarUsuario(UserUpdateDto usuario)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário inválido");


                var userReturn = await _accountService.UpdateAccount(usuario);
                if (userReturn == null) return NoContent();

                return Ok(userReturn);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "atualizar")}: {ex.Message}");
            }
        }
    }
}
