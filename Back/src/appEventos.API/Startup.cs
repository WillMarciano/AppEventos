using AppEventos.API.Configuration;

namespace AppEventos.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiConfiguration(Configuration);
            services.AddIdentityConfiguration(Configuration);
            services.AddJwtConfiguration(Configuration);
            services.AddSwaggerConfiguration();
            services.RegisterServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerConfiguration(env);
            app.UseAuthConfiguration();
            app.UseApiConfiguration(env);
        }
    }
}