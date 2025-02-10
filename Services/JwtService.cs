using GuiaDeMoteisAPI.Models;
using GuiaDeMoteisAPI.Data; // Importando o AppDbContext
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace GuiaDeMoteisAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        Task<User> Authenticate(string username, string password);
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string password);
    }

    public class JwtService : IJwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly AppDbContext _context; // Adicionando o AppDbContext

        public JwtService(IConfiguration configuration, AppDbContext context) // Injeção de dependência
        {
            _secretKey = configuration["Jwt:Secret"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _context = context; // Inicializando o _context
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
                expires: DateTime.Now.AddHours(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await GetUserFromDatabase(username); // Consulta real ao banco de dados

            if (user != null && VerifyPassword(user.PasswordHash, password))
            {
                return user; // Se o usuário e senha estiverem corretos, retorna o usuário
            }

            return null; // Caso contrário, retorna null
        }

        public string HashPassword(string password)
        {
            // Usando bcrypt para hashing de senha
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            // Verifica se a senha fornecida corresponde ao hash armazenado
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private async Task<User> GetUserFromDatabase(string username)
        {
            // Consultando o banco de dados para buscar o usuário
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
