using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GuiaDeMoteisAPI.Data;
using GuiaDeMoteisAPI.Services;
using GuiaDeMoteisAPI.Models;
using GuiaDeMoteisAPI.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do JWT
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Certifique-se de que o valor está correto
            ValidAudience = builder.Configuration["Jwt:Audience"], // Certifique-se de que o valor está correto
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])) // Certifique-se de que a chave secreta está correta
        };
    });

// Adiciona controllers
builder.Services.AddControllers();

// Adicionar Swagger e configuração de autenticação no Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT no formato 'Bearer {token}'",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Configuração de CORS (se necessário)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Rodar o Seed para popular o banco de dados automaticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context); // Certifique-se de que DbInitializer está implementado corretamente
}

// Usar o CORS antes da autenticação
app.UseCors("AllowAll");

// Usar autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Configuração do Swagger (após a autenticação)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
        c.RoutePrefix = string.Empty; // Isso coloca o Swagger UI na raiz do projeto
    });
}

app.MapControllers();
app.Run();


// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using GuiaDeMoteisAPI.Data;
// using GuiaDeMoteisAPI.Services;
// using System.Text;

// var builder = WebApplication.CreateBuilder(args);

// // Configuração do DbContext
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// // Configuração do JWT
// builder.Services.AddScoped<IJwtService, JwtService>();

// var jwtKey = builder.Configuration["Jwt:Secret"];
// if (string.IsNullOrEmpty(jwtKey))
// {
//     throw new Exception("A chave JWT não está configurada corretamente.");
// }

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.RequireHttpsMetadata = false;
//         options.SaveToken = true;
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//         };
//     });

// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//     {
//         policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//     });
// });

// var app = builder.Build();

// app.UseCors("AllowAll");
// app.UseAuthentication();
// app.UseAuthorization();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.MapControllers();
// app.Run();

