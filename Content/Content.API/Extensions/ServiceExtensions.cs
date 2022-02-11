using System.Reflection;
using System.Text;
using Content.API.Filters;
using Content.Core.Events.Integration;
using Content.Infrastructure.Data;
using Content.Infrastructure.EventHandlers.Common;
using Content.Infrastructure.EventHandlers.Integration;
using Content.Infrastructure.Mapper;
using Content.Infrastructure.MessageConsumers;
using Content.Infrastructure.Repositories.Implementations;
using Content.Infrastructure.Repositories.Interfaces;
using Content.Infrastructure.Services.Implementations;
using Content.Infrastructure.Services.Interfaces;
using Content.Infrastructure.UOW;
using Hangfire;
using Hangfire.MemoryStorage;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Content.API.Extensions;

public static class ServiceExtensions
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<ContentContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("ContentConnection"),
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)
            );
        });
    }

    public static void AddHttpClients(this IServiceCollection services)
    {
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // For recognizing iformfiles in multiple-params controller actions
            c.SchemaGeneratorOptions.CustomTypeMappings.Add(typeof(IFormFile),
                () => new OpenApiSchema() {Type = "file", Format = "binary"});

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

    public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (env is "Development" or "Azure")
        {
            services.AddHangfire(opt => opt.UseMemoryStorage());
        }
        else
        {
            // services.AddHangfire(opt => opt.UsePostgreSqlStorage(
            //     configuration.GetConnectionString("HangfireConnection"),
            //     new PostgreSqlStorageOptions
            //     {
            //         JobExpirationCheckInterval = TimeSpan.FromMinutes(5),
            //         QueuePollInterval = TimeSpan.FromSeconds(5)
            //     }));

            // No db created for this project yet :)
            services.AddHangfire(opt => opt.UseMemoryStorage());
        }

        services.AddHangfireServer(opt => opt.WorkerCount = Environment.ProcessorCount);
    }

    public static void ApplyPendingMigrations(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ContentContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IBlobService, AzureBlobService>();
        services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFileVersionRepository, FileVersionRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
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
            sbc.AddConsumer<AccountCreatedConsumer>();
            sbc.AddConsumer<FileCreatedConsumer>();

            sbc.UsingRabbitMq((ctx, rmq) =>
            {
                rmq.Host(conf["RabbitMq:host"], conf["RabbitMq:vhost"], configurator =>
                {
                    configurator.Username(conf["RabbitMq:username"]);
                    configurator.Password(conf["RabbitMq:password"]);
                });

                rmq.ReceiveEndpoint("content-account-created", e => e.ConfigureConsumer<AccountCreatedConsumer>(ctx));
                rmq.ReceiveEndpoint("content-file-created", e => e.ConfigureConsumer<FileCreatedConsumer>(ctx));
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
        services.AddScoped<IEventHandler<FileVersionPostCreatedEvent>, FileVersionPostCreatedEventHandler>();
        services.AddScoped<IEventHandler<FileVersionPostReadyEvent>, FileVersionPostReadyEventHandler>();
        services.AddScoped<IEventHandler<FileVersionPostDeactivatedEvent>, FileVersionPostDeactivatedEventHandler>();
    }

    public static void ConfigureApiOptions(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
    }
}