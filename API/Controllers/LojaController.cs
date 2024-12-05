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

        [HttpGet("{LojCodi}")]
        public async Task<ActionResult<IEnumerable<Loja>>> GetLoja(long LojCodi = 0)
        {
            if (LojCodi > 0)
            {
                var result = await _context.Lojas.FindAsync(LojCodi);

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

        [HttpGet("{PotCodi}/{LojNumL}")]
        public async Task<ActionResult<IEnumerable<Loja>>> GetLojaByPotLojNume(int PotCodi, string LojNumL)
        {
            if (PotCodi > 0 && LojNumL.Length > 0)
            {
                var result = await _context.Lojas
                    .Where(l => l.PotCodi == PotCodi && l.LojNumL == LojNumL)
                    .FirstOrDefaultAsync();

                return Ok(result);
            }
            else
            {
                return BadRequest("Parâmetros inválidos.");
            }
        }

        [HttpGet("{usuCodi}")]
        public ActionResult<IEnumerable<Loja>> GetLojaByIdUsuario(long usuCodi)
        {
            if (usuCodi > 0)
            {
                var result = (from pu in _context.PerfilUsuarios
                              join u in _context.Usuarios on pu.UsuCodi equals u.UsuCodi
                              join l in _context.Lojas on pu.LojCodi equals l.LojCodi
                              where u.UsuCodi == usuCodi
                              select new Loja
                              {
                                  LojCodi = l.LojCodi,
                                  LojNome = l.LojNome,
                                  LojNumL = l.LojNumL
                              })
                              .ToList();

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            else
            {
                return BadRequest("Parâmetros Inválidos.");
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
