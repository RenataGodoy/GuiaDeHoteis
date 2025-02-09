using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using GuiaDeMoteisAPI.Data;
using GuiaDeMoteisAPI.Models;
using GuiaDeMoteisAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace GuiaDeMoteisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService; // Serviço para autenticação JWT

        public AuthController(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // Endpoint para login de usuário
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User model)
        {
            // Autentica o usuário
            var user = await _jwtService.Authenticate(model.Username, model.PasswordHash);
            if (user == null) return Unauthorized(); // Retorna 401 se o usuário não for autenticado
            
            // Gera o token JWT
            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        // Endpoint para registrar um novo usuário (cadastramento)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            // Verifica se o usuário já existe
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                return BadRequest("Usuário já existe.");
            }

            // Cria o novo usuário
            var user = new User
            {
                Username = model.Username,
                PasswordHash = _jwtService.HashPassword(model.PasswordHash), // Supondo que você tenha um método HashPassword para criptografar a senha
                Role = "User" // Define o papel padrão como "User"
            };

            // Adiciona o usuário no banco de dados
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Gera o token para o novo usuário
            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token }); // Retorna o token JWT do novo usuário
        }

        // Endpoint para verificar se o usuário está autenticado (usando o token)
        [HttpGet("auth/me")]
        [Authorize] // Apenas usuários autenticados podem acessar
        public IActionResult Me()
        {
            // O usuário autenticado é obtido a partir do contexto do usuário (extraído do token JWT)
            var user = HttpContext.User;

            // Se o usuário não estiver autenticado, retorna um erro
            if (user == null)
            {
                return Unauthorized();
            }

            // Retorna as informações do usuário autenticado, como username e role (se houver)
            return Ok(new
            {
                Username = user.Identity.Name,
                Role = user.Claims.FirstOrDefault(c => c.Type == "Role")?.Value
            });
        }
    }
}
[HttpGet("auth/me")]
[Authorize] // Apenas usuários autenticados podem acessar
public async Task<IActionResult> GetUserData()
{
    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    if (userId == null)
        return Unauthorized(new { error = "Usuário não autenticado" });

    var user = await _context.Users.FindAsync(int.Parse(userId));
    if (user == null)
        return NotFound(new { error = "Usuário não encontrado" });

    return Ok(new { 
        name = user.Username, 
        role = user.Role.ToUpper() // Retorna o role em maiúsculo para evitar erros 
    });
}