using AccesoDatos.Models;
using AccesoDatos.Data;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.DAO
{
    public class ProjectDao
    {
        private readonly AccesoDatosDbContext _context;

        public ProjectDao(AccesoDatosDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllAsync() => await _context.Projects.Include(p => p.User).ToListAsync();
        public async Task<Project?> GetByIdAsync(int id) => await _context.Projects.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
        public async Task AddAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}