using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        private bool PotenciaExists(int id)
        {
            return _context.Potencia.Any(e => e.PotCodi == id);
        }
    }
}
