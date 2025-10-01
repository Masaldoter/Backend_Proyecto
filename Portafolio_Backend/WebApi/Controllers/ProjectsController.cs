using Microsoft.AspNetCore.Mvc;
using AccesoDatos.Models;
using AccesoDatos.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AccesoDatosDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProjectsController(AccesoDatosDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
            => await _context.Projects.Include(p => p.User).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null) return NotFound();
            return project;
        }

        // Nuevo endpoint para subir imagen de proyecto y crear proyecto
        [HttpPost("with-image")]
        public async Task<ActionResult<Project>> PostProjectWithImage([FromForm] ProjectFormModel model)
        {
            string imagePath = null;
            if (model.Image != null && model.Image.Length > 0)
            {
                var imagesFolder = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(imagesFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var filePath = Path.Combine(imagesFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
                imagePath = $"images/{fileName}";
            }

            var project = new Project {
                Title = model.Title,
                Description = model.Description,
                Technologies = model.Technologies,
                ProjectUrl = model.ProjectUrl,
                UserId = model.UserId,
                ImageUrl = imagePath,
                Images = new List<ProjectImage>()
            };

            // Guardar im�genes adicionales
            if (model.Images != null)
            {
                foreach (var img in model.Images)
                {
                    if (img != null && img.Length > 0)
                    {
                        var imagesFolder = Path.Combine(_env.WebRootPath, "images");
                        Directory.CreateDirectory(imagesFolder);
                        var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                        var filePath = Path.Combine(imagesFolder, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }
                        var url = $"images/{fileName}";
                        project.Images.Add(new ProjectImage { Url = url });
                    }
                }
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.Id) return BadRequest();
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}/with-image")]
        public async Task<IActionResult> PutProjectWithImage(int id, [FromForm] ProjectFormModel model)
        {
            var project = await _context.Projects.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null) return NotFound();

            project.Title = model.Title;
            project.Description = model.Description;
            project.Technologies = model.Technologies;
            project.ProjectUrl = model.ProjectUrl;
            project.UserId = model.UserId;

            // Actualizar imagen principal
            if (model.Image != null && model.Image.Length > 0)
            {
                var imagesFolder = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(imagesFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var filePath = Path.Combine(imagesFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
                project.ImageUrl = $"images/{fileName}";
            }

            // Actualizar im�genes adicionales
            if (model.Images != null && model.Images.Count > 0)
            {
                // Elimina las im�genes anteriores
                project.Images.Clear();
                foreach (var img in model.Images)
                {
                    if (img != null && img.Length > 0)
                    {
                        var imagesFolder = Path.Combine(_env.WebRootPath, "images");
                        Directory.CreateDirectory(imagesFolder);
                        var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                        var filePath = Path.Combine(imagesFolder, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }
                        var url = $"images/{fileName}";
                        project.Images.Add(new ProjectImage { Url = url });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}