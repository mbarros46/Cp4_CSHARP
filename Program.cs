using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Application.Mappings;
using Application.Services;
using Application.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.EF;
using Infrastructure.Repositories;
using Domain.Repositories;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Infrastructure.Mongo;

namespace MottuCrudAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // ===== CP5: Mongo Infrastructure + HealthChecks =====
            builder.Services.AddMongoInfrastructure(builder.Configuration);
            var mongoSettings = new MongoSettings();
            builder.Configuration.GetSection("Mongo").Bind(mongoSettings);
            builder.Services.AddAppAndMongoHealthChecks(mongoSettings);

            // ===== CP5: API Versioning =====
            builder.Services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
                setup.ApiVersionReader = new UrlSegmentApiVersionReader(); // /api/v{version}/...
            });
            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5049", "https://localhost:7208")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // Read Swagger metadata from configuration
            var swaggerConfig = builder.Configuration.GetSection("Swagger");
            var swaggerTitle = swaggerConfig.GetValue<string>("Title") ?? "Mottu API";
            var swaggerDescription = swaggerConfig.GetValue<string>("Description") ?? "API para gerenciamento de motos e pátios da Mottu";
            var swaggerVersion = swaggerConfig.GetValue<string>("Version") ?? "v1";

            // API Versioning + ApiExplorer
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerVersion, new OpenApiInfo
                {
                    Title = swaggerTitle,
                    Version = swaggerVersion,
                    Description = swaggerDescription,
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // MongoDB registration (Mongo infrastructure registers health checks)
            var mongoSection = builder.Configuration.GetSection("Mongo");
            var mongoConn = mongoSection.GetValue<string>("ConnectionString");
            var mongoDbName = mongoSection.GetValue<string>("Database");
            if (!string.IsNullOrEmpty(mongoConn))
            {
                var mongoClient = new MongoClient(mongoConn);
                builder.Services.AddSingleton<IMongoClient>(mongoClient);
                if (!string.IsNullOrEmpty(mongoDbName))
                {
                    builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbName));
                }
            }


            // EF
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Ajuste para Oracle ou SQL Server conforme necessário
                options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
                // Exemplo para SQL Server:
                // options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<MotoRequestValidator>();

            // Repositórios e UoW
            // IMotoRepository agora é registrado por AddMongoInfrastructure (MongoMotoRepository)
            builder.Services.AddScoped<IPatioRepository, PatioRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            builder.Services.AddScoped<MotoService>();
            builder.Services.AddScoped<PatioService>();

            var app = builder.Build();

            // Deve estar antes do UseAuthorization e MapControllers
            app.UseCors("AllowLocalhost");

            // Health checks endpoint
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                    {
                        status = report.Status.ToString(),
                        results = report.Entries.Select(e => new { key = e.Key, status = e.Value.Status.ToString(), description = e.Value.Description })
                    }));
                }
            });

            // Swagger + Versioning UI (rota /docs)
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mottu Fleet API v1");
                options.RoutePrefix = "docs";
                options.DocumentTitle = "Mottu Fleet API Docs";
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Health endpoints
            app.MapHealthChecks("/health");
            app.MapHealthChecks("/health/live");
            app.MapHealthChecks("/health/ready");

            app.Run();
        }
    }
}
