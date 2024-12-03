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

        [HttpPost]
        public async Task<ActionResult<string>> PostConfirmaPresenca([FromBody] ConsultaUsuarioLojaModel objPresenca)
        {
            long novoUsuarioId = 0;
            if (objPresenca.objUsuarioLoja == null || objPresenca.objLojaConsulta == null || objPresenca.sesCodi == 0)
            {
                return BadRequest("Parâmetros inválidos.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //-> Se não vier id do usuário, insere ele na tabela, vinculando-o com a Loja e atribuindo o perfil código 3 (Visitante).
                if (objPresenca.objUsuarioLoja.UsuCodi == 0)
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

                    PerfilUsuario perfilUsuario = new PerfilUsuario
                    {
                        PeUcodi = _context.PerfilUsuarios.Max(p => (int?)p.PeUcodi) + 1 ?? 1,
                        PeUstat = true,
                        PerCodi = 3, //-> Perfil selecionado como Visitante.
                        UsuCodi = usuario.UsuCodi
                    };

                    _context.PerfilUsuarios.Add(perfilUsuario);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //-> Verificando se o usuário já confirmou sua presença para esta sessão
                    var confirmacaoPresenca = await _context.Presencas
                        .Where(p => p.UsuCodi == objPresenca.objUsuarioLoja.UsuCodi && p.SesCodi == objPresenca.sesCodi)
                        .FirstOrDefaultAsync();

                    if(confirmacaoPresenca != null)
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
                }

                //-> Agora insere na tabela de presença.
                Presenca presenca = new Presenca
                {
                    UsuCodi = objPresenca.objUsuarioLoja.UsuCodi > 0 ? objPresenca.objUsuarioLoja.UsuCodi : novoUsuarioId,
                    SesCodi = objPresenca.sesCodi
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
        }


        //[HttpPost]
        //public async Task<ActionResult<string>> PostConfirmaPresenca(ConsultaUsuarioLojaModel objPresenca)
        //{
        //    try
        //    {
        //        if (objPresenca.objUsuarioLoja != null && objPresenca.objLojaConsulta != null)
        //        {
        //            //-> Se não vier id do usuário, insere ele na tabela, vinculando-o com a Loja e atribuindo o perfil código 3 (Visitante).
        //            if (objPresenca.objUsuarioLoja.UsuCodi == 0)
        //            {
        //                Usuario usuario = new Usuario();
        //                usuario.UsuCodi = _context.Usuarios.Max(p => (int?)p.UsuCodi) + 1 ?? 1;
        //                usuario.UsuNome = objPresenca.objUsuarioLoja.UsuNome!;
        //                usuario.UsuNcim = objPresenca.objUsuarioLoja.UsuNCIM!;
        //                usuario.UsuNasc = objPresenca.objUsuarioLoja.UsuNasc!;
        //                usuario.UsuEmai = objPresenca.objUsuarioLoja.UsuEmai!;
        //                usuario.UsuNcel = objPresenca.objUsuarioLoja.UsuNCel!;
        //                usuario.UsuStat = true;
        //                _context.Usuarios.Add(usuario);
        //                var retornoUsrInsert = await _context.SaveChangesAsync();

        //                if (retornoUsrInsert > 0)
        //                {
        //                    //-> Já adiciono o usuário como perfil de visitante.
        //                    PerfilUsuario perfilUsuario = new PerfilUsuario();
        //                    perfilUsuario.PeUcodi = _context.PerfilUsuarios.Max(p => (int?)p.PeUcodi) + 1 ?? 1;
        //                    perfilUsuario.PeUstat = true;
        //                    perfilUsuario.PerCodi = 3; //-> Perfil selecionado como Visitante.

        //                    _context.PerfilUsuarios.Add(perfilUsuario);
        //                    var retorno = await _context.SaveChangesAsync();
        //                }
        //            }

        //            //-> Se o código da Loja vier 0, insere a nova Loja.
        //            if (objPresenca.objLojaConsulta.LojCodi == 0)
        //            {
        //                Loja loja = new Loja();
        //                loja.LojCodi = _context.Lojas.Max(p => (int?)p.LojCodi) + 1 ?? 1;
        //                loja.LojNome = objPresenca.objLojaConsulta.LojNome!;
        //                loja.LojNumL = objPresenca.objLojaConsulta.LojNumL!;
        //                loja.PotCodi = objPresenca.objLojaConsulta.PotCodi!;
        //                loja.LojStat = true;

        //                _context.Lojas.Add(loja);
        //                var retornoLojaInsert = await _context.SaveChangesAsync();

        //                if (retornoLojaInsert > 0)
        //                {

        //                }
        //            }

        //            //-> Agora insere na tabela de presença.
        //            Presenca presenca = new Presenca();
        //            presenca.UsuCodi = objPresenca.objUsuarioLoja.UsuCodi;
        //            presenca.SesCodi = objPresenca.sesCodi;

        //            _context.Presencas.Add(presenca);
        //            var retornoPresencaInsert = await _context.SaveChangesAsync();

        //            if (retornoPresencaInsert > 0)
        //            {

        //            }
        //        }
        //        else
        //        {
        //            return BadRequest("Parâmetros inválidos.");
        //        }

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
        //    }
        //}
    }
}
