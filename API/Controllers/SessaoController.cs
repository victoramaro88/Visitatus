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
        public async Task<ActionResult<IEnumerable<Sessao>>> GetSessao(long id = 0)
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

        [HttpGet("{LojCodi}")]
        public Task<ActionResult<IEnumerable<SessaoListaModel>>> GetSessaoByLojCodi(long LojCodi)
        {
            try
            {
                if (LojCodi > 0)
                {
                    var result = (from s in _context.Sessaos
                                  join g in _context.Graus on s.GraCodi equals g.GraCodi
                                  join ts in _context.TipoSessaos on s.TiScodi equals ts.TiScodi
                                  select new SessaoListaModel
                                  {
                                      SesCodi = s.SesCodi,
                                      SesDesc = s.SesDesc,
                                      SesDtHr = s.SesDtHr,
                                      SesLibe = s.SesLibe,
                                      SesStat = s.SesStat,
                                      LojCodi = s.LojCodi,
                                      GraCodi = s.GraCodi,
                                      GraNome = g.GraNome,
                                      TiSCodi = s.TiScodi,
                                      TiSNome = ts.TiSnome,
                                      SesNume = (long)s.SesNume!,
                                      SesNome = s.SesNome
                                  })
                                  .OrderBy(x => x.SesNume)
                                  .ThenBy(x => x.SesDtHr)
                                  .ToList();

                    if (result == null || result.Count == 0)
                    {
                        return Task.FromResult<ActionResult<IEnumerable<SessaoListaModel>>>(NotFound());
                    }
                    else
                    {
                        return Task.FromResult<ActionResult<IEnumerable<SessaoListaModel>>>(Ok(result));
                    }
                }
                else
                {
                    return Task.FromResult<ActionResult<IEnumerable<SessaoListaModel>>>(BadRequest("Parâmetros Inválidos."));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{sesNume}/{lojCodi}")]
        public Task<ActionResult<IEnumerable<Sessao>>> GetValidaNumeroSessao(long sesNume, long lojCodi)
        {
            if (sesNume > 0 && lojCodi > 0)
            {
                var result = (from s in _context.Sessaos
                              join g in _context.Graus on s.GraCodi equals g.GraCodi
                              join ts in _context.TipoSessaos on s.TiScodi equals ts.TiScodi
                              select new SessaoListaModel
                              {
                                  SesCodi = s.SesCodi,
                                  SesDesc = s.SesDesc,
                                  SesDtHr = s.SesDtHr,
                                  SesLibe = s.SesLibe,
                                  SesStat = s.SesStat,
                                  LojCodi = s.LojCodi,
                                  GraCodi = s.GraCodi,
                                  GraNome = g.GraNome,
                                  TiSCodi = s.TiScodi,
                                  TiSNome = ts.TiSnome,
                                  SesNume = (long)s.SesNume!,
                                  SesNome = s.SesNome
                              }).FirstOrDefault(s => s.SesNume == sesNume && s.LojCodi == lojCodi);

                if (result == null)
                {
                    return Task.FromResult<ActionResult<IEnumerable<Sessao>>>(Ok(result));
                }
                else
                {
                    return Task.FromResult<ActionResult<IEnumerable<Sessao>>>(Ok(result));
                }
            }
            else
            {
                return Task.FromResult<ActionResult<IEnumerable<Sessao>>>(BadRequest("Parâmetros Inválidos."));
            }
        }


        [HttpPut("{sesCodi}")]
        public async Task<IActionResult> PutSessao(long sesCodi, [FromBody] Sessao sessao)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Sessao>> PostSessao([FromBody] Sessao sessao)
        {
            try
            {
                sessao.SesCodi = _context.Sessaos.Max(p => (int?)p.SesCodi) + 1 ?? 1;

                _context.Sessaos.Add(sessao);
                var retorno = await _context.SaveChangesAsync();

                return Ok("OK");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        private bool SessaoExists(long id)
        {
            return _context.Sessaos.Any(e => e.SesCodi == id);
        }
    }
}
