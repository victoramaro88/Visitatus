using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class EstadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstadoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Estado>>> GetEstados(int id)
        {
            if (id > 0)
            {
                var result = await _context.Estados.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<Estado> { result });
                }
            }
            else
            {
                var result = await _context.Estados.ToListAsync();

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
    }
}
