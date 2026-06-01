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

        [HttpGet("{SesCodi}")]
        public async Task<ActionResult<IEnumerable<Loja>>> GetLojaCertificadoBySesCodi(long SesCodi = 0)
        {
            if (SesCodi == 0)
            {
                return BadRequest("Parâmetros Inválidos.");
            }

            CertificadoSessaoModel objRetorno = new CertificadoSessaoModel();

            CertificadoDadosLojaModel? objCertificadoDadosLojaModel = await (from ses in _context.Sessaos
                                                                             join loj in _context.Lojas on ses.LojCodi equals loj.LojCodi
                                                                             join pot in _context.Potencia on loj.PotCodi equals pot.PotCodi
                                                                             join cid in _context.Cidades on loj.CidCodi equals cid.CidCodi
                                                                             join est in _context.Estados on cid.EstCodi equals est.EstCodi
                                                                             where ses.SesCodi == SesCodi
                                                                             select new CertificadoDadosLojaModel
                                                                             {
                                                                                 LojCodi = loj.LojCodi,
                                                                                 LojNome = loj.LojNome,
                                                                                 LojNumL = loj.LojNumL,
                                                                                 PotSigl = pot.PotSigl,
                                                                                 PotNome = pot.PotNome,
                                                                                 SesDtHr = ses.SesDtHr,
                                                                                 CidNome = cid.CidNome,
                                                                                 EstSigl = est.EstSigl,
                                                                                 LojLogo = loj.LojLogo,
                                                                                 PotLogo = pot.PotLogo
                                                                             }).FirstOrDefaultAsync();

            List<CertificadoDadosPresencaModel>? objCertificadoDadosPresencaModel = await (from pre in _context.Presencas
                                                                                           join usu in _context.Usuarios on pre.UsuCodi equals usu.UsuCodi
                                                                                           join loj in _context.Lojas on pre.LojCodi equals loj.LojCodi
                                                                                           join ses in _context.Sessaos on pre.SesCodi equals ses.SesCodi
                                                                                           join tip in _context.TipoSessaos on ses.TiScodi equals tip.TiScodi
                                                                                           where pre.SesCodi == SesCodi
                                                                                           select new CertificadoDadosPresencaModel
                                                                                           {
                                                                                               UsuCodi = usu.UsuCodi,
                                                                                               UsuNome = usu.UsuNome,
                                                                                               LojNome = loj.LojNome,
                                                                                               LojNumL = loj.LojNumL
                                                                                           }).ToListAsync();

            List<GestaoAdmAtivaModel> lstCargosGestaoLoja = await (from l in _context.Lojas
                                                                   join ga in _context.GestaoAdministrativas on l.LojCodi equals ga.LojCodi
                                                                   join gc in _context.GestaoCargos on ga.GstAdmCodi equals gc.GstAdmCodi
                                                                   join c in _context.Cargos on gc.CarCodi equals c.CarCodi
                                                                   join u in _context.Usuarios on gc.UsuCodi equals u.UsuCodi
                                                                   where l.LojCodi == objCertificadoDadosLojaModel!.LojCodi &&
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
                                                                       UsuNome = u.UsuNome
                                                                   }).ToListAsync();


            objRetorno.objCertificadoDadosLojaModel = objCertificadoDadosLojaModel;
            objRetorno.objCertificadoDadosPresencaModel = objCertificadoDadosPresencaModel;
            objRetorno.lstCargosGestaoLoja = lstCargosGestaoLoja;

            return Ok(objRetorno);
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
        public async Task<IActionResult> PutLoja(int lojCodi, [FromBody] Loja loja)
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
        public async Task<ActionResult<Loja>> PostLoja([FromBody] Loja loja)
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

        #region ORIENTAÇÃO LOJA
        [HttpGet("{lojCodi}")]
        public async Task<ActionResult<IEnumerable<OrientacaoLoja>>> GetOrientacaoLojaByLojCodi(long lojCodi = 0)
        {
            var result = await _context.OrientacaoLojas
                                .FirstOrDefaultAsync(x => x.LojCodi == lojCodi);

            if (result == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(new List<OrientacaoLoja> { result });
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrientacaoLoja>> PostOrientacaoLoja([FromBody] OrientacaoLoja orientacaoLoja)
        {
            try
            {
                if (OrientacaoLojaExists(orientacaoLoja.LojCodi))
                {
                    return BadRequest("Loja já possui orientação.");
                }

                orientacaoLoja.OrlCodi = _context.OrientacaoLojas.Max(p => (int?)p.OrlCodi) + 1 ?? 1;
                orientacaoLoja.OrlDtHr = DateTime.Now;

                _context.OrientacaoLojas.Add(orientacaoLoja);
                var retorno = await _context.SaveChangesAsync();

                return Ok(orientacaoLoja);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        [HttpPut("{orlCodi}")]
        public async Task<IActionResult> PutOrientacaoLoja(int orlCodi, [FromBody] OrientacaoLoja orientacaoLoja)
        {
            if (orlCodi != orientacaoLoja.OrlCodi)
            {
                return BadRequest();
            }

            _context.Entry(orientacaoLoja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrientacaoLojaExists(orlCodi))
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

        private bool OrientacaoLojaExists(long id)
        {
            return _context.OrientacaoLojas.Any(e => e.LojCodi == id);
        }
        #endregion
    }
}
