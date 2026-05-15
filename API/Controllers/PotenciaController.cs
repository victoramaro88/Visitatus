using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Visitatus.Data.Repositories;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PotenciaController : ControllerBase
    {
        private readonly PotenciaRepository _potenciaRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly AppDbContext _context;

        public PotenciaController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, AppDbContext context)
        {
            _context = context; 
            _hostingEnvironment = hostingEnvironment;
            _potenciaRepo = new PotenciaRepository(configuration, _hostingEnvironment);
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Potencium>>> GetPotenciaComLogo(int id = 0)
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

        [HttpGet("{potCodi}")]
        public IActionResult GetPotencia(int potCodi = 0)
        {
            try
            {
                var ret = _potenciaRepo.GetPotencia(potCodi);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Potencium>>> ListaPotenciasSemLogo()
        {

            //var result = await _context.Potencia.ToListAsync();

            var result = await _context.Potencia
                            .Select(p => new Potencium
                            {
                                PotCodi = p.PotCodi,
                                PotNome = p.PotNome,
                                PotRegu = p.PotRegu,
                                PotStat = p.PotStat,
                                PotSigl = p.PotSigl
                            }).ToListAsync();

            if (result == null || result.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
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
                        //p.PotLogo,
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
                    return Task.FromResult<ActionResult<IEnumerable<Potencium>>>(Ok(result));
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
