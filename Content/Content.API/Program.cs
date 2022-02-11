using Content.API.Extensions;
using Content.API.Middlewares;
using Hangfire;
using Serilog;
using VOL.API.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHangfire(builder.Configuration);
builder.Services.AddJWTAuthentication(builder.Configuration);
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddServices();
// builder.Services.AddHttpClients();
builder.Services.AddServiceFilters();
builder.Services.AddRepositories();
builder.Services.AddMassTransit(builder.Configuration);
builder.Services.AddUOW();
builder.Services.AddAutoMapper();
builder.Services.AddEvents();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureApiOptions();

var app = builder.Build();
app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHangfireDashboard("/hangfire",
    new DashboardOptions {Authorization = new[] {new HangfireAuthorizationFilter()}});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Services.ApplyPendingMigrations();
app.Run();