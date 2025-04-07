using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

using Models;

// using Middlewares;
using Services;
using System.Net;

namespace roulette
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigurationServices(IServiceCollection services)
        {
            ConfigureCors(services);
            ConfigureDatabase(services);
            ConfigureAuthentication(services);
            ConfigureScopedServices(services);
            services
                .AddControllers()
                .AddJsonOptions(
                    x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
                );

            // services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Roullette API",
                        Version = "v1"
                    }
                );

                options.AddSecurityDefinition(
                    "Bearer",
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description =
                            "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjE2IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IlZpY3RvciBDb2JvIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMyIsImV4cCI6MTc2ODg0NzkyMn0.R7Fwe22IFTJtjfA0p9fkWcqfyAK5i2RnCLfdqcK8f99v0b_LjHyufAMvUbIrU4JrxH1UEL9aSH0gMf4ieAMhuA",
                    }
                );

                options.AddSecurityRequirement(
                    new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        {
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                            {
                                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                                {
                                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                },
                            },
                            Array.Empty<string>()
                        },
                    }
                );
            });
            services.AddHttpContextAccessor();

            services.AddSignalR();
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 50 * 1024 * 1024;
            }); // 50 MB
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
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Models")
                );
            });

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var token = Configuration.GetSection("AppSettings:Token").Value;
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token), "El token no puede ser null");
            }

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(token)
                        ),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
        }

        private static void ConfigureScopedServices(IServiceCollection services)
        {
            ConfigureGlobalServices(services);
        }

        private static void ConfigureGlobalServices(IServiceCollection services) { }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddlewares>();
            app.Use(
                async (context, next) =>
                {
                    await next();
                }
            );
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseMiddleware<AuditMiddleware>();
            app.UseCors("AllowAllOrigins");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(
                async (context) =>
                {
                    if (context.Request.Method == "GET" && context.Request.Path == "/")
                    {
                        await context.Response.WriteAsync("Api funcionando");
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        await context.Response.WriteAsync("Ruta no encontrada");
                    }
                }
            );
        }
    }
}
