using Microsoft.AspNetCore.Mvc;
using AccesoDatos.Models;
using AccesoDatos.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactMessagesController : ControllerBase
    {
        private readonly AccesoDatosDbContext _context;
        public ContactMessagesController(AccesoDatosDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactMessage>>> GetContactMessages()
            => await _context.ContactMessages.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactMessage>> GetContactMessage(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null) return NotFound();
            return message;
        }

        [HttpPost]
        public async Task<ActionResult<ContactMessage>> PostContactMessage(ContactMessage message)
        {
            message.SentAt = DateTime.UtcNow;
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContactMessage), new { id = message.Id }, message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactMessage(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null) return NotFound();
            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}