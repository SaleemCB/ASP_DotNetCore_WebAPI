using ConceptZeeWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConceptZeeWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CZeeContactsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpPost]
        public async Task<IActionResult> PostContact([FromBody] CZeeContact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return NotFound();
            return Ok(contact);
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await _context.Contacts.ToListAsync());
        }
    }

}
