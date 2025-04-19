using EscolaDanca.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


app.MapControllers();

app.Run();
