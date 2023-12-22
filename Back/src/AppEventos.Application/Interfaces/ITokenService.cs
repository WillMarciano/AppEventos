using AppEventos.Application.Dtos;

namespace AppEventos.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserUpdateDto userUpdateDto);
    }
}
