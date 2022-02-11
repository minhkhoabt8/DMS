using System.Reflection;
using System.Text;
using MassTransit;
using Metadata.API.Filters;
using Metadata.Core.Events.Integration;
using Metadata.Infrastructure.Data;
using Metadata.Infrastructure.EventHandlers.Common;
using Metadata.Infrastructure.EventHandlers.Integration;
using Metadata.Infrastructure.Mapper;
using Metadata.Infrastructure.MessageConsumers;
using Metadata.Infrastructure.Repositories.Implementations;
using Metadata.Infrastructure.Repositories.Interfaces;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Metadata.API.Extensions;

public static class ServiceExtensions
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<MetadataContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("MetadataConnection"),
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

    public static void ApplyPendingMigrations(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<MetadataContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IFolderService, FolderService>();
        services.AddScoped<IFileService, FileService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IFolderRepository, FolderRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFileVersionRepository, FileVersionRepository>();
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
            sbc.AddConsumer<FileVersionCreatedConsumer>();
            sbc.AddConsumer<FileVersionReadyConsumer>();
            sbc.AddConsumer<FileVersionDeactivatedConsumer>();

            sbc.UsingRabbitMq((ctx, rmq) =>
            {
                rmq.Host(conf["RabbitMq:host"], conf["RabbitMq:vhost"], configurator =>
                {
                    configurator.Username(conf["RabbitMq:username"]);
                    configurator.Password(conf["RabbitMq:password"]);
                });

                rmq.ReceiveEndpoint("metadata-account-created", e => e.ConfigureConsumer<AccountCreatedConsumer>(ctx));
                rmq.ReceiveEndpoint("metadata-file-version-created",
                    e => e.ConfigureConsumer<FileVersionCreatedConsumer>(ctx));
                rmq.ReceiveEndpoint("metadata-file-version-ready",
                    e => e.ConfigureConsumer<FileVersionReadyConsumer>(ctx));
                rmq.ReceiveEndpoint("metadata-file-version-deactivated",
                    e => e.ConfigureConsumer<FileVersionDeactivatedConsumer>(ctx));
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
        services.AddScoped<IEventHandler<FilePostCreatedEvent>, FilePostCreatedEventHandler>();
        services.AddScoped<IEventHandler<FolderPostCreatedEvent>, FolderPostCreatedEventHandler>();
    }

    public static void ConfigureApiOptions(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
    }
}