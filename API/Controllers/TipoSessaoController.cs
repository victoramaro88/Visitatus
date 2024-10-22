using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TipoSessaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoSessaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TipoSessao>>> GetTipoSessao(int id = 0)
        {
            if (id > 0)
            {
                var result = await _context.TipoSessaos.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<TipoSessao> { result });
                }
            }
            else
            {
                var result = await _context.TipoSessaos.ToListAsync();

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

        [HttpPut("{tiSCodi}")]
        public async Task<IActionResult> PutTipoSessao(int tiSCodi, TipoSessao tipoSessao)
        {
            if (tiSCodi != tipoSessao.TiScodi)
            {
                return BadRequest();
            }

            _context.Entry(tipoSessao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoSessaoExists(tiSCodi))
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
        public async Task<ActionResult<TipoSessao>> PostTipoSessao(TipoSessao tipoSessao)
        {
            try
            {
                tipoSessao.TiScodi = _context.TipoSessaos.Max(p => (int?)p.TiScodi) + 1 ?? 1;

                _context.TipoSessaos.Add(tipoSessao);
                var retorno = await _context.SaveChangesAsync();

                return Ok(tipoSessao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        private bool TipoSessaoExists(int id)
        {
            return _context.TipoSessaos.Any(e => e.TiScodi == id);
        }
    }
}
