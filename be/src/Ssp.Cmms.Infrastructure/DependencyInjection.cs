using System.Text;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Infrastructure.Auth;
using Ssp.Cmms.Infrastructure.Persistence;
using Ssp.Cmms.Infrastructure.Realtime;

namespace Ssp.Cmms.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IRealtimeNotifier, SignalRNotifier>();
        services.AddSingleton<AuthCookieFactory>();
        services.AddScoped<IAuthService, AuthService>();

        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));
        services.Configure<AuthCookieSettings>(
            configuration.GetSection(AuthCookieSettings.SectionName));

        var jwt = configuration.GetSection(JwtSettings.SectionName)
            .Get<JwtSettings>() ?? new JwtSettings();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.SigningKey))
                };

                // Read the JWT from the HttpOnly access_token cookie. SignalR
                // may still pass it via the access_token query string during
                // the WebSocket handshake.
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var path = context.HttpContext.Request.Path;
                        var queryToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(queryToken) &&
                            path.StartsWithSegments(WorkOrderHub.Path))
                        {
                            context.Token = queryToken;
                        }
                        else
                        {
                            var cookieToken = context.Request.Cookies[
                                AuthCookieSettings.AccessTokenName];
                            if (!string.IsNullOrEmpty(cookieToken))
                            {
                                context.Token = cookieToken;
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdmin", p => p.RequireRole("Admin"))
            .AddPolicy("RequireManagerOrAdmin",
                p => p.RequireRole("Admin", "Manager"))
            .AddPolicy("RequireTechnicianOrAbove",
                p => p.RequireRole("Admin", "Manager", "Technician"));

        services.AddSignalR()
            .AddJsonProtocol(options =>
                options.PayloadSerializerOptions.Converters.Add(
                    new System.Text.Json.Serialization.JsonStringEnumConverter()));

        services.AddHangfire(config =>
            config.UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(connectionString)));
        services.AddHangfireServer();
        services.AddScoped<Jobs.GeneratePreventiveWorkOrdersJob>();

        return services;
    }
}
