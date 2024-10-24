using API_Visitatus.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UtilController : Controller
    {
        private readonly IPasswordHasher<object> _passwordHasher;
        private readonly AppDbContext _context;

        public UtilController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<object>();
        }

        [HttpPost]
        public Task<ActionResult<UsuarioLogadoModel>> Login(LoginModel objLogin)
        {
            try
            {
                UsuarioLogadoModel result = new UsuarioLogadoModel();

                var objUsuarioLogin = _context.UsuarioLogins
                                         .Where(l => l.UsLuser == objLogin.usuario)
                                         .FirstOrDefault();

                if (objUsuarioLogin == null)
                {
                    return Task.FromResult<ActionResult<UsuarioLogadoModel>>(NotFound("Usuário não encontrado."));
                }

                string senhaCripto = CriptografarSenha(objLogin.senha!);

                var validaSenha = VerifyPassword(objUsuarioLogin.UsLpass, objLogin.senha!);

                if (validaSenha == PasswordVerificationResult.Success)
                {
                    var objUsuario = _context.Usuarios.Where(p => p.UsuCodi == objUsuarioLogin.UsuCodi).FirstOrDefault();

                    if (objUsuario == null)
                    {
                        return Task.FromResult<ActionResult<UsuarioLogadoModel>>(NotFound("Usuário não encontrada."));
                    }

                    var objUsuarioLoja = _context.UsuarioLojas.Where(ul => ul.UsuCodi == objUsuarioLogin.UsuCodi).FirstOrDefault();
                    if (objUsuarioLoja == null)
                    {
                        return Task.FromResult<ActionResult<UsuarioLogadoModel>>(NotFound("Usuário sem vínculo com nenhuma Loja."));
                    }

                    var objLoja = _context.UsuarioLojas.Where(l => l.LojCodi == objUsuarioLoja!.LojCodi).FirstOrDefault();
                    if (objLoja == null)
                    {
                        return Task.FromResult<ActionResult<UsuarioLogadoModel>>(NotFound("Loja não localizada."));
                    }

                    List<PerfilUsuarioListaModel> lstPerfil = (from pu in _context.PerfilUsuarios
                                     join p in _context.Perfils on pu.PerCodi equals p.PerCodi
                                     where pu.UsuCodi == objUsuario.UsuCodi && p.PerStat == true
                                     select new PerfilUsuarioListaModel
                                     {
                                         perCodi = p.PerCodi,
                                         perNome = p.PerNome,
                                         peUCodi = pu.PeUcodi,
                                         peUStat = pu.PeUstat
                                     }).ToList();

                    if (lstPerfil == null || lstPerfil.Count == 0)
                    {
                        return Task.FromResult<ActionResult<UsuarioLogadoModel>>(NotFound("Usuário sem perfil cadsatrado."));
                    }

                    result.usLCodi = objUsuarioLogin.UsLcodi;
                    result.usuCodi = objUsuario.UsuCodi;
                    result.usuNome = objUsuario.UsuNome;
                    result.lojCodi = objLoja.LojCodi;
                    result.lstPerfil = lstPerfil;

                    return Task.FromResult<ActionResult<UsuarioLogadoModel>>(Ok(result));
                }
                else
                {
                    return Task.FromResult<ActionResult<UsuarioLogadoModel>>(Unauthorized("Senha incorreta."));
                }
            }
            catch (Exception e)
            {
                return Task.FromResult<ActionResult<UsuarioLogadoModel>>(BadRequest(e));
            }
        }

        [NonAction]
        public PasswordVerificationResult VerifyPassword(string hashedPassword, string plainPassword)
        {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
        }

        [NonAction]
        public string CriptografarSenha(string senha)
        {
            return _passwordHasher.HashPassword(null, senha);
        }
    }
}
