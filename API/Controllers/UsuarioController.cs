using API_Visitatus.Models;
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

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario(int id = 0)
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
                    return Ok(new List<Usuario> { result });
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

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuCodi == id);
        }
    }
}
