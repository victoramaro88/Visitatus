using API_Visitatus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Visitatus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GrauController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GrauController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Grau>>> GetGrau(int id = 0)
        {
            if (id > 0)
            {
                var result = await _context.Graus.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new List<Grau> { result });
                }
            }
            else
            {
                var result = await _context.Graus.ToListAsync();

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

        [HttpPut("{graCodi}")]
        public async Task<IActionResult> PutGrau(int graCodi, Grau grau)
        {
            if (graCodi != grau.GraCodi)
            {
                return BadRequest();
            }

            _context.Entry(grau).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrauExists(graCodi))
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
        public async Task<ActionResult<Grau>> PostGrau(Grau grau)
        {
            try
            {
                grau.GraCodi = _context.Graus.Max(p => (int?)p.GraCodi) + 1 ?? 1;

                _context.Graus.Add(grau);
                var retorno = await _context.SaveChangesAsync();

                return Ok(grau);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " \n " + ex.InnerException?.Message);
            }
        }

        private bool GrauExists(int id)
        {
            return _context.Graus.Any(e => e.GraCodi == id);
        }
    }
}
