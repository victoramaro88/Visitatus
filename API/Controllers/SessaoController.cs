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
                                      SesNome = s.SesNome,
                                      SesAgap = s.SesAgap,
                                      SesVlAg = s.SesVlAg
                                  })
                                  .Where(l => l.LojCodi == LojCodi)
                                  .OrderByDescending(x => x.SesDtHr)
                                  .ThenBy(x => x.SesNume)
                                  .ToList();

                    if (result == null || result.Count == 0)
                    {
                        return Task.FromResult<ActionResult<IEnumerable<SessaoListaModel>>>(Ok(result));
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

        [HttpGet("{sesCodi}")]
        public Task<ActionResult<IEnumerable<SessaoConviteModel>>> GetSessaoBySesCodi(long sesCodi)
        {
            try
            {
                if (sesCodi > 0)
                {
                    SessaoConviteModel result = (from s in _context.Sessaos
                                                 join g in _context.Graus on s.GraCodi equals g.GraCodi
                                                 join ts in _context.TipoSessaos on s.TiScodi equals ts.TiScodi
                                                 join l in _context.Lojas on s.LojCodi equals l.LojCodi
                                                 join p in _context.Potencia on l.PotCodi equals p.PotCodi
                                                 join r in _context.Ritos on l.RitCodi equals r.RitCodi
                                                 join c in _context.Cidades on l.CidCodi equals c.CidCodi
                                                 join e in _context.Estados on c.EstCodi equals e.EstCodi
                                                 where s.SesCodi == sesCodi
                                                 select new SessaoConviteModel
                                                 {
                                                     SesCodi = s.SesCodi,
                                                     SesNume = (long)s.SesNume!,
                                                     SesNome = s.SesNome,
                                                     SesDesc = s.SesDesc,
                                                     SesDtHr = s.SesDtHr,
                                                     SesLibe = s.SesLibe,
                                                     SesStat = s.SesStat,
                                                     TiSNome = ts.TiSnome,
                                                     GraNome = g.GraNome,
                                                     LojCodi = l.LojCodi,
                                                     LojNome = l.LojNome,
                                                     LojStat = l.LojStat,
                                                     LojNume = l.LojNume,
                                                     LojLogr = l.LojLogr,
                                                     LojNumL = l.LojNumL,
                                                     LojBair = l.LojBair,
                                                     PotNome = p.PotNome,
                                                     PotSigl = p.PotSigl,
                                                     PotRegu = p.PotRegu,
                                                     RitNome = r.RitNome,
                                                     CidNome = c.CidNome,
                                                     EstSigl = e.EstSigl,
                                                     LojLogo = l.LojLogo,
                                                     PotLogo = p.PotLogo,
                                                     SesAgap = s.SesAgap,
                                                     SesVlAg = s.SesVlAg
                                                 }).FirstOrDefault()!;

                    if (result != null)
                    {
                        List<GestaoAdmAtivaModel> lstGestaoAdmAtiva = (from l in _context.Lojas
                                                                       join ga in _context.GestaoAdministrativas on l.LojCodi equals ga.LojCodi
                                                                       join gc in _context.GestaoCargos on ga.GstAdmCodi equals gc.GstAdmCodi
                                                                       join c in _context.Cargos on gc.CarCodi equals c.CarCodi
                                                                       join u in _context.Usuarios on gc.UsuCodi equals u.UsuCodi
                                                                       where l.LojCodi == result.LojCodi &&
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
                                                                           CarCodi = c.CarCodi,
                                                                           CarNome = c.CarNome,
                                                                           UsuNome = u.UsuNome
                                                                       }).ToList();
                        if (lstGestaoAdmAtiva != null)
                        {
                            result.lstGestaoAdmAtiva = new List<GestaoAdmAtivaModel>();
                            result.lstGestaoAdmAtiva = lstGestaoAdmAtiva;
                        }
                    }


                    return Task.FromResult<ActionResult<IEnumerable<SessaoConviteModel>>>(Ok(result));
                }
                else
                {
                    return Task.FromResult<ActionResult<IEnumerable<SessaoConviteModel>>>(BadRequest("Parâmetros Inválidos."));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{lojCodi}")]
        public async Task<ActionResult<IEnumerable<ProximaSessaoModel>>> GetProximaSessao(long lojCodi)
        {
            try
            {
                if (lojCodi > 0)
                {
                    List<ProximaSessaoModel> result = await (from ses in _context.Sessaos
                                         join pre in _context.Presencas on ses.SesCodi equals pre.SesCodi into presencas
                                         from pre in presencas.DefaultIfEmpty() // LEFT JOIN
                                         where ses.LojCodi == lojCodi && ses.SesDtHr >= DateTime.Now
                                         group pre by new
                                         {
                                             ses.SesCodi,
                                             ses.SesNume,
                                             ses.SesDtHr,
                                             ses.SesLibe,
                                             ses.SesStat,
                                             ses.SesNome
                                         } into grouped
                                         orderby grouped.Key.SesDtHr ascending
                                         select new ProximaSessaoModel
                                         {
                                             SesCodi = grouped.Key.SesCodi,
                                             SesNume = grouped.Key.SesNume ?? 0,
                                             SesDtHr = grouped.Key.SesDtHr,
                                             SesLibe = grouped.Key.SesLibe,
                                             SesStat = grouped.Key.SesStat,
                                             SesNome = grouped.Key.SesNome,
                                             TotalPresenca = grouped.Count(pre => pre != null)
                                         }).ToListAsync();


                    return Ok(result);
                }
                else
                {
                    return BadRequest("Parâmetros Inválidos.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{sesCodi}/{sesLibe}")]
        public async Task<IActionResult> GetLiberaBloqueiaSessao(long sesCodi, bool sesLibe)
        {
            try
            {
                if (sesCodi == 0)
                {
                    return BadRequest();
                }

                var sessao = await _context.Sessaos.FindAsync(sesCodi);

                if (sessao == null)
                {
                    return NotFound("Sessão não encontrada.");
                }

                sessao.SesLibe = sesLibe;

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
                sessao.SesCodi = _context.Sessaos.Max(p => (long?)p.SesCodi) + 1 ?? 1;

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
