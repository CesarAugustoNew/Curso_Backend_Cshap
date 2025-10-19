using System.ComponentModel.DataAnnotations;
using System.Text;
using CadastroProdutos.Database;
using CadastroProdutos.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CadastroProdutos.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Description = @"Insira o JWT no campo abaixo usando o seguinte formato: Bearer {seu_token}.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    x.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "ouath2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header
            },

            new List<string>()
        }
    });
});
//builder.Services.AddScoped<IProdutosServices, ProdutosService>();
builder.Services.AddScoped<IProdutosServices, ProdutosDatabaseService>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source = Produtos.db"));

var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtConfig["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
 
var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");



var produtos = new List<Produto>()
{
    new Produto () { Id = 1, Nome = "Mouse sem fio", Preco = 99.90M, Estoque = 50},
    new Produto () { Id = 2, Nome = "Teclado", Preco = 249.90M, Estoque = 30}
};

app.MapGet("/produtos", () =>
{
    return produtos;
});

app.MapGet("/produtos/{id}", (int id) =>
{
    var produto = produtos.FirstOrDefault(x => x.Id == id);

    return produtos is not null
        ? Results.Ok(produto)
        : Results.NotFound($"Produto com ID {id} não encontrado.");
});

app.MapPost("/produtos", (Produto novoproduto) =>
{
    produtos.Add(novoproduto);
});

app.MapPut("/produtos/{id}", (int id, Produto produtoAtualizado) =>
{
    var produto = produtos.FirstOrDefault(x => x.Id == id);

    if (produto is null)
    {
        return Results.NotFound($"Produto com ID {id} não encontrado.");
    }

    produto.Nome = produtoAtualizado.Nome;
    produto.Preco = produtoAtualizado.Preco;
    produto.Estoque = produtoAtualizado.Estoque;

    return Results.Ok(produto);
});

app.MapDelete("/produtos/{id}", (int id) =>
{
    var produto = produtos.FirstOrDefault(x => x.Id == id);

    if (produto is null)
    {
        return Results.NotFound($"Produto com ID {id} não encontrado.");
    }

    produtos.Remove(produto);

    return Results.NoContent();
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
