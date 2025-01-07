using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PerfilController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PerfilController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Perfil>>> GetPerfil(int id = 0)
        {
            if (id > 0)
            {
                var result = await _context.Perfils.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<Perfil> { result });
                }
            }
            else
            {
                var result = await _context.Perfils.ToListAsync();

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

        [HttpGet("{ids}")]
        public async Task<ActionResult<IEnumerable<Perfil>>> GetPerfilByPerCodi(string ids)
        {
            if (ids.Length == 0)
            {
                return BadRequest("Parâmetros Inválidos.");
            }

            try
            {
                if (string.IsNullOrWhiteSpace(ids))
                    return BadRequest("IDs inválidos.");

                // Separar a string por vírgulas e converter para lista de inteiros
                var idList = ids.Split(',')
                                .Where(x => int.TryParse(x, out _)) // Validar se é um número
                                .Select(int.Parse)
                                .ToList();

                if (!idList.Any())
                    return BadRequest("Nenhum ID válido foi fornecido.");

                var perfis = await _context.Perfils
                    .Where(p => idList.Contains(p.PerCodi))
                    .OrderBy(p => p.PerNome)
                    .ToListAsync();

                return Ok(perfis);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool RitoExists(int id)
        {
            return _context.Ritos.Any(e => e.RitCodi == id);
        }
    }
}
