namespace GuiaDeMoteisAPI.Services;

using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using GuiaDeMoteisAPI.Services;
using GuiaDeMoteisAPI.Models;


public interface IJwtService
{
    string GenerateToken(User user);
    Task<User> Authenticate(string username, string password);
}

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User> Authenticate(string username, string password)
    {
        // Aqui você pode consultar seu banco de dados e verificar se o usuário existe
        // e se a senha está correta.
        // Se o usuário for válido, retorne o objeto User.

        return new User { Id = 1, Username = "TestUser" }; // Exemplo fictício
    }
}
