using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TemplateLojaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TemplateLojaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{lojCodi}")]
        public async Task<ActionResult<TemplateLojaModel>> GetTemplateLoja(int lojCodi)
        {
            if (lojCodi > 0)
            {
                TemplateLojaModel? result = await _context.TemplateLojas
                    .Where(tl => tl.LojCodi == lojCodi)
                    .Join(
                        _context.TemplateConvites,
                        tl => tl.TmpCvtCodi,
                        tc => tc.TmpCvtCodi,
                        (tl, tc) => new TemplateLojaModel
                        {
                            TmpCvtCodi = tl.TmpCvtCodi,
                            TmpCvtNome = tc.TmpCvtNome,
                            TmpCvtMode = tc.TmpCvtMode,
                            TmpCvtStat = tc.TmpCvtStat
                        }
                    )
                    .FirstOrDefaultAsync();



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
