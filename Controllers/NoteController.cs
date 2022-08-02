using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DailyHelper.Entity;
using DailyHelper.Extentions;
using DailyHelper.Models;
using DailyHelper.Models.ViewModels.Requests;
using Microsoft.AspNetCore.Authorization;

namespace DailyHelper.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NoteController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: api/NoteController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            return await _context.Notes
                .Where(n=>n.UserId==HttpContext.GetUserId())
                .ToListAsync();
        }
        
        // GET: api/NoteController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(Guid id)
        {
            var note = await _context.Notes
                .Where(n=>n.UserId==HttpContext.GetUserId())
                .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        // PUT: api/NoteController/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(Guid id, Note note)
        {
            var userOwnsNote = note.UserId == HttpContext.GetUserId();

            if (!userOwnsNote)
            {
                return Unauthorized(new []{"You do not own this post"});
            }
            
            if (id != note.Id)
            {
                return BadRequest();
            }

            _context.Entry(note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/NoteController
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(NoteRequest noteRequest)
        {
            var note = new Note()
            {
                Id = Guid.NewGuid(),
                Title = noteRequest.Title,
                Description = noteRequest.Description,
                UserId = HttpContext.GetUserId()
            };
            
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.Id }, note);
        }

        // DELETE: api/NoteController/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NoteExists(Guid id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
        
    }
}
