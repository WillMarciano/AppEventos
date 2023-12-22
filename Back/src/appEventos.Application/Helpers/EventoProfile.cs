using AppEventos.Application.Dtos;
using AppEventos.Domain.Identity;
using AppEventos.Domain.Models;
using AutoMapper;

namespace AppEventos.Application.Helpers
{
    public class EventoProfile : Profile
    {
        public EventoProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Lote, LoteDto>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<Palestrante, PalestranteDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
