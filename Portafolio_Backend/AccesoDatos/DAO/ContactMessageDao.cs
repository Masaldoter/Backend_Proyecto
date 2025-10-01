using AccesoDatos.Models;
using AccesoDatos.Data;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.DAO
{
    public class ContactMessageDao
    {
        private readonly AccesoDatosDbContext _context;

        public ContactMessageDao(AccesoDatosDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContactMessage>> GetAllAsync() => await _context.ContactMessages.ToListAsync();
        public async Task<ContactMessage?> GetByIdAsync(int id) => await _context.ContactMessages.FindAsync(id);
        public async Task AddAsync(ContactMessage message)
        {
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                _context.ContactMessages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }
    }
}