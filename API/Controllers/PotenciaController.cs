using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PotenciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PotenciaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Potencium>>> GetPotencia(int id = 0)
        {
            if (id > 0)
            {
                var result = await _context.Potencia.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<Potencium> { result });
                }
            }
            else
            {
                var result = await _context.Potencia.ToListAsync();

                if (result == null || result.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
        }

        [HttpGet("{LojCodi}")]
        public Task<ActionResult<IEnumerable<Potencium>>> GetPotenciaRegularByLojCodi(int LojCodi)
        {
            if (LojCodi > 0)
            {
                var potenciaRegu = _context.Lojas
                    .Where(l => l.LojCodi == LojCodi)
                                .Join(
                        _context.Potencia,
                        l => l.PotCodi,
                        p => p.PotCodi,
                        (l, p) => p.PotRegu
                    )
                    .FirstOrDefault();

                var result = _context.Potencia
                    .Where(p => p.PotRegu == potenciaRegu)
                    .Select(p => new
                    {
                        p.PotCodi,
                        p.PotNome,
                        p.PotLogo,
                        p.PotRegu,
                        p.PotStat,
                        p.PotSigl
                    })
                    .ToList();

                if (result == null)
                {
                    return Task.FromResult<ActionResult<IEnumerable<Potencium>>>(NotFound());
                }
                else
                {
                    return Task.FromResult<ActionResult<IEnumerable<Potencium>>>(Ok( result ));
                }
            }
            else
            {
                return Task.FromResult<ActionResult<IEnumerable<Potencium>>>(BadRequest("Parâmetros Inválidos."));
            }
        }

        private bool PotenciaExists(int id)
        {
            return _context.Potencia.Any(e => e.PotCodi == id);
        }
    }
}
