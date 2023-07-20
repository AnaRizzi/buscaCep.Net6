using BuscaCep;
using BuscaCep.Domain.Interfaces;
using BuscaCep.Domain.Models;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configurar Refit para acessar ViaCep:
builder.Services.AddRefitClient<IViaCepRefit>().ConfigureHttpClient(c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ViaCep"));
});

//configurar o Mediatr:
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CepEntrada).Assembly));

//Configurar o Redis para cache:
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = "Cep_Cache";
    options.Configuration = "rediscep";
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

//chama a classe de Endpoints:
app.MapAppEndpoints();

app.Run();