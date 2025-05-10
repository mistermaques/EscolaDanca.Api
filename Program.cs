using EscolaDanca.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(); // habilita HttpClient via injeção


builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseCors("PermitirFrontend");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();



using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        if (!context.Usuarios.Any())
        {
            context.Usuarios.Add(new EscolaDanca.Api.Models.Usuario
            {
                Name = "Administrador1",
                Email = "admin@email.com",
                SenhaHash = "123",
                TipoUsuario = "Administrador",
                Status = "Ativo"
            });

            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro ao popular dados iniciais: " + ex.Message);
    }
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.OpenConnection(); // Testa a conexão com o PostgreSQL
        Console.WriteLine(" Conexão com o PostgreSQL Railway funcionando!");
        db.Database.CloseConnection();
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Erro ao conectar com o banco de dados:");
        Console.WriteLine(ex.Message);
    }
}

app.MapControllers();

app.Run();
