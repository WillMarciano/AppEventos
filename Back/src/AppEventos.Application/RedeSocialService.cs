using AppEventos.Application.Dtos;
using AppEventos.Application.Interfaces;
using AppEventos.Domain.Models;
using AppEventos.Repository.Interfaces;
using AutoMapper;

namespace AppEventos.Application
{
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialRepository _redeSocialRepository;
        private readonly IMapper _mapper;

        public RedeSocialService(IRedeSocialRepository redeSocialPersist, IMapper mapper)
        {
            _redeSocialRepository = redeSocialPersist;
            _mapper = mapper;
        }

        private async Task AddRedeSocial(int Id, RedeSocialDto model, bool isEvento)
        {
            try
            {
                var RedeSocial = _mapper.Map<RedeSocial>(model);
                if (isEvento)
                {
                    RedeSocial.EventoId = Id;
                    RedeSocial.PalestranteId = null;
                }
                else
                {
                    RedeSocial.EventoId = null;
                    RedeSocial.PalestranteId = Id;
                }

                _redeSocialRepository.Add<RedeSocial>(RedeSocial);
                await _redeSocialRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]?> GetAllRedesSociaisAsync(int id, bool isEvento)
        {
            try
            {
                var redeSocials = isEvento
                    ? await _redeSocialRepository.GetAllByEventoIdAsync(id)
                    : await _redeSocialRepository.GetAllByPalestranteIdAsync(id);

                if (redeSocials == null) return null;

                var resultado = _mapper.Map<RedeSocialDto[]>(redeSocials);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto?> GetRedeSocialByIdAsync(int id, int redeSocialId, bool isEvento)
        {
            try
            {
                var redeSocial = isEvento
                    ? await _redeSocialRepository.GetRedeSocialEventoByIdAsync(id, redeSocialId)
                    : await _redeSocialRepository.GetRedeSocialPalestranteByIdAsync(id, redeSocialId);

                if (redeSocial == null) return null;

                var resultado = _mapper.Map<RedeSocialDto?>(redeSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id, int redeSocialId, bool isEvento)
        {
            try
            {
                var redeSocial = isEvento
                    ? await _redeSocialRepository.GetRedeSocialEventoByIdAsync(id, redeSocialId)
                    : await _redeSocialRepository.GetRedeSocialPalestranteByIdAsync(id, redeSocialId);


                if (redeSocial == null) throw new Exception("Rede Social não encontrada.");

                _redeSocialRepository.Delete<RedeSocial>(redeSocial);
                return await _redeSocialRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]?> SaveAsync(int id, RedeSocialDto[] models, bool isEvento)
        {
            try
            {
                var redes = isEvento
                    ? await _redeSocialRepository.GetAllByEventoIdAsync(id)
                    : await _redeSocialRepository.GetAllByPalestranteIdAsync(id);

                if (redes == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                        await AddRedeSocial(id, model, isEvento);

                    else
                    {
                        var RedeSocial = redes.FirstOrDefault(RedeSocial => RedeSocial.Id == model.Id);
                        model.EventoId = id;

                        _mapper.Map(model, RedeSocial);
                        _redeSocialRepository.Update<RedeSocial>(RedeSocial!);
                        await _redeSocialRepository.SaveChangesAsync();
                    }
                }

                var redeSocialRetorno = isEvento
                    ? await _redeSocialRepository.GetAllByEventoIdAsync(id)
                    : await _redeSocialRepository.GetAllByPalestranteIdAsync(id);

                return _mapper.Map<RedeSocialDto[]>(redeSocialRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}