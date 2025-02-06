using Microsoft.EntityFrameworkCore;
using GuiaDeMoteisAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionando o DbContext com a string de conexão
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.Run();
