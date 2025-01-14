using API_Visitatus.Models;
using API_Visitatus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static API_Visitatus.Models.ConsultaUsuarioLojaModel;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUtilService _utilService;

        public UsuarioController(AppDbContext context, IUtilService utilService)
        {
            _context = context;
            _utilService = utilService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario(long id = 0)
        {
            if (id > 0)
            {
                var result = await _context.Usuarios.FindAsync(id);

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
                var result = await _context.Usuarios.ToListAsync();

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

        [HttpGet("{usuNCIM}/{potCodi}/{lojNumL}")]
        public ActionResult<IEnumerable<ConsultaUsuarioLojaModel>> GetUsuarioByLoja(string usuNCIM, int potCodi, string lojNumL)
        {
            ConsultaUsuarioLojaModel objConsultaUsuarioLoja = new ConsultaUsuarioLojaModel();
            objConsultaUsuarioLoja.objUsuarioLoja = new UsrLoja();
            objConsultaUsuarioLoja.objLojaConsulta = new LojaConsulta();

            if (usuNCIM.Length > 0 && potCodi > 0 && lojNumL.Length > 0)
            {
                var resultUsuarioLoja = _context.Usuarios
                    .Where(u => u.UsuNcim == usuNCIM)
                                .Join(
                        _context.PerfilUsuarios,
                        u => u.UsuCodi,
                        pu => pu.UsuCodi,
                        (u, pu) => new { Usuario = u, PerfilUsuario = pu }
                                )
                    .Join(
                        _context.Lojas,
                        combined => combined.PerfilUsuario.LojCodi,
                        l => l.LojCodi,
                        (combined, l) => new UsrLoja
                        {
                            UsuCodi = combined.Usuario.UsuCodi,
                            UsuNome = combined.Usuario.UsuNome,
                            UsuNCIM = combined.Usuario.UsuNcim,
                            UsuNasc = combined.Usuario.UsuNasc,
                            UsuEmai = combined.Usuario.UsuEmai,
                            UsuNCel = combined.Usuario.UsuNcel,
                            UsuStat = combined.Usuario.UsuStat,
                            LojNome = l.LojNome,
                            LojNumL = l.LojNumL,
                            PotCodi = l.PotCodi
                        }
                    )
                    .Where(result => result.PotCodi == potCodi && result.LojNumL == lojNumL)
                    .FirstOrDefault();

                objConsultaUsuarioLoja.objUsuarioLoja = resultUsuarioLoja;


                var resultLoja = _context.Lojas
                    .Where(l => l.LojNumL == lojNumL && l.PotCodi == potCodi)
                    .Select(l => new LojaConsulta
                    {
                        LojCodi = l.LojCodi,
                        LojNome = l.LojNome,
                        LojNumL = l.LojNumL,
                        LojLogo = l.LojLogo,
                        LojLogr = l.LojLogr,
                        LojNume = l.LojNume,
                        LojBair = l.LojBair,
                        LojStat = l.LojStat,
                        CidCodi = (long)l.CidCodi!,
                        PotCodi = l.PotCodi,
                        RitCodi = (int)l.RitCodi!
                    })
                    .FirstOrDefault();

                objConsultaUsuarioLoja.objLojaConsulta = resultLoja;

                return Ok(objConsultaUsuarioLoja);
            }
            else
            {
                return BadRequest("Parâmetros inválidos.");
            }
        }

        [HttpGet("{lojCodi}")]
        public async Task<ActionResult<IEnumerable<ConsultaUsuarioLojaModel>>> GetUsuarioByLojCodi(long lojCodi)
        {
            List<UsuarioLojaModel> lstConsultaUsuarioLoja = new List<UsuarioLojaModel>();

            if (lojCodi > 0)
            {
                lstConsultaUsuarioLoja = await (from pu in _context.PerfilUsuarios
                                                join p in _context.Perfils on pu.PerCodi equals p.PerCodi
                                                join l in _context.Lojas on pu.LojCodi equals l.LojCodi
                                                join u in _context.Usuarios on pu.UsuCodi equals u.UsuCodi
                                                where l.LojCodi == lojCodi
                                                orderby u.UsuNome
                                                select new UsuarioLojaModel
                                                {
                                                    UsuCodi = u.UsuCodi,
                                                    UsuNome = u.UsuNome,
                                                    UsuNCIM = u.UsuNcim,
                                                    UsuNCel = u.UsuNcel,
                                                    UsuEmai = u.UsuEmai,
                                                    UsuNasc = u.UsuNasc,
                                                    UsuStat = u.UsuStat,
                                                    PerCodi = p.PerCodi,
                                                    PerNome = p.PerNome,
                                                    PeUStat = pu.PeUstat
                                                }).ToListAsync();


                return Ok(lstConsultaUsuarioLoja);
            }
            else
            {
                return BadRequest("Parâmetros inválidos.");
            }
        }

        [HttpGet("{potCodi}/{usuNCIM}")]
        public async Task<ActionResult<IEnumerable<UsuarioPotenciaModel>>> GetUsuarioByPotCodi(long potCodi, string usuNCIM)
        {
            UsuarioPotenciaModel objUsrPot = new UsuarioPotenciaModel();
            objUsrPot.lstLjUsrPot = new List<LojaUsuarioPotenciaModel>();

            if (potCodi > 0 && usuNCIM.Length > 0)
            {
                var lstConsultaUsuarioLoja = await (from usr in _context.Usuarios
                                                    join perUsu in _context.PerfilUsuarios on usr.UsuCodi equals perUsu.UsuCodi
                                                    join loj in _context.Lojas on perUsu.LojCodi equals loj.LojCodi
                                                    join perf in _context.Perfils on perUsu.PerCodi equals perf.PerCodi
                                                    join login in _context.UsuarioLogins on usr.UsuCodi equals login.UsuCodi into loginGroup
                                                    from login in loginGroup.DefaultIfEmpty() // Left join aqui
                                                    where usr.UsuNcim == usuNCIM && loj.PotCodi == potCodi
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
                                                        perf.PerStat,
                                                        UsLuser = login != null ? login.UsLuser : "",
                                                        UsLpass = login != null ? login.UsLpass : "",
                                                        UsLstat = login != null ? login.UsLstat : false
                                                    }).ToListAsync();

                if (lstConsultaUsuarioLoja.Count > 0)
                {
                    objUsrPot.UsuCodi = lstConsultaUsuarioLoja[0].UsuCodi;
                    objUsrPot.UsuNome = lstConsultaUsuarioLoja[0].UsuNome;
                    objUsrPot.UsuNCIM = lstConsultaUsuarioLoja[0].UsuNcim;
                    objUsrPot.UsuNasc = lstConsultaUsuarioLoja[0].UsuNasc;
                    objUsrPot.UsuEmai = lstConsultaUsuarioLoja[0].UsuEmai;
                    objUsrPot.UsuNCel = lstConsultaUsuarioLoja[0].UsuNcel;
                    objUsrPot.UsuStat = lstConsultaUsuarioLoja[0].UsuStat;
                    objUsrPot.UsLUser = lstConsultaUsuarioLoja[0].UsLuser;
                    objUsrPot.UsLPass = lstConsultaUsuarioLoja[0].UsLpass;
                    objUsrPot.UsLStat = lstConsultaUsuarioLoja[0].UsLstat;

                    foreach (var itemUsr in lstConsultaUsuarioLoja)
                    {
                        LojaUsuarioPotenciaModel objUsrLjPot = new LojaUsuarioPotenciaModel();
                        objUsrLjPot.LojCodi = itemUsr.LojCodi;
                        objUsrLjPot.LojNome = itemUsr.LojNome;
                        objUsrLjPot.LojNumL = itemUsr.LojNumL;
                        objUsrLjPot.PotCodi = itemUsr.PotCodi;
                        objUsrLjPot.PerCodi = itemUsr.PerCodi;
                        objUsrLjPot.PerNome = itemUsr.PerNome;
                        objUsrLjPot.PerStat = itemUsr.PerStat;

                        objUsrPot.lstLjUsrPot.Add(objUsrLjPot);
                    }
                }

                return Ok(objUsrPot);
            }
            else
            {
                return BadRequest("Parâmetros inválidos.");
            }
        }

        [HttpPut("{usuCodi}")]
        public async Task<IActionResult> PutUsuario(int usuCodi, Usuario usuario)
        {
            if (usuCodi != usuario.UsuCodi)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuCodi))
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

        [HttpPut("{usuCodi}")]
        public async Task<ActionResult<UsuarioPotenciaModel>> PutUsuarioCompleto(long usuCodi, [FromBody] UsuarioPotenciaModel usuarioCompleto)
        {
            if (usuCodi != usuarioCompleto.UsuCodi)
            {
                return BadRequest();
            }

            using var transaction = await _context.Database.BeginTransactionAsync(); // Inicia a transação
            try
            {
                //-> INSERINDO O USUÁRIO
                Usuario usuario = new Usuario
                {
                    UsuCodi = usuarioCompleto.UsuCodi,
                    UsuNome = usuarioCompleto.UsuNome!,
                    UsuNcim = usuarioCompleto.UsuNCIM!,
                    UsuNasc = usuarioCompleto.UsuNasc,
                    UsuEmai = usuarioCompleto.UsuEmai!,
                    UsuNcel = usuarioCompleto.UsuNCel!,
                    UsuStat = usuarioCompleto.UsuStat
                };

                _context.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                //-> APAGA TODOS OS PERFIS RELACIONADO AO USUÁRIO, PARA INSERIR NOVAMENTE ATUALIZADO
                // Obter os registros que atendem à condição
                var usuariosParaExcluir = _context.PerfilUsuarios.Where(p => p.UsuCodi == usuCodi && p.LojCodi != 0).ToList();
                // Remover os registros
                _context.PerfilUsuarios.RemoveRange(usuariosParaExcluir);
                await _context.SaveChangesAsync();

                //-> INSERINDO OS PERFIS DO USUÁRIO
                foreach (var item in usuarioCompleto.lstLjUsrPot!)
                {
                    PerfilUsuario perfilUsuario = new PerfilUsuario
                    {
                        PeUcodi = _context.PerfilUsuarios.Max(p => (int?)p.PeUcodi) + 1 ?? 1,
                        PeUstat = true,
                        PerCodi = item.PerCodi,
                        UsuCodi = usuario.UsuCodi,
                        LojCodi = item.LojCodi
                    };

                    _context.PerfilUsuarios.Add(perfilUsuario);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync(); // Confirma a transação

                return Ok(usuarioCompleto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Reverte a transação em caso de erro
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                usuario.UsuCodi = _context.Usuarios.Max(p => (int?)p.UsuCodi) + 1 ?? 1;

                _context.Usuarios.Add(usuario);
                var retorno = await _context.SaveChangesAsync();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioPotenciaModel>> PostUsuarioCompleto([FromBody] UsuarioPotenciaModel usuarioCompleto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(); // Inicia a transação
            try
            {
                //-> INSERINDO O USUÁRIO
                Usuario usuario = new Usuario
                {
                    UsuCodi = _context.Usuarios.Max(p => (int?)p.UsuCodi) + 1 ?? 1,
                    UsuNome = usuarioCompleto.UsuNome!,
                    UsuNcim = usuarioCompleto.UsuNCIM!,
                    UsuNasc = usuarioCompleto.UsuNasc,
                    UsuEmai = usuarioCompleto.UsuEmai!,
                    UsuNcel = usuarioCompleto.UsuNCel!,
                    UsuStat = usuarioCompleto.UsuStat
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                //-> INSERINDO O LOGIN, CASO VENHA CADASTRADO
                if(usuarioCompleto.UsLUser?.Length > 0 && usuarioCompleto.UsLPass?.Length > 0) {
                    UsuarioLogin usrLogin = new UsuarioLogin
                    {
                        UsLcodi = _context.UsuarioLogins.Max(p => (long?)p.UsLcodi) + 1 ?? 1,
                        UsLuser = usuarioCompleto.UsLUser,
                        UsLpass = _utilService.CriptografarSenha(usuarioCompleto.UsLPass),
                        UsLstat = true,
                        UsuCodi = usuario.UsuCodi
                    };
                    _context.UsuarioLogins.Add(usrLogin);
                    await _context.SaveChangesAsync();
                }

                //-> INSERINDO OS PERFIS DO USUÁRIO
                foreach (var item in usuarioCompleto.lstLjUsrPot!)
                {
                    PerfilUsuario perfilUsuario = new PerfilUsuario
                    {
                        PeUcodi = _context.PerfilUsuarios.Max(p => (int?)p.PeUcodi) + 1 ?? 1,
                        PeUstat = true,
                        PerCodi = item.PerCodi,
                        UsuCodi = usuario.UsuCodi,
                        LojCodi = item.LojCodi
                    };

                    _context.PerfilUsuarios.Add(perfilUsuario);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync(); // Confirma a transação

                return Ok(usuarioCompleto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Reverte a transação em caso de erro
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuCodi == id);
        }
    }
}
