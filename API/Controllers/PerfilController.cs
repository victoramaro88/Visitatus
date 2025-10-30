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

        [HttpPut("{perCodi}")]
        public async Task<IActionResult> PutPerfil(int perCodi, [FromBody] Perfil perfil)
        {
            try
            {
                if (perCodi != perfil.PerCodi)
                {
                    return BadRequest();
                }

                _context.Entry(perfil).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfilExists(perCodi))
                    {
                        return NotFound("Registro não encontrado.");
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok("Alterado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Perfil>> PostPerfil([FromBody] Perfil perfil)
        {
            bool novoRegistro = false;
            if (perfil.PerCodi == 0)
            {
                novoRegistro = true;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                perfil.PerCodi = _context.Perfils.Max(p => (int?)p.PerCodi) + 1 ?? 1;

                _context.Perfils.Add(perfil);
                var retorno = await _context.SaveChangesAsync();

                if(novoRegistro)
                {
                    var lstPermissoes = await _context.Permissaos.ToListAsync();

                    foreach (var itemPermissao in lstPermissoes)
                    {
                        PermissaoPerfil objPermissaoPerfil = new PermissaoPerfil();
                        objPermissaoPerfil.PerCodi = perfil.PerCodi;
                        objPermissaoPerfil.PemCodi = itemPermissao.PemCodi;
                        objPermissaoPerfil.PapAtvo = false;
                        objPermissaoPerfil.PepStat = true;

                        _context.PermissaoPerfils.Add(objPermissaoPerfil);
                        var retornoPermPerf = await _context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }

            await transaction.CommitAsync();
            return Ok("OK");
        }

        private bool PerfilExists(int id)
        {
            return _context.Perfils.Any(e => e.PerCodi == id);
        }
    }
}
