using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using AppEventos.Domain.Identity;
using AppEventos.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AccountService(IUserRepository userRepository,
                              UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefaultAsync(x => x.UserName == userUpdateDto.UserName.ToLower()).Result ?? null;
                if (user != null)
                    return await _signInManager.CheckPasswordSignInAsync(user, password, false);
                throw new InvalidOperationException("Erro ao Consultar usuário");
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar password. Erro:{ex.Message}");
            }
        }

        public async Task<UserUpdateDto?> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                if (user.UserName != null)
                {
                    user.UserName = user.UserName.ToLower();

                    var result = await _userManager.CreateAsync(user, userDto.Password);
                    if (result.Succeeded)
                        return _mapper.Map<UserUpdateDto?>(user);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar criar conta. Erro:{ex.Message}");
            }
        }

        public async Task<UserUpdateDto?> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userRepository.GetUserByUserNameAsync(userName.ToLower());
                if (user == null) return null;
                return _mapper.Map<UserUpdateDto?>(user!);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar retornar usuário por Username. Erro:{ex.Message}");
            }
        }

        public async Task<UserUpdateDto?> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userRepository.GetUserByUserNameAsync(userUpdateDto.UserName);
                if (user == null) return null;

                _mapper.Map(userUpdateDto, user);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);

                _userRepository.Update(user);
                if (await _userRepository.SaveChangesAsync())
                {
                    var uResponse = await _userRepository.GetUserByUserNameAsync(user.UserName!);
                    return _mapper.Map<UserUpdateDto>(uResponse);
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar atualizar usuário. Erro:{ex.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(x => x.UserName == userName.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar usuário. Erro:{ex.Message}");
            }
        }
    }
}
