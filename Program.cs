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

namespace MottuCrudAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Mottu API",
                    Version = "v1",
                    Description = "API para gerenciamento de motos e pátios da Mottu",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


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
            builder.Services.AddScoped<IMotoRepository, MotoRepository>();
            builder.Services.AddScoped<IPatioRepository, PatioRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            builder.Services.AddScoped<MotoService>();
            builder.Services.AddScoped<PatioService>();

            var app = builder.Build();

            // Deve estar antes do UseAuthorization e MapControllers
            app.UseCors("AllowLocalhost");


            // Swagger sempre disponível
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
