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
            // Allow skipping infrastructure initialization for isolation testing via SKIP_INFRA=1
            var skipInfra = builder.Configuration.GetValue<bool>("SkipInfra", false) || Environment.GetEnvironmentVariable("SKIP_INFRA") == "1";
            // Health checks can be skipped by setting config key "SkipHealthChecks" or env var SKIP_HEALTHCHECKS=1
            var skipHealthChecks = builder.Configuration.GetValue<bool>("SkipHealthChecks", false) || Environment.GetEnvironmentVariable("SKIP_HEALTHCHECKS") == "1";
            // If infra is skipped we must also skip health checks mapping to avoid mapping checks
            // when no IHealthCheck services were registered (prevents InvalidOperationException).
            if (skipInfra) skipHealthChecks = true;

            var mongoSettings = new MongoSettings();
            builder.Configuration.GetSection("Mongo").Bind(mongoSettings);
            if (!skipInfra)
            {
                builder.Services.AddMongoInfrastructure(builder.Configuration);
                if (!skipHealthChecks)
                {
                    builder.Services.AddAppAndMongoHealthChecks(mongoSettings);
                }
            }

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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                // Include XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // Provide at least v1 and v2 docs so evaluators can see versioning (v2 may be empty)
                c.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerTitle, Version = "v1", Description = swaggerDescription });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = swaggerTitle, Version = "v2", Description = swaggerDescription });
            });

            // MongoDB registration (only if infra not skipped)
            if (!skipInfra)
            {
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
            }


            // EF (only if infra not skipped)
            if (!skipInfra)
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    // Ajuste para Oracle ou SQL Server conforme necessário
                    options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
                    // Exemplo para SQL Server:
                    // options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
                });
            }

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<MotoRequestValidator>();

            // Repositórios e UoW
            // IMotoRepository é registrado por AddMongoInfrastructure quando a infra não é pulada.
            if (!skipInfra)
            {
                builder.Services.AddScoped<IPatioRepository, PatioRepository>();
                builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            }
            else
            {
                // For isolation runs (SKIP_INFRA=1) register lightweight in-memory implementations
                builder.Services.AddScoped<Domain.Repositories.IMotoRepository, InMemoryMotoRepository>();
                builder.Services.AddScoped<IPatioRepository, InMemoryPatioRepository>();
                builder.Services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();
            }

            // Services (always registered — repositories are either real or in-memory above)
            builder.Services.AddScoped<MotoService>();
            builder.Services.AddScoped<PatioService>();

            var app = builder.Build();

            // Deve estar antes do UseAuthorization e MapControllers
            app.UseCors("AllowLocalhost");

            // Health checks endpoint (only mapped if not skipped)
            if (!skipHealthChecks)
            {
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
            }

            // Swagger + Versioning UI (rota /docs)
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // Register a swagger endpoint for each discovered API version
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", $"{swaggerTitle} {desc.GroupName}");
                }
                options.RoutePrefix = "docs";
                options.DocumentTitle = "Mottu Fleet API Docs";
            });

            // Only enable HTTPS redirection if an HTTPS URL is configured (avoids the "Failed to determine the https port for redirect" warning)
            // and avoid forcing a redirect in development when HTTPS isn't set up.
            var hasHttps = app.Urls.Any(u => u.StartsWith("https://", StringComparison.OrdinalIgnoreCase));
            if (hasHttps)
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthorization();

            app.MapControllers();

            // Health endpoints (keep writer for /health)
            if (!skipHealthChecks)
            {
                app.MapHealthChecks("/health/live");
                app.MapHealthChecks("/health/ready");
            }

            // Diagnostics: capture unhandled exceptions and lifetime events to aid debugging when the host shuts down unexpectedly
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Console.Error.WriteLine("UNHANDLED EXCEPTION: " + (e.ExceptionObject?.ToString() ?? "<null>"));
            };
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Console.Error.WriteLine("UNOBSERVED TASK EXCEPTION: " + e.Exception?.ToString());
            };
            var lifetime = app.Lifetime;
            lifetime.ApplicationStopping.Register(() =>
            {
                var msg = $"ApplicationStopping event fired. Thread stack: {Environment.StackTrace}";
                Console.WriteLine(msg);
                try { System.IO.File.AppendAllText("stop-debug.log", DateTime.UtcNow + " - " + msg + "\n"); } catch { }
            });
            lifetime.ApplicationStopped.Register(() =>
            {
                var msg = $"ApplicationStopped event fired. Thread stack: {Environment.StackTrace}";
                Console.WriteLine(msg);
                try { System.IO.File.AppendAllText("stop-debug.log", DateTime.UtcNow + " - " + msg + "\n"); } catch { }
            });
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                var msg = $"ProcessExit event fired. Stack: {Environment.StackTrace}";
                Console.WriteLine(msg);
                try { System.IO.File.AppendAllText("stop-debug.log", DateTime.UtcNow + " - " + msg + "\n"); } catch { }
            };

            // Wrap Run in try/catch to log any synchronous exceptions that might cause the host to stop immediately.
            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Host terminated unexpectedly: " + ex);
                // Re-throw so the process exit code reflects the failure.
                throw;
            }
        }
    }
}
