using appEventos.Application.Dtos;
using appEventos.Domain.Models;
using AutoMapper;

namespace appEventos.Application.Helpers
{
    public class EventoProfile : Profile
    {
        public EventoProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
        }
    }
}
