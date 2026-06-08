using Hangfire;
using Microsoft.OpenApi.Models;
using Serilog;
using Ssp.Cmms.Api.Middleware;
using Ssp.Cmms.Application;
using Ssp.Cmms.Infrastructure;
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

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    options.AddSecurityDefinition("Bearer", jwtScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [jwtScheme] = Array.Empty<string>()
    });
});

var corsOrigins = GetCorsOrigins(builder.Configuration);

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(corsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMMS API v1"));
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<WorkOrderHub>(WorkOrderHub.Path);
app.MapHangfireDashboard("/hangfire");

// Schedule the recurring PM auto-generation job (daily at 06:00).
RecurringJob.AddOrUpdate<GeneratePreventiveWorkOrdersJob>(
    "generate-preventive-work-orders",
    job => job.RunAsync(CancellationToken.None),
    "0 6 * * *");

app.Run();

static string[] GetCorsOrigins(IConfiguration configuration)
{
    var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
    if (origins is { Length: > 0 })
        return origins;

    var raw = configuration["Cors:AllowedOrigins"];
    if (!string.IsNullOrWhiteSpace(raw))
    {
        return raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    return ["http://localhost:5173"];
}

public partial class Program;
