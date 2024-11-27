using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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
        public ActionResult<IEnumerable<Usuario>> GetUsuarioByLoja(string usuNCIM, int potCodi, string lojNumL)
        {
            if (usuNCIM.Length > 0 && potCodi > 0 && lojNumL.Length > 0)
            {
                var result = _context.Usuarios
                    .Where(u => u.UsuNcim == "320097")
                                .Join(
                        _context.UsuarioLojas,
                        u => u.UsuCodi,
                        ul => ul.UsuCodi,
                        (u, ul) => new { Usuario = u, UsuarioLoja = ul }
                                )
                    .Join(
                        _context.Lojas,
                        combined => combined.UsuarioLoja.LojCodi,
                        l => l.LojCodi,
                        (combined, l) => new
                        {
                            combined.Usuario.UsuCodi,
                            combined.Usuario.UsuNome,
                            combined.Usuario.UsuNcim,
                            combined.Usuario.UsuNasc,
                            combined.Usuario.UsuEmai,
                            combined.Usuario.UsuNcel,
                            combined.Usuario.UsuStat,
                            LojNome = l.LojNome,
                            LojNumL = l.LojNumL,
                            PotCodi = l.PotCodi
                        }
                    )
                    .Where(result => result.PotCodi == 5 && result.LojNumL == "4024")
                    .ToList();


                return Ok(result);
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
