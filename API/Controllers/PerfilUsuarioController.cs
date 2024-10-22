using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PerfilUsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PerfilUsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<PerfilUsuario>>> GetPerfilUsuario(int id = 0)
        {
            if (id > 0)
            {
                var result = await _context.PerfilUsuarios.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<PerfilUsuario> { result });
                }
            }
            else
            {
                var result = await _context.PerfilUsuarios.ToListAsync();

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

        [HttpPut("{peUCodi}")]
        public async Task<IActionResult> PutPerfilUsuario(int peUCodi, PerfilUsuario perfilUsuario)
        {
            if (peUCodi != perfilUsuario.PeUcodi)
            {
                return BadRequest();
            }

            _context.Entry(perfilUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerfilUsuarioExists(peUCodi))
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
        public async Task<ActionResult<PerfilUsuario>> PostPerfilUsuario(PerfilUsuario perfilUsuario)
        {
            try
            {
                perfilUsuario.PeUcodi = _context.PerfilUsuarios.Max(p => (int?)p.PeUcodi) + 1 ?? 1;

                _context.PerfilUsuarios.Add(perfilUsuario);
                var retorno = await _context.SaveChangesAsync();

                return Ok(perfilUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        private bool PerfilUsuarioExists(int id)
        {
            return _context.PerfilUsuarios.Any(e => e.PeUcodi == id);
        }
    }
}
