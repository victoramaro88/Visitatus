using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SessaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SessaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Sessao>>> GetSessao(int id = 0)
        {
            if (id > 0)
            {
                var result = await _context.Sessaos.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<Sessao> { result });
                }
            }
            else
            {
                var result = await _context.Sessaos.ToListAsync();

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

        [HttpPut("{sesCodi}")]
        public async Task<IActionResult> PutSessao(int sesCodi, Sessao sessao)
        {
            if (sesCodi != sessao.SesCodi)
            {
                return BadRequest();
            }

            _context.Entry(sessao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessaoExists(sesCodi))
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
        public async Task<ActionResult<Sessao>> PostSessao(Sessao sessao)
        {
            try
            {
                sessao.SesCodi = _context.Sessaos.Max(p => (int?)p.SesCodi) + 1 ?? 1;

                _context.Sessaos.Add(sessao);
                var retorno = await _context.SaveChangesAsync();

                return Ok(sessao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        private bool SessaoExists(int id)
        {
            return _context.Sessaos.Any(e => e.SesCodi == id);
        }
    }
}
