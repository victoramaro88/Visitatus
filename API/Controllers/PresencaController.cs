using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<string>> PostConfirmaPresenca(ConsultaUsuarioLojaModel objPresenca)
        {
            try
            {
                if (objPresenca.objUsuarioLoja != null && objPresenca.objLojaConsulta != null)
                {
                    //-> Se não vier id do usuário, insere ele na tabela, vinculando-o com a Loja e atribuindo o perfil código 3 (Visitante).
                    if (objPresenca.objUsuarioLoja.UsuCodi == 0)
                    {
                        Usuario usuario = new Usuario();
                        usuario.UsuCodi = _context.Usuarios.Max(p => (int?)p.UsuCodi) + 1 ?? 1;
                        usuario.UsuNome = objPresenca.objUsuarioLoja.UsuNome!;
                        usuario.UsuNcim = objPresenca.objUsuarioLoja.UsuNCIM!;
                        usuario.UsuNasc = objPresenca.objUsuarioLoja.UsuNasc!;
                        usuario.UsuEmai = objPresenca.objUsuarioLoja.UsuEmai!;
                        usuario.UsuNcel = objPresenca.objUsuarioLoja.UsuNCel!;
                        usuario.UsuStat = true;
                        _context.Usuarios.Add(usuario);
                        var retornoUsrInsert = await _context.SaveChangesAsync();

                        if (retornoUsrInsert > 0)
                        {

                        }
                    }

                    //-> Se o código da Loja vier 0, insere a nova Loja.
                    if (objPresenca.objLojaConsulta.LojCodi == 0)
                    {
                        Loja loja = new Loja();
                        loja.LojCodi = _context.Lojas.Max(p => (int?)p.LojCodi) + 1 ?? 1;
                        loja.LojNome = objPresenca.objLojaConsulta.LojNome!;
                        loja.LojNumL = objPresenca.objLojaConsulta.LojNumL!;
                        loja.PotCodi = objPresenca.objLojaConsulta.PotCodi!;
                        loja.LojStat = true;

                        _context.Lojas.Add(loja);
                        var retornoLojaInsert = await _context.SaveChangesAsync();

                        if (retornoLojaInsert > 0)
                        {

                        }
                    }
                }
                else
                {
                    return BadRequest("Parâmetros inválidos.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }
    }
}
