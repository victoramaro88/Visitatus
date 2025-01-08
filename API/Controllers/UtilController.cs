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
        private readonly EncryptionService _encryptionService;

        public UtilController(AppDbContext context, EncryptionService encryptionService)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<object>();
            _encryptionService = encryptionService;
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

                if (validaSenha != PasswordVerificationResult.Success)
                {
                    return Task.FromResult<ActionResult<UsuarioLogadoModel>>(Unauthorized("Senha incorreta."));
                }

                var objUsuario = _context.Usuarios.Where(p => p.UsuCodi == objUsuarioLogin.UsuCodi).FirstOrDefault();

                if (objUsuario == null)
                {
                    return Task.FromResult<ActionResult<UsuarioLogadoModel>>(NotFound("Usuário não encontrada."));
                }

                List<PerfilUsuarioListaModel> lstPerfil = _context.PerfilUsuarios
                    .Join(_context.Usuarios,
                          pu => pu.UsuCodi,
                          u => u.UsuCodi,
                          (pu, u) => new { pu, u })
                    .Join(_context.Lojas,
                          pu_u => pu_u.pu.LojCodi,
                          l => l.LojCodi,
                          (pu_u, l) => new { pu_u.pu, pu_u.u, l })
                    .Join(_context.Perfils,
                          pu_u_l => pu_u_l.pu.PerCodi,
                          p => p.PerCodi,
                          (pu_u_l, p) => new
                          {
                              pu_u_l.pu.PeUcodi,
                              pu_u_l.pu.PeUstat,
                              pu_u_l.pu.PerCodi,
                              pu_u_l.pu.UsuCodi,
                              pu_u_l.pu.LojCodi,
                              p.PerNome,
                              p.PerStat,
                              pu_u_l.l.LojNome,
                              pu_u_l.l.LojNumL,
                              pu_u_l.l.LojStat,
                              pu_u_l.l.PotCodi
                          })
                    .Where(joined => joined.UsuCodi == 1)
                    .Select(result => new PerfilUsuarioListaModel
                    {
                        peUCodi = result.PeUcodi,
                        peUStat = result.PeUstat,
                        perCodi = result.PerCodi,
                        usuCodi = result.UsuCodi,
                        lojCodi = result.LojCodi,
                        perNome = result.PerNome,
                        perStat = result.PerStat,
                        lojNome = result.LojNome,
                        lojNumL = result.LojNumL,
                        lojStat = result.LojStat,
                        potCodi = result.PotCodi
                    })
                    .ToList();

                if (lstPerfil == null || lstPerfil.Count == 0)
                {
                    return Task.FromResult<ActionResult<UsuarioLogadoModel>>(NotFound("Usuário sem perfil cadsatrado."));
                }

                result.usLCodi = objUsuarioLogin.UsLcodi;
                result.usuCodi = objUsuario.UsuCodi;
                result.usuNome = objUsuario.UsuNome;
                result.lstPerfil = lstPerfil;

                return Task.FromResult<ActionResult<UsuarioLogadoModel>>(Ok(result));

            }
            catch (Exception e)
            {
                return Task.FromResult<ActionResult<UsuarioLogadoModel>>(BadRequest(e));
            }
        }

        [HttpPost]
        public IActionResult Encrypt([FromBody] EncryptAPIModel objMensagem)
        {
            if (objMensagem == null && objMensagem?.valorMensagem?.Length == 0)
            {
                return BadRequest("Parâmetros Inválidos.");
            }

            var encrypted = _encryptionService.Encrypt(objMensagem!.valorMensagem!);
            return Ok(encrypted);
        }

        [HttpPost]
        public IActionResult Decrypt([FromBody] EncryptAPIModel objMensagem)
        {
            var decrypted = _encryptionService.Decrypt(objMensagem.valorMensagem!);
            return Ok(decrypted);
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
