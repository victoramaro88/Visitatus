using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PresencaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PresencaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{SesCodi}")]
        public async Task<ActionResult<IEnumerable<ListaPresencaModel>>> GetListaPresencaBySesCodi(long SesCodi = 0)
        {
            if (SesCodi == 0)
            {
                return BadRequest("Parâmetros Inválidos.");
            }

            List<ListaPresencaModel> listaRetorno = await (from pre in _context.Presencas
                                                           join usu in _context.Usuarios on pre.UsuCodi equals usu.UsuCodi
                                                           join loj in _context.Lojas on pre.LojCodi equals loj.LojCodi
                                                           join ses in _context.Sessaos on pre.SesCodi equals ses.SesCodi
                                                           where pre.SesCodi == SesCodi
                                                           select new ListaPresencaModel
                                                           {
                                                               SesCodi = pre.SesCodi,
                                                               UsuCodi = usu.UsuCodi,
                                                               UsuNome = usu.UsuNome,
                                                               UsuNCIM = usu.UsuNcim,
                                                               LojNome = loj.LojNome,
                                                               LojNumL = loj.LojNumL,
                                                               PreAtiv = pre.PreAtiv
                                                           }).ToListAsync();

            return Ok(listaRetorno);
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostConfirmaPresenca([FromBody] ConsultaUsuarioLojaModel objPresenca)
        {
            long novoUsuarioId = 0;
            long novoLojaId = 0;
            if (objPresenca.objUsuarioLoja == null || objPresenca.objLojaConsulta == null || objPresenca.sesCodi == 0)
            {
                return BadRequest("Parâmetros inválidos.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
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

                //-> Se não vier id do usuário, verifica se já existe, senão, insere ele na tabela, vinculando-o com a Loja e atribuindo o perfil código 4 (Membro).
                if (objPresenca.objUsuarioLoja.UsuCodi == 0)
                {
                    //-> Faz uma consulta, para saber se o usuário já existe na base, mas em outra Loja
                    var usuarioOutraLoja = await _context.Usuarios
                        .Where(u => u.UsuNome.ToUpper() == objPresenca.objUsuarioLoja.UsuNome!.ToUpper()
                            && u.UsuNcim.ToUpper() == objPresenca.objUsuarioLoja.UsuNCIM!.ToUpper())
                        .FirstOrDefaultAsync();

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
                            PreAtiv = false
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
                        PreAtiv = false
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
    }
}
