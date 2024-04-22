using System.Reflection;
using System.Text.Json;
using FluentValidation;
using Microsoft.OpenApi.Models;
using RainfallApi;
using RainfallApi.Client;
using RainfallApi.Controllers.Rainfall;
using Refit;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var kestrelConfig = configuration.GetSection("Kestrel:Endpoints:Https:Url").Get<string>()!;
var uri = new Uri(kestrelConfig);

// Add services to the container.
builder.Services
   .AddRefitClient<IRainfallApi>()
   .ConfigureHttpClient(client => client.BaseAddress = new Uri(configuration["ApiUrls:RainfallApi"]!));

builder.Services.AddSingleton<IRainfallService, RainfallService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0",
        Title = "Rainfall Api",
        Description = "An API which provides rainfall reading data",
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/license/mit")
        },
        Contact = new OpenApiContact
        {
            Name = "Sorted",
            Url = new Uri("https://www.sorted.com")
        }
    });

    options.AddServer(new OpenApiServer
    {
        Url = uri.ToString(),
        Description = "Rainfall Api"
    });

    options.CustomSchemaIds(x => JsonNamingPolicy.CamelCase.ConvertName(x.Name));

    options.EnableAnnotations();

    var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xml));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<EnforcePortMiddleware>(uri.Port);

app.Run();

/// <summary>
///     The entry point for the application.
/// </summary>
public partial class Program { }