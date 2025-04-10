using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using Services;

namespace Roulette
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigurationServices(IServiceCollection services)
        {
            ConfigureCors(services);
            ConfigureDatabase(services);
            ConfigureScopedServices(services);

            services
                .AddControllers()
                .AddJsonOptions(
                    x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
                );

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Roulette API",
                        Version = "v1"
                    }
                );
            });

            services.AddHttpContextAccessor();
            services.AddSignalR();

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 50 * 1024 * 1024;
            });
        }

        private static void ConfigureScopedServices(IServiceCollection services)
        {
            ConfigureGlobalServices(services);
        }

        private static void ConfigureGlobalServices(IServiceCollection services)
        {
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IResultServices, ResultServices>();
            services.AddScoped<IRouletteNumberServices, RouletteNumberServices>();
        }

        private static void ConfigureCors(IServiceCollection services)
        {
            string serverIp = GetIPLocal();
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .SetIsOriginAllowed(origin => true);
                    }
                );
            });
        }

        private static string GetIPLocal()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No se pudo encontrar la dirección IP local.");
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddlewares>();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAllOrigins");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async context =>
            {
                if (context.Request.Method == "GET" && context.Request.Path == "/")
                {
                    await context.Response.WriteAsync("Roulette API Working...");
                }
                else
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("Route not found");
                }
            });
        }
    }
}
