﻿using AppEventos.Repository.Interfaces;
using AppEventos.Repository;
using AppEventos.Application.Interfaces;
using AppEventos.Application;
using AppEventos.API.Helpers;

namespace AppEventos.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IEventoService, EventoService>();
            services.AddScoped<ILoteService, LoteService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPalestranteService, PalestranteService>();
            services.AddScoped<IRedeSocialService, RedeSocialService>();
            services.AddScoped<IUtil, Util>();

            services.AddScoped<IGeralRepository, GeralRepository>();
            services.AddScoped<IEventoRepository, EventoRepository>();
            services.AddScoped<ILoteRepository, LoteRepository>();
            services.AddScoped<IPalestranteRepository, PalestranteRepository>();
            services.AddScoped<IRedeSocialRepository, RedeSocialRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
