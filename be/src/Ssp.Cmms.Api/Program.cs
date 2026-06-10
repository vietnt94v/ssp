using Hangfire;
using Microsoft.OpenApi.Models;
using Serilog;
using Ssp.Cmms.Api.Middleware;
using Ssp.Cmms.Application;
using Ssp.Cmms.Infrastructure;
using Ssp.Cmms.Infrastructure.Auth;
using Ssp.Cmms.Infrastructure.Jobs;
using Ssp.Cmms.Infrastructure.Realtime;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console());

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CMMS API",
        Version = "v1",
        Description =
            "Computerized Maintenance Management System. " +
            "Full contract with examples: be/docs/openapi/cmms-api.yaml"
    });

    // Auth uses an HttpOnly access_token cookie set by /api/auth/login.
    // Swagger UI cannot set HttpOnly cookies, so this scheme is documentation
    // only; use the SPA flow for authenticated calls.
    var cookieScheme = new OpenApiSecurityScheme
    {
        Name = AuthCookieSettings.AccessTokenName,
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Cookie,
        Description =
            "JWT delivered as an HttpOnly cookie via POST /api/auth/login.",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "CookieAuth"
        }
    };
    options.AddSecurityDefinition("CookieAuth", cookieScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [cookieScheme] = Array.Empty<string>()
    });
});

var configuredOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?.Where(static origin => !string.IsNullOrWhiteSpace(origin))
    .Select(static origin => origin.Trim())
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray() ?? [];

var allowedOrigins = configuredOrigins.Length > 0
    ? configuredOrigins
    : ["http://localhost:5173"];

if (builder.Environment.IsProduction())
{
    const string localDevFrontend = "http://localhost:5173";
    if (!allowedOrigins.Contains(localDevFrontend, StringComparer.OrdinalIgnoreCase))
    {
        allowedOrigins = [..allowedOrigins, localDevFrontend];
    }
}

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()));

var app = builder.Build();

app.Logger.LogInformation(
    "CORS allowed origins: {Origins}",
    string.Join(", ", allowedOrigins));

app.UseCors();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMMS API v1"));
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/healthz", () => Results.Ok(new { status = "ok" }));
app.MapControllers();
app.MapHub<WorkOrderHub>(WorkOrderHub.Path);
app.MapHangfireDashboard("/hangfire");

// Schedule the recurring PM auto-generation job (daily at 06:00).
RecurringJob.AddOrUpdate<GeneratePreventiveWorkOrdersJob>(
    "generate-preventive-work-orders",
    job => job.RunAsync(CancellationToken.None),
    "0 6 * * *");

app.Run();

public partial class Program;
