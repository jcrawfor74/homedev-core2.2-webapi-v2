using Autofac;
using HomeDev.Core.WebApi.Configuration;
using HomeDev.Core.WebApi.Interfaces;
using HomeDev.Core.WebApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HomeDev.Core.WebApi
{
    public static class DependencyRegistrar
    {

        public static void RegisterComponents(IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, TokenAuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            // swagger setup
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SetupSwaggerOptions>();
        }

        public static void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterType<TokenAuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<UserManagementService>().As<IUserManagementService>();
            builder.RegisterType<SetupSwaggerOptions>().As<IConfigureOptions<SwaggerGenOptions>>();
            builder.RegisterType<ApiSettings>().As<IApiSettings>();

            // Serilog Logging
            builder.RegisterInstance(Log.Logger);
        }
    }
}