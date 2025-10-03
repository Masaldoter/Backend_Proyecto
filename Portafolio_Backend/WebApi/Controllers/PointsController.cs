using Microsoft.AspNetCore.Mvc;
using AccesoDatos.Models;
using AccesoDatos.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly AccesoDatosDbContext _context;
        public PointsController(AccesoDatosDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Point>>> GetPoints()
            => await _context.Points.ToListAsync(); // No incluir Project para evitar ciclos

        [HttpGet("{id}")]
        public async Task<ActionResult<Point>> GetPoint(int id)
        {
            var point = await _context.Points.FirstOrDefaultAsync(p => p.Id == id); // No incluir Project para evitar ciclos
            if (point == null) return NotFound();
            return point;
        }

        [HttpPost]
        public async Task<ActionResult<Point>> PostPoint(PointCreateDto dto)
        {
            var point = new Point
            {
                Description = dto.Description,
                ProjectId = dto.ProjectId
            };
            _context.Points.Add(point);
            await _context.SaveChangesAsync();
            // Devuelve solo los datos simples, no la entidad completa para evitar ciclos
            return CreatedAtAction(nameof(GetPoint), new { id = point.Id }, new {
                id = point.Id,
                description = point.Description,
                projectId = point.ProjectId
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoint(int id, Point point)
        {
            if (id != point.Id) return BadRequest();
            _context.Entry(point).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoint(int id)
        {
            var point = await _context.Points.FindAsync(id);
            if (point == null) return NotFound();
            _context.Points.Remove(point);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}