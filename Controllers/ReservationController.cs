namespace GuiaDeMoteisAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using GuiaDeMoteisAPI.Data;
    using GuiaDeMoteisAPI.Models;
    using GuiaDeMoteisAPI.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService; // Serviço para autenticação JWT

        public ReservationController(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // Endpoint para listar reservas filtradas por data
        [HttpGet("reservations")]
        public async Task<IActionResult> GetReservations(DateTime startDate, DateTime endDate)
        {
            // Filtra as reservas com base nas datas de início e fim
            var reservations = await _context.Reservations
                .Where(r => r.StartDate >= startDate && r.EndDate <= endDate)
                .ToListAsync();

            return Ok(reservations);
        }

        // Endpoint otimizado para obter faturamento mensal
        [HttpGet("monthly-revenue")]
        public async Task<IActionResult> GetMonthlyRevenue(int month, int year)
        {
            var revenue = await _context.Reservations
                .Where(r => (r.StartDate.Month == month && r.StartDate.Year == year) ||
                            (r.EndDate.Month == month && r.EndDate.Year == year))
                .SumAsync(r => r.TotalAmount);

            return Ok(new { Revenue = revenue });
        }
    }
}






/////////////////////////
///namespace GuiaDeMoteisAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using GuiaDeMoteisAPI.Data; 
using GuiaDeMoteisAPI.Models;
using GuiaDeMoteisAPI.Services;
using Microsoft.AspNetCore.Authorization;


[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService; // Serviço para autenticação JWT

    public ReservationController(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }


    // Endpoint para listar reservas filtradas por data
[Authorize]
[HttpGet("reservations")]
public async Task<IActionResult> GetReservations(DateOnly startDate, DateOnly endDate)
{
    var reservations = await _context.Reservations
        .Where(r => r.StartDate >= startDate && r.EndDate <= endDate)
        .ToListAsync();

    return Ok(reservations);
}  // Endpoint otimizado para obter faturamento mensal
[HttpGet("monthly-revenue")]
public async Task<IActionResult> GetMonthlyRevenue(int month, int year)
{
    var revenue = await _context.Reservations
        .Where(r => r.StartDate.Month == month && r.StartDate.Year == year)
        .SumAsync(r => r.TotalAmount);

    return Ok(new { Revenue = revenue });
}