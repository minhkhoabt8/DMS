using System.Reflection;
using System.Text;
using Auth.API.Filters;
using Auth.Core.Events.Integration;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.EventHandlers.Common;
using Auth.Infrastructure.EventHandlers.Integration;
using Auth.Infrastructure.Mapper;
using Auth.Infrastructure.MessageConsumers;
using Auth.Infrastructure.Repositories.Implementations;
using Auth.Infrastructure.Repositories.Interfaces;
using Auth.Infrastructure.Services.Implementations;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;

namespace Auth.API.Extensions;

public static class ServiceExtensions
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<AuthContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("AuthConnection"),
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
        });
    }

    public static void AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<ISystemManagementService, SystemManagementService>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(
                HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            );
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DMS",
                Version = "v1.0.0",
                Description = "Document management system",
            });
            c.UseInlineDefinitionsForEnums();
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Add DMS Bearer Token Here",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer"
                    },
                    new List<string>()
                }
            });
        });
    }

    public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                RequireAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                IssuerSigningKey = new
                    SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes
                        (configuration["JWT:Secret"]))
            };
        });
    }

    public static void ApplyPendingMigrations(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AuthContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFolderRepository, FolderRepository>();
    }

    public static void AddUOW(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddServiceFilters(this IServiceCollection services)
    {
        services.AddScoped<AutoValidateModelState>();
    }

    public static void AddMassTransit(this IServiceCollection services, IConfiguration conf)
    {
        services.AddMassTransit(sbc =>
        {
            sbc.AddConsumer<FileCreatedConsumer>();
            sbc.AddConsumer<FolderCreatedConsumer>();

            sbc.UsingRabbitMq((ctx, rmq) =>
            {
                rmq.Host(conf["RabbitMq:host"], conf["RabbitMq:vhost"], configurator =>
                {
                    configurator.Username(conf["RabbitMq:username"]);
                    configurator.Password(conf["RabbitMq:password"]);
                });

                rmq.ReceiveEndpoint("auth-file-created", e => e.ConfigureConsumer<FileCreatedConsumer>(ctx));
                rmq.ReceiveEndpoint("auth-folder-created", e => e.ConfigureConsumer<FolderCreatedConsumer>(ctx));
            });
        });

        services.AddMassTransitHostedService();
    }

    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
    }

    public static void AddEvents(this IServiceCollection services)
    {
        services.AddScoped<IEventDispatcher, EventDispatcher>();
        services.AddScoped<IEventHandler<AccountPostCreatedEvent>, AccountPostCreatedEventHandler>();
    }

    public static void ConfigureApiOptions(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
    }
}