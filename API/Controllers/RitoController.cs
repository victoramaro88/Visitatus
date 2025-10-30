using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RitoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RitoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Rito>>> GetRito(int id = 0)
        {
            if (id > 0)
            {
                var result = await _context.Ritos.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<Rito> { result });
                }
            }
            else
            {
                var result = await _context.Ritos.ToListAsync();

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

        [HttpPut("{ritCodi}")]
        public async Task<IActionResult> PutRito(int ritCodi, Rito rito)
        {
            if (ritCodi != rito.RitCodi)
            {
                return BadRequest();
            }

            _context.Entry(rito).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RitoExists(ritCodi))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Alterado com sucesso!");
        }

        [HttpPost]
        public async Task<ActionResult<Rito>> PostRito(Rito rito)
        {
            try
            {
                rito.RitCodi = _context.Ritos.Max(p => (int?)p.RitCodi) + 1 ?? 1;

                _context.Ritos.Add(rito);
                var retorno = await _context.SaveChangesAsync();

                return Ok(rito);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        private bool RitoExists(int id)
        {
            return _context.Ritos.Any(e => e.RitCodi == id);
        }
    }
}
