using GuiaDeMoteisAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GuiaDeMoteisAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        Task<User> Authenticate(string username, string password);
        string HashPassword(string password);
    }

    public class JwtService : IJwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Secret"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> Authenticate(string username, string password)
        {
            // Simulação de autenticação simples, em um caso real, você deve verificar o usuário no banco de dados
            // Aqui está um exemplo básico de correspondência de senha
            var user = new User { Username = username, PasswordHash = "hashed_password" }; // Exemplo

            if (user != null && password == "senha123") // Simulação de senha simples
            {
                return user; // Se a senha corresponder, retorna o usuário
            }

            return null; // Se não encontrar, retorna null
        }

        public string HashPassword(string password)
        {
            // Aqui você pode adicionar sua lógica para gerar o hash da senha
            return password; // Simples por enquanto, use algo como bcrypt ou SHA256 no seu caso real
        }
    }
}
