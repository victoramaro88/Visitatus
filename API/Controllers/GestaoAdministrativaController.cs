using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GestaoAdministrativaController : Controller
    {
        private readonly AppDbContext _context;

        public GestaoAdministrativaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{gstAdmCodi}")]
        public async Task<ActionResult<IEnumerable<GestaoAdministrativa>>> GetGestaoAdministrativaByGstAdmCodi(long gstAdmCodi = 0)
        {
            if (gstAdmCodi > 0)
            {
                var result = await _context.GestaoAdministrativas.FindAsync(gstAdmCodi);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<GestaoAdministrativa> { result });
                }
            }
            else
            {
                var result = await _context.GestaoAdministrativas.ToListAsync();

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

        [HttpGet("{lojCodi}")]
        public async Task<ActionResult<IEnumerable<GestaoAdministrativa>>> GetGestaoAtivaByLojCodi(long lojCodi)
        {
            if (lojCodi > 0)
            {
                List<GestaoAdmAtivaModel> result = await (from l in _context.Lojas
                              join ga in _context.GestaoAdministrativas on l.LojCodi equals ga.LojCodi
                              join gc in _context.GestaoCargos on ga.GstAdmCodi equals gc.GstAdmCodi
                              join c in _context.Cargos on gc.CarCodi equals c.CarCodi
                              join u in _context.Usuarios on gc.UsuCodi equals u.UsuCodi
                              where l.LojCodi == lojCodi &&
                                    ga.GstAdmStat == true &&
                                    ga.GstAdmDtIn <= DateTime.Today &&
                                    ga.GstAdmDtFi >= DateTime.Today
                              orderby ga.GstAdmDtFi descending
                              select new GestaoAdmAtivaModel
                              {
                                  LojCodi = l.LojCodi,
                                  LojNome = l.LojNome,
                                  LojNumL = l.LojNumL,
                                  GstAdmNome = ga.GstAdmNome,
                                  GstAdmDtIn = ga.GstAdmDtIn,
                                  GstAdmDtFi = ga.GstAdmDtFi,
                                  GstAdmStat = ga.GstAdmStat,
                                  CarNome = c.CarNome,
                                  UsuNome = u.UsuNome,
                                  CarCodi = c.CarCodi
                              }).ToListAsync();


                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok( result );
                }
            }
            else
            {
                return BadRequest("Parâmetros Inválidos.");
            }
        }
    }
}
