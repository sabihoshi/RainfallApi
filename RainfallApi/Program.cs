using System.Reflection;
using FluentValidation;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(RainfallApi.Client.RainfallApi.Create());
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version     = "1.0",
        Title       = "Rainfall Api",
        Description = "An API which provides rainfall reading data",
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url  = new Uri("https://opensource.org/license/mit")
        },
        Contact = new OpenApiContact
        {
            Name = "Sorted",
            Url  = new Uri("https://www.sorted.com")
        }
    });

    options.AddServer(new OpenApiServer
    {
        Url         = "https://localhost:3000",
        Description = "Rainfall Api"
    });

    options.CustomSchemaIds(x => $"{char.ToLower(x.Name[0])}{x.Name[1..]}");

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

app.Run();