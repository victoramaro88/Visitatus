using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PermissaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PermissaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Permissao>>> GetPermissao(int id = 0)
        {
            if (id > 0)
            {
                var result = await _context.Permissaos.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<Permissao> { result });
                }
            }
            else
            {
                var result = await _context.Permissaos.ToListAsync();

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

        [HttpPut("{pemCodi}")]
        public async Task<IActionResult> PutRito(int pemCodi, Permissao permissao)
        {
            if (pemCodi != permissao.PemCodi)
            {
                return BadRequest();
            }

            _context.Entry(permissao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissaoExists(pemCodi))
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
        public async Task<ActionResult<Rito>> PostPermissao(Permissao permissao)
        {
            try
            {
                permissao.PemCodi = _context.Permissaos.Max(p => (int?)p.PemCodi) + 1 ?? 1;

                _context.Permissaos.Add(permissao);
                var retorno = await _context.SaveChangesAsync();

                return Ok(permissao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        private bool PermissaoExists(int id)
        {
            return _context.Permissaos.Any(e => e.PemCodi == id);
        }
    }
}
