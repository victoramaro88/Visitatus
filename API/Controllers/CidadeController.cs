using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CidadeController : Controller
    {
        private readonly AppDbContext _context;

        public CidadeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Cidade>>> GetCidades(long id = 0)
        {
            if (id > 0)
            {
                var result = await _context.Cidades.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<Cidade> { result });
                }
            }
            else
            {
                var result = await _context.Cidades.ToListAsync();

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

        [HttpGet("{estCodi}")]
        public async Task<ActionResult<Cidade>> GetCidadesPorEstado(int estCodi)
        {
            // Verifica se o estado existe
            var estadoExiste = await _context.Estados.AnyAsync(e => e.EstCodi == estCodi);
            if (!estadoExiste)
            {
                return NotFound("Estado não encontrado.");
            }

            // Consulta cidades relacionadas ao estado
            var result = await _context.Cidades
                .Where(c => c.EstCodi == estCodi)
                .Select(c => new Cidade
                {
                    CidCodi = c.CidCodi,
                    CidNome = c.CidNome,
                    CidStat = c.CidStat
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
