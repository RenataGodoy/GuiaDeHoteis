using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using GuiaDeMoteisAPI.Data;
using GuiaDeMoteisAPI.Models;
using GuiaDeMoteisAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging; 

namespace GuiaDeMoteisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService; // Serviço para autenticação JWT
        private readonly ILogger<AuthController> _logger; // Adicionando o ILogger aqui

        // Injeção de dependência para AppDbContext, IJwtService e ILogger
        public AuthController(AppDbContext context, IJwtService jwtService, ILogger<AuthController> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger; // Atribuindo o logger injetado
        }

        // Endpoint para login de usuário
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User model)
        {
            // Autentica o usuário
            var user = await _jwtService.Authenticate(model.Username, model.PasswordHash);
            if (user == null)
            {
                _logger.LogWarning("Tentativa de login falhou para o usuário: " + model.Username); // Log de aviso
                return Unauthorized(); // Retorna 401 se o usuário não for autenticado
            }

            // Gera o token JWT
            var token = _jwtService.GenerateToken(user);
            _logger.LogInformation("Usuário autenticado com sucesso: " + model.Username); // Log de informação

            return Ok(new { Token = token,           
            name = user.Username, 
                role = user.Role });
        }

        // Endpoint para registrar um novo usuário (cadastramento)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            // Verifica se o usuário já existe
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                _logger.LogWarning("Tentativa de registrar um usuário já existente: " + model.Username); // Log de aviso
                return BadRequest("Usuário já existe.");
            }

            // Cria o novo usuário
            var user = new User
            {
                Username = model.Username,
                PasswordHash = _jwtService.HashPassword(model.PasswordHash), // Supondo que você tenha um método HashPassword para criptografar a senha
                Role = model.Role 
            };

            // Adiciona o usuário no banco de dados
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Gera o token para o novo usuário
            var token = _jwtService.GenerateToken(user);
            _logger.LogInformation("Novo usuário registrado com sucesso: " + model.Username); // Log de informação

            return Ok(new { Token = token }); // Retorna o token JWT do novo usuário
        }

        // Endpoint para verificar se o usuário está autenticado (usando o token)
        [HttpGet("me")]
        [Authorize] // Apenas usuários autenticados podem acessar
        public async Task<IActionResult> GetUserData()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            // Log para verificar se o token está sendo processado corretamente
            if (userId == null)
            {
                _logger.LogWarning("Token não encontrado ou inválido.");
                return Unauthorized(new { error = "Usuário não autenticado" });
            }

            // Log para verificar o valor do userId extraído
            _logger.LogInformation("ID do usuário extraído do token: " + userId);

            var user = await _context.Users.FindAsync(int.Parse(userId));
            
            if (user == null)
            {
                _logger.LogWarning("Usuário não encontrado com o ID: " + userId);
                return NotFound(new { error = "Usuário não encontrado" });
            }

            // Log para confirmar a obtenção dos dados do usuário
            _logger.LogInformation("Usuário encontrado: " + user.Username);

            return Ok(new { 
                name = user.Username, 
                role = user.Role
            });
        }
    }
}

