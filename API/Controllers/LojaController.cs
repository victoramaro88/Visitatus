using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class LojaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LojaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Loja>>> GetLoja(int id = 0)
        {
            if (id > 0)
            {
                var result = await _context.Lojas.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<Loja> { result });
                }
            }
            else
            {
                var result = await _context.Lojas.ToListAsync();

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

        [HttpPut("{lojCodi}")]
        public async Task<IActionResult> PutLoja(int lojCodi, Loja loja)
        {
            if (lojCodi != loja.LojCodi)
            {
                return BadRequest();
            }

            _context.Entry(loja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LojaExists(lojCodi))
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
        public async Task<ActionResult<Loja>> PostLoja(Loja loja)
        {
            try
            {
                loja.LojCodi = _context.Lojas.Max(p => (int?)p.LojCodi) + 1 ?? 1;

                _context.Lojas.Add(loja);
                var retorno = await _context.SaveChangesAsync();

                return Ok(loja);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        private bool LojaExists(int id)
        {
            return _context.Lojas.Any(e => e.LojCodi == id);
        }
    }
}
