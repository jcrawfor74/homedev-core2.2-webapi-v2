using System;
using System.IO;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HomeDev.Core.WebApi.Configuration;
using HomeDev.Core.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace HomeDev.Core.WebApi
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var jwtTokenConfig = SetupAppSettings(services);
            SetupAuthentication(services, jwtTokenConfig);
            SetupApiVersioning(services);
            SetupSwagger(services);
            SetupLoggers(services);

            return SetupDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader())
                .UseAuthentication()
                .UseHttpsRedirection()
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(
                    options =>
                    {
                        // build a swagger endpoint for each discovered API version
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        }
                    });

        }

        private TokenSettings SetupAppSettings(IServiceCollection services)
        {
            services.Configure<ApiSettings>(Configuration.GetSection("apiSettings"));
            services.Configure<TokenSettings>(Configuration.GetSection("tokenSettings"));
            var tokenSettings = Configuration.GetSection("tokenSettings").Get<TokenSettings>();
            var secret = Encoding.ASCII.GetBytes(tokenSettings.Secret);
            return tokenSettings;
        }

        private void SetupAuthentication(IServiceCollection services, TokenSettings tokenSettings)
        {
            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(j =>
            {
                j.RequireHttpsMetadata = false;
                j.SaveToken = true;
                j.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings.Secret)),
                    ValidIssuer = tokenSettings.Issuer,
                    ValidAudience = tokenSettings.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        }

        private void SetupApiVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        private void SetupSwagger(IServiceCollection services)
        {

            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SetupSwaggerDefaults>();

                    // integrate xml comments
                    options.IncludeXmlComments(XmlCommentsFilePath);

                    options.AddSecurityDefinition("oauth2", new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Enter: \"Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });

                    options.OperationFilter<SecurityRequirementsOperationFilter>();

                });
            
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        private IServiceProvider SetupDependencies(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container.

            builder.Populate(services);
            DependencyRegistrar.RegisterComponents(builder);

            this.ApplicationContainer = builder.Build();

            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        private void SetupLoggers(IServiceCollection services)
        {

            services.Configure<ApiSettings>(Configuration.GetSection("apiSettings"));
            var apiSettings = Configuration.GetSection("apiSettings").Get<ApiSettings>();

            LoggersConfiguration.SetupLoggers(apiSettings);

            // var loggers = new  List<ISerilogService>();
            // var loggerConfigurationService = new SerilogConfigurationService();
            // return loggerConfigurationService.Initialise(loggers);
        }
    }
}