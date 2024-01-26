using AppEventos.API.Extensions;
using AppEventos.API.Helpers;
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
        private readonly IUtil _util;
        private readonly string _destino = "Images";

        public UsuarioController(IAccountService accountService, 
                                ITokenService tokenService, 
                                IUtil util)
        {
            _accountService = accountService;
            _tokenService = tokenService;
            _util = util;
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
                if (await _accountService.UserExists(usuario.UserName.ToLower()))
                    return BadRequest("Usuário já existe");

                var user = await _accountService.CreateAccountAsync(usuario);
                if (user != null)
                    return Ok(new
                    {
                        username = user.UserName,
                        Nome = user.Nome,
                        token = _tokenService.CreateToken(user).Result,
                    });

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
                if (usuario.UserName != User.GetUserName()) return Unauthorized("Usuário inválido");

                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário inválido");


                var userReturn = await _accountService.UpdateAccount(usuario);
                if (userReturn == null) return NoContent();

                return Ok(new
                {
                    username = userReturn.UserName,
                    Nome = userReturn.Nome,
                    token = _tokenService.CreateToken(userReturn).Result,
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "atualizar")}: {ex.Message}");
            }
        }

        /// <summary>
        /// Salva Imagem do Usuario
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0 && user.ImagemUrl != null)
                {
                    _util.DeleteImage(user.ImagemUrl, _destino);
                    user.ImagemUrl = await _util.SaveImage(file, _destino);
                }
                var userReturn = await _accountService.UpdateAccount(user);
                return userReturn == null ? NoContent() : Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{errorResponse.Replace("*", "Upload de foto do")} {ex.Message}");
            }
        }
    }
}
