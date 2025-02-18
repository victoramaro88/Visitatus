using API_Visitatus.Models;
using API_Visitatus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PresencaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;
        private readonly EncryptionService _encryptService;
        private readonly int _idModeloEmailCertificado;
        private readonly string _urlAplicacaoCertificado;

        public PresencaController(IConfiguration configuration, AppDbContext context, EmailService emailService, EncryptionService encryptService)
        {
            _context = context;
            _emailService = emailService;
            _encryptService = encryptService;
            _idModeloEmailCertificado = configuration.GetValue<int>("IdModeloEmailCertificado")!;
            _urlAplicacaoCertificado = configuration.GetValue<string>("UrlAplicacaoCertificado")!;
        }

        [HttpGet("{SesCodi}")]
        public async Task<ActionResult<IEnumerable<ListaPresencaModel>>> GetListaPresencaBySesCodi(long SesCodi = 0)
        {
            if (SesCodi == 0)
            {
                return BadRequest("Parâmetros Inválidos.");
            }

            //-> Pegando o código da Loja, pelo código da Sessão
            var lojCodi = await _context.Sessaos
                            .Where(s => s.SesCodi == SesCodi)
                            .Select(s => s.LojCodi)
                            .FirstOrDefaultAsync();

            List<ListaPresencaModel> listaRetorno = await (from pre in _context.Presencas
                                                           join usu in _context.Usuarios on pre.UsuCodi equals usu.UsuCodi
                                                           join loj in _context.Lojas on pre.LojCodi equals loj.LojCodi
                                                           join ses in _context.Sessaos on pre.SesCodi equals ses.SesCodi
                                                           join per in _context.PerfilUsuarios on usu.UsuCodi equals per.UsuCodi
                                                           where pre.SesCodi == SesCodi
                                                                && (per.PerCodi == 3 || per.PerCodi == 4 || per.PerCodi == 5) //-> Percodi = Vist/Memb/Fil
                                                           select new ListaPresencaModel
                                                           {
                                                               SesCodi = pre.SesCodi,
                                                               UsuCodi = usu.UsuCodi,
                                                               UsuNome = usu.UsuNome,
                                                               UsuNCIM = usu.UsuNcim,
                                                               LojCodi = loj.LojCodi,
                                                               LojNome = loj.LojNome,
                                                               LojNumL = loj.LojNumL,
                                                               PreAtiv = pre.PreAtiv,
                                                               PreEmai = pre.PreEmai,
                                                               MembroLoja = _context.Presencas.Any(preSub =>
                                                                   preSub.UsuCodi == pre.UsuCodi &&
                                                                   preSub.LojCodi == lojCodi &&
                                                                   _context.PerfilUsuarios.Any(perSub =>
                                                                       perSub.UsuCodi == preSub.UsuCodi &&
                                                                       (perSub.PerCodi == 4 || perSub.PerCodi == 5))) //-> Verifica se é membro ou filiado
                                                           })
                                                            .OrderBy(r => r.MembroLoja)
                                                            .ThenBy(r => r.UsuNome)
                                                            .ToListAsync();

            return Ok(listaRetorno);
        }

        [HttpGet("{usuCodi}/{sesCodi}")]
        public async Task<ActionResult<IEnumerable<string>>> GetCertificado(long usuCodi = 0, long sesCodi = 0)
        {
            if (usuCodi == 0 || sesCodi == 0)
            {
                return BadRequest("Parâmetros Inválidos.");
            }

            string htmlCorpo = "";

            try
            {
                //-> Retornando as informações da Loja para emitir o certificado
                var LojaCertificado = await (from ses in _context.Sessaos
                                             join tip in _context.TipoSessaos on ses.TiScodi equals tip.TiScodi
                                             join loj in _context.Lojas on ses.LojCodi equals loj.LojCodi
                                             join pot in _context.Potencia on loj.PotCodi equals pot.PotCodi
                                             join cid in _context.Cidades on loj.CidCodi equals cid.CidCodi
                                             join est in _context.Estados on cid.EstCodi equals est.EstCodi
                                             where ses.SesCodi == sesCodi
                                             select new
                                             {
                                                 loj.LojCodi,
                                                 loj.LojNome,
                                                 loj.LojNumL,
                                                 tip.TiSnome,
                                                 pot.PotSigl,
                                                 pot.PotNome,
                                                 cid.CidNome,
                                                 est.EstSigl,
                                                 ses.SesDtHr,
                                                 loj.LojLogo,
                                                 pot.PotLogo
                                             }).FirstOrDefaultAsync();

                //-> Retornando a Gestão Administrativa atual (cargos)
                if (LojaCertificado == null)
                {
                    return BadRequest("Sem informações da Loja para emissão do certificado.");
                }
                List<GestaoAdmAtivaModel> gestaoAdmAtual = await (from l in _context.Lojas
                                                                  join ga in _context.GestaoAdministrativas on l.LojCodi equals ga.LojCodi
                                                                  join gc in _context.GestaoCargos on ga.GstAdmCodi equals gc.GstAdmCodi
                                                                  join c in _context.Cargos on gc.CarCodi equals c.CarCodi
                                                                  join u in _context.Usuarios on gc.UsuCodi equals u.UsuCodi
                                                                  where l.LojCodi == LojaCertificado!.LojCodi &&
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

                //-> Retornando o template do certificado.
                var templateCertificado = await (from cer in _context.TemplateCertificadoLojas
                                                 join pre in _context.TemplateCertificadoPresencas on cer.TmpCrtPreCodi equals pre.TmpCrtPreCodi
                                                 join loj in _context.Lojas on cer.LojCodi equals loj.LojCodi
                                                 join ses in _context.Sessaos on loj.LojCodi equals ses.LojCodi
                                                 where ses.SesCodi == sesCodi
                                                 select new
                                                 {
                                                     pre.TmpCrtPreMode
                                                 }).FirstOrDefaultAsync();

                if (templateCertificado != null)
                {
                    htmlCorpo = templateCertificado.TmpCrtPreMode
                        .Replace("[LojLogo]", LojaCertificado!.LojLogo)
                        .Replace("[PotLogo]", LojaCertificado.PotLogo)
                        .Replace("[LojNome]", LojaCertificado.LojNome)
                        .Replace("[PotSigl]", LojaCertificado.PotSigl)
                        .Replace("[PotNome]", LojaCertificado.PotNome)
                        .Replace("[TipoSessao]", LojaCertificado.TiSnome)
                        .Replace("[CidadeLoja]", LojaCertificado.CidNome + ", " + LojaCertificado.EstSigl)
                        .Replace("[DataSessao]", LojaCertificado.SesDtHr.ToShortDateString())
                        .Replace("[NomeVM]", gestaoAdmAtual.Count > 0 ? gestaoAdmAtual.Where(g => g.CarCodi == 1).FirstOrDefault()!.UsuNome : "")
                        .Replace("[CargoVM]", gestaoAdmAtual.Count > 0 ? gestaoAdmAtual.Where(g => g.CarCodi == 1).FirstOrDefault()!.CarNome : "")
                        .Replace("[NomeSecretario]", gestaoAdmAtual.Count > 0 ? gestaoAdmAtual.Where(g => g.CarCodi == 4).FirstOrDefault()!.UsuNome : "")
                        .Replace("[CargoSecretario]", gestaoAdmAtual.Count > 0 ? gestaoAdmAtual.Where(g => g.CarCodi == 4).FirstOrDefault()!.CarNome : "")
                        ;
                }

                //-> Retornando os dados do usuário
                var dadosUsuario = await (from pre in _context.Presencas
                                          join usr in _context.Usuarios on pre.UsuCodi equals usr.UsuCodi
                                          join loj in _context.Lojas on pre.LojCodi equals loj.LojCodi
                                          where pre.SesCodi == sesCodi && usr.UsuCodi == usuCodi
                                          select new
                                          {
                                              usr.UsuNome,
                                              loj.LojNome,
                                              loj.LojNumL
                                          }).FirstOrDefaultAsync();

                if (dadosUsuario != null && dadosUsuario.UsuNome.Length > 0)
                {
                    htmlCorpo = htmlCorpo
                                     .Replace("[NomeUsuario]", dadosUsuario.UsuNome)
                                     .Replace("[NomeLoja]", dadosUsuario.LojNome)
                                     .Replace("[NumeroLoja]", dadosUsuario.LojNumL)
                                     ;
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao confirmar presença: {ex.Message} \n {ex.InnerException?.Message}");
            }

            //string testeURL = RetornaUrlCertificado(usuCodi, sesCodi);

            return Ok(htmlCorpo);
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostConfirmaPresenca([FromBody] ConsultaUsuarioLojaModel objPresenca)
        {
            if (objPresenca.objUsuarioLoja == null || objPresenca.objLojaConsulta == null || objPresenca.sesCodi == 0)
            {
                return BadRequest("Parâmetros inválidos.");
            }

            long novoUsuarioId = 0;
            long novoLojaId = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                #region Verificando se o usuário e Loja existem

                //-> Se o código da Loja vier 0, insere a nova Loja.
                if (objPresenca.objLojaConsulta.LojCodi == 0)
                {
                    Loja loja = new Loja
                    {
                        LojCodi = _context.Lojas.Max(p => (int?)p.LojCodi) + 1 ?? 1,
                        LojNome = objPresenca.objLojaConsulta.LojNome!,
                        LojNumL = objPresenca.objLojaConsulta.LojNumL!,
                        PotCodi = objPresenca.objLojaConsulta.PotCodi!,
                        LojStat = true
                    };

                    _context.Lojas.Add(loja);
                    await _context.SaveChangesAsync();

                    novoLojaId = loja.LojCodi;
                }

                //-> Se o usuário existir, verifica se já confirmou sua presença para esta sessão
                if (objPresenca.objUsuarioLoja.UsuCodi > 0)
                {
                    var confirmacaoPresenca = await _context.Presencas
                        .Where(p => p.UsuCodi == objPresenca.objUsuarioLoja.UsuCodi && p.SesCodi == objPresenca.sesCodi)
                        .FirstOrDefaultAsync();

                    if (confirmacaoPresenca != null)
                    {
                        return Ok("Presença já confirmada.");
                    }
                }
                //-> Senão, insere o usuário como MEMBRO na tabela Usuário e PerfilUsuario
                else
                {
                    Usuario usuario = new Usuario
                    {
                        UsuCodi = _context.Usuarios.Max(p => (int?)p.UsuCodi) + 1 ?? 1,
                        UsuNome = objPresenca.objUsuarioLoja.UsuNome!,
                        UsuNcim = objPresenca.objUsuarioLoja.UsuNCIM!,
                        UsuNasc = objPresenca.objUsuarioLoja.UsuNasc!,
                        UsuEmai = objPresenca.objUsuarioLoja.UsuEmai!,
                        UsuNcel = objPresenca.objUsuarioLoja.UsuNCel!,
                        UsuStat = true
                    };

                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();

                    novoUsuarioId = usuario.UsuCodi;

                    //-> Inserindo o perfil deste usuário como MEMBRO de sua Loja.
                    PerfilUsuario perfilUsuario = new PerfilUsuario
                    {
                        PeUcodi = _context.PerfilUsuarios.Max(p => (int?)p.PeUcodi) + 1 ?? 1,
                        PeUstat = true,
                        PerCodi = 4, //-> Perfil selecionado como MEMBRO.
                        UsuCodi = novoUsuarioId,
                        LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId
                    };

                    _context.PerfilUsuarios.Add(perfilUsuario);
                    await _context.SaveChangesAsync();
                }

                #endregion

                //-> Verificando se o usuário pertence a essa Loja
                if (novoUsuarioId == 0)
                {
                    //-> Verifica se o usuário pertence a essa Loja
                    var usuarioLojas = await (from usr in _context.Usuarios
                                              join perUsu in _context.PerfilUsuarios on usr.UsuCodi equals perUsu.UsuCodi
                                              join loj in _context.Lojas on perUsu.LojCodi equals loj.LojCodi
                                              join perf in _context.Perfils on perUsu.PerCodi equals perf.PerCodi
                                              where usr.UsuNcim == objPresenca.objUsuarioLoja.UsuNCIM && loj.PotCodi == objPresenca.objLojaConsulta.PotCodi
                                              orderby loj.LojNome
                                              select new
                                              {
                                                  usr.UsuCodi,
                                                  usr.UsuNome,
                                                  usr.UsuNasc,
                                                  usr.UsuEmai,
                                                  usr.UsuNcel,
                                                  usr.UsuStat,
                                                  usr.UsuNcim,
                                                  loj.LojCodi,
                                                  loj.LojNome,
                                                  loj.LojNumL,
                                                  loj.PotCodi,
                                                  perf.PerCodi,
                                                  perf.PerNome,
                                                  perf.PerStat
                                              }).ToListAsync();

                    //-> Verificando se o usuário é membro ou filiado a essa Loja
                    var usrPertencenteLoja = usuarioLojas.FindAll(u => u.LojCodi == objPresenca.objLojaConsulta.LojCodi);

                    //-> Se não pertencer, 
                    if(usrPertencenteLoja.Count == 0)
                    {
                        //-> Inserindo o perfil deste usuário como FILIADO desta Loja.
                        PerfilUsuario perfilUsuario = new PerfilUsuario
                        {
                            PeUcodi = _context.PerfilUsuarios.Max(p => (int?)p.PeUcodi) + 1 ?? 1,
                            PeUstat = true,
                            PerCodi = 5, //-> Perfil selecionado como FILIADO.
                            UsuCodi = objPresenca.objUsuarioLoja.UsuCodi,
                            LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId
                        };

                        _context.PerfilUsuarios.Add(perfilUsuario);
                        await _context.SaveChangesAsync();
                    }
                }

                //-> Por fim, insere a presença
                Presenca presenca = new Presenca
                {
                    UsuCodi = objPresenca.objUsuarioLoja.UsuCodi > 0 ? objPresenca.objUsuarioLoja.UsuCodi : novoUsuarioId,
                    SesCodi = objPresenca.sesCodi,
                    LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId,
                    PreAtiv = false,
                    PreEmai = false
                };

                _context.Presencas.Add(presenca);
                await _context.SaveChangesAsync();

                // Commit da transação
                await transaction.CommitAsync();
                return Ok("Presença confirmada com sucesso.");
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                await transaction.RollbackAsync();
                return BadRequest($"Erro ao confirmar presença: {ex.Message} \n {ex.InnerException?.Message}");
            }





















            //using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {




                //-> Se não vier id do usuário, verifica se já existe, senão, insere ele na tabela, vinculando-o com a Loja e atribuindo o perfil código 4 (Membro).
                if (objPresenca.objUsuarioLoja.UsuCodi == 0)
                {
                    //-> Faz uma consulta, para saber se o usuário já existe na base, mas em outra Loja
                    var usuarioOutraLoja = await _context.Usuarios
                        .Where(u => u.UsuNome.ToUpper() == objPresenca.objUsuarioLoja.UsuNome!.ToUpper()
                            && u.UsuNcim.ToUpper() == objPresenca.objUsuarioLoja.UsuNCIM!.ToUpper())
                        .FirstOrDefaultAsync();

                    //var usuarioOutraLoja = await (from usr in _context.Usuarios
                    //                                    join perUsu in _context.PerfilUsuarios on usr.UsuCodi equals perUsu.UsuCodi
                    //                                    join loj in _context.Lojas on perUsu.LojCodi equals loj.LojCodi
                    //                                    join perf in _context.Perfils on perUsu.PerCodi equals perf.PerCodi
                    //                                    join login in _context.UsuarioLogins on usr.UsuCodi equals login.UsuCodi into loginGroup
                    //                                    from login in loginGroup.DefaultIfEmpty() // Left join aqui
                    //                                    where usr.UsuNcim == objPresenca.objUsuarioLoja.UsuNCIM && loj.PotCodi == objPresenca.objLojaConsulta.PotCodi
                    //                              orderby loj.LojNome
                    //                                    select new
                    //                                    {
                    //                                        usr.UsuCodi
                    //                                    }).FirstOrDefaultAsync();

                    if (usuarioOutraLoja != null)
                    {
                        //-> Inserindo o perfil deste usuário como FILIADO desta Loja.
                        PerfilUsuario perfilUsuario = new PerfilUsuario
                        {
                            PeUcodi = _context.PerfilUsuarios.Max(p => (int?)p.PeUcodi) + 1 ?? 1,
                            PeUstat = true,
                            PerCodi = 5, //-> Perfil selecionado como FILIADO.
                            UsuCodi = usuarioOutraLoja.UsuCodi,
                            LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId
                        };

                        _context.PerfilUsuarios.Add(perfilUsuario);
                        await _context.SaveChangesAsync();

                        Presenca presenca = new Presenca
                        {
                            UsuCodi = usuarioOutraLoja.UsuCodi,
                            SesCodi = objPresenca.sesCodi,
                            LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId,
                            PreAtiv = false
                        };

                        _context.Presencas.Add(presenca);
                        await _context.SaveChangesAsync();
                    }
                    else //-> Caso a pesquisa venha null, insere o usuário
                    {
                        Usuario usuario = new Usuario
                        {
                            UsuCodi = _context.Usuarios.Max(p => (int?)p.UsuCodi) + 1 ?? 1,
                            UsuNome = objPresenca.objUsuarioLoja.UsuNome!,
                            UsuNcim = objPresenca.objUsuarioLoja.UsuNCIM!,
                            UsuNasc = objPresenca.objUsuarioLoja.UsuNasc!,
                            UsuEmai = objPresenca.objUsuarioLoja.UsuEmai!,
                            UsuNcel = objPresenca.objUsuarioLoja.UsuNCel!,
                            UsuStat = true
                        };

                        _context.Usuarios.Add(usuario);
                        await _context.SaveChangesAsync();

                        novoUsuarioId = usuario.UsuCodi;

                        //-> Inserindo o perfil deste usuário como MEMBRO de sua Loja.
                        PerfilUsuario perfilUsuario = new PerfilUsuario
                        {
                            PeUcodi = _context.PerfilUsuarios.Max(p => (int?)p.PeUcodi) + 1 ?? 1,
                            PeUstat = true,
                            PerCodi = 4, //-> Perfil selecionado como MEMBRO.
                            UsuCodi = novoUsuarioId,
                            LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId
                        };

                        _context.PerfilUsuarios.Add(perfilUsuario);
                        await _context.SaveChangesAsync();

                        //-> Insere a presença
                        Presenca presenca = new Presenca
                        {
                            UsuCodi = novoUsuarioId,
                            SesCodi = objPresenca.sesCodi,
                            LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId,
                            PreAtiv = false,
                            PreEmai = false
                        };

                        _context.Presencas.Add(presenca);
                        await _context.SaveChangesAsync();
                    }
                }
                else //-> Se já existir usuário na pesquisa vindo da aplicação, insere na tabela de presença.
                {
                    Presenca presenca = new Presenca
                    {
                        UsuCodi = objPresenca.objUsuarioLoja.UsuCodi,
                        SesCodi = objPresenca.sesCodi,
                        LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId,
                        PreAtiv = false,
                        PreEmai = false
                    };

                    _context.Presencas.Add(presenca);
                    await _context.SaveChangesAsync();
                }

                // Commit da transação
                await transaction.CommitAsync();
                return Ok("Presença confirmada com sucesso.");
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                await transaction.RollbackAsync();
                return BadRequest($"Erro ao confirmar presença: {ex.Message} \n {ex.InnerException?.Message}");
            }
        }

        //[HttpPost]
        //public async Task<ActionResult<string>> PostConfirmaPresenca([FromBody] ConsultaUsuarioLojaModel objPresenca)
        //{
        //    long novoUsuarioId = 0;
        //    long novoLojaId = 0;
        //    if (objPresenca.objUsuarioLoja == null || objPresenca.objLojaConsulta == null || objPresenca.sesCodi == 0)
        //    {
        //        return BadRequest("Parâmetros inválidos.");
        //    }

        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        //-> Se o usuário existir, verifica se já confirmou sua presença para esta sessão
        //        if (objPresenca.objUsuarioLoja.UsuCodi > 0)
        //        {
        //            var confirmacaoPresenca = await _context.Presencas
        //                .Where(p => p.UsuCodi == objPresenca.objUsuarioLoja.UsuCodi && p.SesCodi == objPresenca.sesCodi)
        //                .FirstOrDefaultAsync();

        //            if (confirmacaoPresenca != null)
        //            {
        //                return Ok("Presença já confirmada.");
        //            }
        //        }

        //        //-> Se o código da Loja vier 0, insere a nova Loja.
        //        if (objPresenca.objLojaConsulta.LojCodi == 0)
        //        {
        //            Loja loja = new Loja
        //            {
        //                LojCodi = _context.Lojas.Max(p => (int?)p.LojCodi) + 1 ?? 1,
        //                LojNome = objPresenca.objLojaConsulta.LojNome!,
        //                LojNumL = objPresenca.objLojaConsulta.LojNumL!,
        //                PotCodi = objPresenca.objLojaConsulta.PotCodi!,
        //                LojStat = true
        //            };

        //            _context.Lojas.Add(loja);
        //            await _context.SaveChangesAsync();

        //            novoLojaId = loja.LojCodi;
        //        }

        //        //-> Se não vier id do usuário, verifica se já existe, senão, insere ele na tabela, vinculando-o com a Loja e atribuindo o perfil código 4 (Membro).
        //        if (objPresenca.objUsuarioLoja.UsuCodi == 0)
        //        {
        //            //-> Faz uma consulta, para saber se o usuário já existe na base, mas em outra Loja
        //            var usuarioOutraLoja = await _context.Usuarios
        //                .Where(u => u.UsuNome.ToUpper() == objPresenca.objUsuarioLoja.UsuNome!.ToUpper()
        //                    && u.UsuNcim.ToUpper() == objPresenca.objUsuarioLoja.UsuNCIM!.ToUpper())
        //                .FirstOrDefaultAsync();

        //            //var usuarioOutraLoja = await (from usr in _context.Usuarios
        //            //                                    join perUsu in _context.PerfilUsuarios on usr.UsuCodi equals perUsu.UsuCodi
        //            //                                    join loj in _context.Lojas on perUsu.LojCodi equals loj.LojCodi
        //            //                                    join perf in _context.Perfils on perUsu.PerCodi equals perf.PerCodi
        //            //                                    join login in _context.UsuarioLogins on usr.UsuCodi equals login.UsuCodi into loginGroup
        //            //                                    from login in loginGroup.DefaultIfEmpty() // Left join aqui
        //            //                                    where usr.UsuNcim == objPresenca.objUsuarioLoja.UsuNCIM && loj.PotCodi == objPresenca.objLojaConsulta.PotCodi
        //            //                              orderby loj.LojNome
        //            //                                    select new
        //            //                                    {
        //            //                                        usr.UsuCodi
        //            //                                    }).FirstOrDefaultAsync();

        //            if (usuarioOutraLoja != null)
        //            {
        //                //-> Inserindo o perfil deste usuário como FILIADO desta Loja.
        //                PerfilUsuario perfilUsuario = new PerfilUsuario
        //                {
        //                    PeUcodi = _context.PerfilUsuarios.Max(p => (int?)p.PeUcodi) + 1 ?? 1,
        //                    PeUstat = true,
        //                    PerCodi = 5, //-> Perfil selecionado como FILIADO.
        //                    UsuCodi = usuarioOutraLoja.UsuCodi,
        //                    LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId
        //                };

        //                _context.PerfilUsuarios.Add(perfilUsuario);
        //                await _context.SaveChangesAsync();

        //                Presenca presenca = new Presenca
        //                {
        //                    UsuCodi = usuarioOutraLoja.UsuCodi,
        //                    SesCodi = objPresenca.sesCodi,
        //                    LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId,
        //                    PreAtiv = false
        //                };

        //                _context.Presencas.Add(presenca);
        //                await _context.SaveChangesAsync();
        //            }
        //            else //-> Caso a pesquisa venha null, insere o usuário
        //            {
        //                Usuario usuario = new Usuario
        //                {
        //                    UsuCodi = _context.Usuarios.Max(p => (int?)p.UsuCodi) + 1 ?? 1,
        //                    UsuNome = objPresenca.objUsuarioLoja.UsuNome!,
        //                    UsuNcim = objPresenca.objUsuarioLoja.UsuNCIM!,
        //                    UsuNasc = objPresenca.objUsuarioLoja.UsuNasc!,
        //                    UsuEmai = objPresenca.objUsuarioLoja.UsuEmai!,
        //                    UsuNcel = objPresenca.objUsuarioLoja.UsuNCel!,
        //                    UsuStat = true
        //                };

        //                _context.Usuarios.Add(usuario);
        //                await _context.SaveChangesAsync();

        //                novoUsuarioId = usuario.UsuCodi;

        //                //-> Inserindo o perfil deste usuário como MEMBRO de sua Loja.
        //                PerfilUsuario perfilUsuario = new PerfilUsuario
        //                {
        //                    PeUcodi = _context.PerfilUsuarios.Max(p => (int?)p.PeUcodi) + 1 ?? 1,
        //                    PeUstat = true,
        //                    PerCodi = 4, //-> Perfil selecionado como MEMBRO.
        //                    UsuCodi = novoUsuarioId,
        //                    LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId
        //                };

        //                _context.PerfilUsuarios.Add(perfilUsuario);
        //                await _context.SaveChangesAsync();

        //                //-> Insere a presença
        //                Presenca presenca = new Presenca
        //                {
        //                    UsuCodi = novoUsuarioId,
        //                    SesCodi = objPresenca.sesCodi,
        //                    LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId,
        //                    PreAtiv = false,
        //                    PreEmai = false
        //                };

        //                _context.Presencas.Add(presenca);
        //                await _context.SaveChangesAsync();
        //            }
        //        }
        //        else //-> Se já existir usuário na pesquisa vindo da aplicação, insere na tabela de presença.
        //        {
        //            Presenca presenca = new Presenca
        //            {
        //                UsuCodi = objPresenca.objUsuarioLoja.UsuCodi,
        //                SesCodi = objPresenca.sesCodi,
        //                LojCodi = objPresenca.objLojaConsulta.LojCodi > 0 ? objPresenca.objLojaConsulta.LojCodi : novoLojaId,
        //                PreAtiv = false,
        //                PreEmai = false
        //            };

        //            _context.Presencas.Add(presenca);
        //            await _context.SaveChangesAsync();
        //        }

        //        // Commit da transação
        //        await transaction.CommitAsync();
        //        return Ok("Presença confirmada com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Rollback em caso de erro
        //        await transaction.RollbackAsync();
        //        return BadRequest($"Erro ao confirmar presença: {ex.Message} \n {ex.InnerException?.Message}");
        //    }
        //}

        #region CONFIRMAÇÃO DE PRESENÇA E ENVIO DE E-MAIL
        [HttpPost]
        public async Task<ActionResult<string>> PostLancamentoPresencaSessao([FromBody] List<ListaPresencaModel> listaPresentes)
        {
            if (listaPresentes == null || listaPresentes.Count == 0)
            {
                return BadRequest("Parâmetros inválidos.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Obter informações da loja e do certificado
                var lojaCertificado = await ObterLojaCertificado(listaPresentes[0].SesCodi);
                if (lojaCertificado == null)
                {
                    return BadRequest("Sessão ou Loja não encontrada.");
                }

                // Obter a gestão administrativa atual
                var gestaoAdmAtual = await ObterGestaoAdministrativa(lojaCertificado.LojCodi);

                // Gerar o template do certificado
                //var templateCertificado = await ObterTemplateCertificado(listaPresentes[0].SesCodi);
                //if (templateCertificado == null)
                //{
                //    return BadRequest("Template de certificado não encontrado.");
                //}

                //string htmlCorpoBase = GerarHtmlCertificado(templateCertificado.TmpCrtPreMode, lojaCertificado, gestaoAdmAtual);

                //-> Gerar template do e-mail a ser enviado
                var htmlCorpoBase = await GerarHtmlEmailCertificado(_idModeloEmailCertificado, lojaCertificado);

                // Processar lista de presenças
                foreach (var itemPresente in listaPresentes)
                {
                    await ProcessarPresenca(itemPresente, htmlCorpoBase, lojaCertificado, gestaoAdmAtual);
                }

                // Commit da transação
                await transaction.CommitAsync();
                return Ok("Presença confirmada com sucesso.");
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                await transaction.RollbackAsync();
                return BadRequest($"Erro ao confirmar presença: {ex.Message} \n {ex.InnerException?.Message}");
            }
        }

        //-> Obter Dados da Loja e do Certificado:
        private async Task<dynamic?> ObterLojaCertificado(long sesCodi)
        {
            return await (from ses in _context.Sessaos
                          join tip in _context.TipoSessaos on ses.TiScodi equals tip.TiScodi
                          join loj in _context.Lojas on ses.LojCodi equals loj.LojCodi
                          join pot in _context.Potencia on loj.PotCodi equals pot.PotCodi
                          join cid in _context.Cidades on loj.CidCodi equals cid.CidCodi
                          join est in _context.Estados on cid.EstCodi equals est.EstCodi
                          where ses.SesCodi == sesCodi
                          select new
                          {
                              loj.LojCodi,
                              loj.LojNome,
                              loj.LojNumL,
                              tip.TiSnome,
                              pot.PotSigl,
                              pot.PotNome,
                              cid.CidNome,
                              est.EstSigl,
                              ses.SesDtHr,
                              loj.LojLogo,
                              pot.PotLogo
                          }).FirstOrDefaultAsync();
        }

        //-> Obter Gestão Administrativa:
        private async Task<List<GestaoAdmAtivaModel>> ObterGestaoAdministrativa(long lojCodi)
        {
            return await (from l in _context.Lojas
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
        }

        //-> Obter o template do Certificado:
        private async Task<dynamic?> ObterTemplateCertificado(long sesCodi)
        {
            var templateCertificado = await (from cer in _context.TemplateCertificadoLojas
                                             join pre in _context.TemplateCertificadoPresencas on cer.TmpCrtPreCodi equals pre.TmpCrtPreCodi
                                             join loj in _context.Lojas on cer.LojCodi equals loj.LojCodi
                                             join ses in _context.Sessaos on loj.LojCodi equals ses.LojCodi
                                             where ses.SesCodi == sesCodi
                                             select new
                                             {
                                                 pre.TmpCrtPreMode
                                             }).FirstOrDefaultAsync();

            return templateCertificado;
        }

        //-> Gerar o HTML do Certificado:
        private string GerarHtmlCertificado(string template, dynamic loja, List<GestaoAdmAtivaModel> gestaoAdm)
        {
            return template
                .Replace("[LojLogo]", loja.LojLogo)
                .Replace("[PotLogo]", loja.PotLogo)
                .Replace("[LojNome]", loja.LojNome)
                .Replace("[PotSigl]", loja.PotSigl)
                .Replace("[PotNome]", loja.PotNome)
                .Replace("[TipoSessao]", loja.TiSnome)
                .Replace("[CidadeLoja]", loja.CidNome + ", " + loja.EstSigl)
                .Replace("[DataSessao]", loja.SesDtHr.ToShortDateString())
                .Replace("[NomeVM]", gestaoAdm.FirstOrDefault(g => g.CarCodi == 1)?.UsuNome ?? "")
                .Replace("[CargoVM]", gestaoAdm.FirstOrDefault(g => g.CarCodi == 1)?.CarNome ?? "")
                .Replace("[NomeSecretario]", gestaoAdm.FirstOrDefault(g => g.CarCodi == 4)?.UsuNome ?? "")
                .Replace("[CargoSecretario]", gestaoAdm.FirstOrDefault(g => g.CarCodi == 4)?.CarNome ?? "");
        }

        //-> Obter o template do e-mail do certificado
        private async Task<string> GerarHtmlEmailCertificado(int tmpCrtPreCodi, dynamic loja)
        {
            string? retEmail = "";

            retEmail = await _context.TemplateCertificadoPresencas
                .Where(template => template.TmpCrtPreCodi == tmpCrtPreCodi)
                .Select(template => template.TmpCrtPreMode)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(retEmail))
            {
                throw new Exception("Template de email não encontrado.");
            }

            return retEmail
                .Replace("[NomeLoja]", loja.LojNome)
                .Replace("[NumeroLoja]", loja.LojNumL)
                .Replace("[PotenciaSigla]", loja.PotSigl)
                .Replace("[DataSessao]", loja.SesDtHr.ToShortDateString())
                .Replace("[HoraSessao]", loja.SesDtHr.ToShortTimeString())
                .Replace("[TipoSessao]", loja.TiSnome);
        }


        //-> Processar Presença:
        private async Task ProcessarPresenca(ListaPresencaModel itemPresente, string htmlCorpoBase, dynamic loja, List<GestaoAdmAtivaModel> gestaoAdm)
        {
            bool emailEnviado = itemPresente.PreEmai;
            if (itemPresente.PreAtiv && !itemPresente.PreEmai)
            {
                if (!itemPresente.MembroLoja)
                {
                    var emailUsuario = await _context.Usuarios
                        .Where(u => u.UsuCodi == itemPresente.UsuCodi)
                        .Select(u => u.UsuEmai)
                        .FirstOrDefaultAsync();

                    string testeURL = RetornaUrlCertificado(itemPresente.UsuCodi, itemPresente.SesCodi);

                    if (!string.IsNullOrEmpty(emailUsuario))
                    {
                        string htmlCorpoTmp = htmlCorpoBase
                            .Replace("[PrimeiroNome]", itemPresente.UsuNome!.Split(" ")[0])
                            .Replace("[UrlCertificado]", testeURL)
                            ;

                        await _emailService.EnviarEmailAsync(emailUsuario, "Certificado", htmlCorpoTmp);
                        emailEnviado = true;
                    }
                }
            }

            //-> Modo antigo, que retornava o certificado completo
            //private async Task ProcessarPresenca(ListaPresencaModel itemPresente, string htmlCorpoBase, dynamic loja, List<GestaoAdmAtivaModel> gestaoAdm)
            //{
            //    bool emailEnviado = itemPresente.PreEmai;
            //    if (itemPresente.PreAtiv && !itemPresente.PreEmai)
            //    {
            //        if (!itemPresente.MembroLoja)
            //        {
            //            var emailUsuario = await _context.Usuarios
            //                .Where(u => u.UsuCodi == itemPresente.UsuCodi)
            //                .Select(u => u.UsuEmai)
            //                .FirstOrDefaultAsync();

            //            if (!string.IsNullOrEmpty(emailUsuario))
            //            {
            //                string htmlCorpoTmp = htmlCorpoBase
            //                    .Replace("[PrimeiroNome]", itemPresente.UsuNome)
            //                    .Replace("[NomeLoja]", loja.LojNome)
            //                    .Replace("[NumeroLoja]", loja.LojNumL);

            //                await _emailService.EnviarEmailAsync(emailUsuario, "Certificado", htmlCorpoTmp);
            //                emailEnviado = true;
            //            }
            //        }
            //    }
            //else if(!itemPresente.PreAtiv && itemPresente.PreEmai)
            //{
            //    emailEnviado = false;
            //}

            var presenca = new Presenca
            {
                UsuCodi = itemPresente.UsuCodi,
                SesCodi = itemPresente.SesCodi,
                PreAtiv = itemPresente.PreAtiv,
                LojCodi = itemPresente.LojCodi,
                PreEmai = itemPresente.MembroLoja ? false : emailEnviado
            };

            _context.Entry(presenca).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        #endregion

        //[HttpPost]
        //public async Task<ActionResult<string>> PostLancamentoPresencaSessao([FromBody] List<ListaPresencaModel> listaPresentes)
        //{
        //    if (listaPresentes.Count == 0)
        //    {
        //        return BadRequest("Parâmetros inválidos.");
        //    }

        //    string destinatario = "";
        //    string assunto = "";
        //    string htmlCorpo = "";

        //    //using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        //-> Retornando as informações da Loja para emitir o certificado
        //        var LojaCertificado = await (from ses in _context.Sessaos
        //                                     join tip in _context.TipoSessaos on ses.TiScodi equals tip.TiScodi
        //                                     join loj in _context.Lojas on ses.LojCodi equals loj.LojCodi
        //                                     join pot in _context.Potencia on loj.PotCodi equals pot.PotCodi
        //                                     join cid in _context.Cidades on loj.CidCodi equals cid.CidCodi
        //                                     join est in _context.Estados on cid.EstCodi equals est.EstCodi
        //                                     where ses.SesCodi == listaPresentes[0].SesCodi
        //                                     select new
        //                                     {
        //                                         loj.LojCodi,
        //                                         loj.LojNome,
        //                                         loj.LojNumL,
        //                                         tip.TiSnome,
        //                                         pot.PotSigl,
        //                                         pot.PotNome,
        //                                         cid.CidNome,
        //                                         est.EstSigl,
        //                                         ses.SesDtHr,
        //                                         loj.LojLogo,
        //                                         pot.PotLogo
        //                                     }).FirstOrDefaultAsync();

        //        assunto = "Certificado de Presença - "
        //            + LojaCertificado!.LojNome
        //            + ", " + LojaCertificado!.LojNumL
        //            + " - " + LojaCertificado!.SesDtHr.ToShortDateString();

        //        //-> Retornando a Gestão Administrativa atual (cargos)
        //        List<GestaoAdmAtivaModel> gestaoAdmAtual = await (from l in _context.Lojas
        //                                                          join ga in _context.GestaoAdministrativas on l.LojCodi equals ga.LojCodi
        //                                                          join gc in _context.GestaoCargos on ga.GstAdmCodi equals gc.GstAdmCodi
        //                                                          join c in _context.Cargos on gc.CarCodi equals c.CarCodi
        //                                                          join u in _context.Usuarios on gc.UsuCodi equals u.UsuCodi
        //                                                          where l.LojCodi == LojaCertificado.LojCodi &&
        //                                                                ga.GstAdmStat == true &&
        //                                                                ga.GstAdmDtIn <= DateTime.Today &&
        //                                                                ga.GstAdmDtFi >= DateTime.Today
        //                                                          orderby ga.GstAdmDtFi descending
        //                                                          select new GestaoAdmAtivaModel
        //                                                          {
        //                                                              LojCodi = l.LojCodi,
        //                                                              LojNome = l.LojNome,
        //                                                              LojNumL = l.LojNumL,
        //                                                              GstAdmNome = ga.GstAdmNome,
        //                                                              GstAdmDtIn = ga.GstAdmDtIn,
        //                                                              GstAdmDtFi = ga.GstAdmDtFi,
        //                                                              GstAdmStat = ga.GstAdmStat,
        //                                                              CarNome = c.CarNome,
        //                                                              UsuNome = u.UsuNome,
        //                                                              CarCodi = c.CarCodi
        //                                                          }).ToListAsync();

        //        //-> Retornando o template do certificado.
        //        var templateCertificado = await (from cer in _context.TemplateCertificadoLojas
        //                                         join pre in _context.TemplateCertificadoPresencas on cer.TmpCrtPreCodi equals pre.TmpCrtPreCodi
        //                                         join loj in _context.Lojas on cer.LojCodi equals loj.LojCodi
        //                                         join ses in _context.Sessaos on loj.LojCodi equals ses.LojCodi
        //                                         where ses.SesCodi == listaPresentes[0].SesCodi
        //                                         select new
        //                                         {
        //                                             pre.TmpCrtPreMode
        //                                         }).FirstOrDefaultAsync();

        //        if (templateCertificado != null)
        //        {
        //            htmlCorpo = templateCertificado.TmpCrtPreMode
        //                .Replace("[LojLogo]", LojaCertificado.LojLogo)
        //                .Replace("[PotLogo]", LojaCertificado.PotLogo)
        //                .Replace("[LojNome]", LojaCertificado.LojNome)
        //                .Replace("[PotSigl]", LojaCertificado.PotSigl)
        //                .Replace("[PotNome]", LojaCertificado.PotNome)
        //                .Replace("[TipoSessao]", LojaCertificado.TiSnome)
        //                .Replace("[CidadeLoja]", LojaCertificado.CidNome + ", " + LojaCertificado.EstSigl)
        //                .Replace("[DataSessao]", LojaCertificado.SesDtHr.ToShortDateString())
        //                .Replace("[NomeVM]", gestaoAdmAtual.Count > 0 ? gestaoAdmAtual.Where(g => g.CarCodi == 1).FirstOrDefault()!.UsuNome : "")
        //                .Replace("[CargoVM]", gestaoAdmAtual.Count > 0 ? gestaoAdmAtual.Where(g => g.CarCodi == 1).FirstOrDefault()!.CarNome : "")
        //                .Replace("[NomeSecretario]", gestaoAdmAtual.Count > 0 ? gestaoAdmAtual.Where(g => g.CarCodi == 4).FirstOrDefault()!.UsuNome : "")
        //                .Replace("[CargoSecretario]", gestaoAdmAtual.Count > 0 ? gestaoAdmAtual.Where(g => g.CarCodi == 4).FirstOrDefault()!.CarNome : "")
        //                ;
        //        }

        //        foreach (var itemPresente in listaPresentes)
        //        {
        //            if (itemPresente.PreAtiv && !itemPresente.PreEmai)
        //            {
        //                if (!itemPresente.MembroLoja) //-> Se não for membro da Loja, envia e-mail, senão apenas salva a presença.
        //                {
        //                    //-> Retornando o e-mail do usuário
        //                    var emailUsuario = await _context.Usuarios
        //                        .Where(u => u.UsuCodi == itemPresente.UsuCodi).FirstOrDefaultAsync();

        //                    if (emailUsuario != null && emailUsuario.UsuEmai.Length > 0)
        //                    {
        //                        string htmlCorpoTmp = htmlCorpo
        //                             .Replace("[NomeUsuario]", itemPresente.UsuNome)
        //                             .Replace("[NomeLoja]", itemPresente.LojNome)
        //                             .Replace("[NumeroLoja]", itemPresente.LojNumL)
        //                             ;

        //                        destinatario = emailUsuario.UsuEmai;
        //                        await _emailService.EnviarEmailAsync(destinatario, assunto, htmlCorpoTmp);
        //                    }
        //                }

        //                Presenca objPresencaUpdate = new Presenca();
        //                objPresencaUpdate.UsuCodi = itemPresente.UsuCodi;
        //                objPresencaUpdate.SesCodi = itemPresente.SesCodi;
        //                objPresencaUpdate.PreAtiv = true;
        //                objPresencaUpdate.LojCodi = itemPresente.LojCodi;
        //                objPresencaUpdate.PreEmai = itemPresente.MembroLoja ? false : true;
        //                _context.Entry(objPresencaUpdate).State = EntityState.Modified;
        //                try
        //                {
        //                    await _context.SaveChangesAsync();
        //                }
        //                catch (DbUpdateConcurrencyException)
        //                {
        //                    return BadRequest("Falha ao alterar o registro");
        //                }
        //            }
        //            else if (!itemPresente.PreAtiv && itemPresente.MembroLoja)
        //            {
        //                Presenca objPresencaUpdate = new Presenca();
        //                objPresencaUpdate.UsuCodi = itemPresente.UsuCodi;
        //                objPresencaUpdate.SesCodi = itemPresente.SesCodi;
        //                objPresencaUpdate.PreAtiv = false;
        //                objPresencaUpdate.LojCodi = itemPresente.LojCodi;
        //                objPresencaUpdate.PreEmai = itemPresente.MembroLoja ? false : true;
        //                _context.Entry(objPresencaUpdate).State = EntityState.Modified;
        //                try
        //                {
        //                    await _context.SaveChangesAsync();
        //                }
        //                catch (DbUpdateConcurrencyException)
        //                {
        //                    return BadRequest("Falha ao alterar o registro");
        //                }
        //            }
        //            else if (!itemPresente.PreAtiv && itemPresente.PreEmai)
        //            {
        //                Presenca objPresencaUpdate = new Presenca();
        //                objPresencaUpdate.UsuCodi = itemPresente.UsuCodi;
        //                objPresencaUpdate.SesCodi = itemPresente.SesCodi;
        //                objPresencaUpdate.PreAtiv = itemPresente.PreAtiv;
        //                objPresencaUpdate.LojCodi = itemPresente.LojCodi;
        //                objPresencaUpdate.PreEmai = false;
        //                _context.Entry(objPresencaUpdate).State = EntityState.Modified;
        //                try
        //                {
        //                    await _context.SaveChangesAsync();
        //                }
        //                catch (DbUpdateConcurrencyException)
        //                {
        //                    return BadRequest("Falha ao alterar o registro");
        //                }
        //            }
        //        }

        //        // Commit da transação
        //        //await transaction.CommitAsync();
        //        return Ok("Presença confirmada com sucesso.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Rollback em caso de erro
        //        //await transaction.RollbackAsync();
        //        return BadRequest($"Erro ao confirmar presença: {ex.Message} \n {ex.InnerException?.Message}");
        //    }
        //}

        [NonAction]
        public string RetornaUrlCertificado(long usuCodi = 0, long sesCodi = 0)
        {
            string urlRetorno = "";

            if (usuCodi > 0 && sesCodi > 0)
            {
                string paramConcat = usuCodi.ToString() + "|" + sesCodi.ToString();
                string paramCrypto = _encryptService.Encrypt(paramConcat);
                string paramBase64 = _encryptService.ConvertToBase64(paramCrypto);
                urlRetorno = _urlAplicacaoCertificado + paramBase64;
            }

            return urlRetorno;
        }
    }
}
