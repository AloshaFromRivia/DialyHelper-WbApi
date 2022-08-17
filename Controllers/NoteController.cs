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
        private readonly IRepository<Note> _repository;

        public NoteController(IRepository<Note> repository)
        {
            _repository = repository;
        }
        
        // GET: api/NoteController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            return await _repository.Items
                .Where(n => n.UserId == HttpContext.GetUserId())
                .ToListAsync();
        }
        
        // GET: api/NoteController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(Guid id)
        {
            var note = await _repository.Items
                .Where(n=>n.UserId==HttpContext.GetUserId())
                .FirstOrDefaultAsync(n=>n.Id==id);

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
            
            await _repository.PostAsync(note);

            return CreatedAtAction("GetNote", new { id = note.Id }, note);
        }

        // DELETE: api/NoteController/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var note = await _repository.GetAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            _repository.RemoveAsync(note);

            return NoContent();
        }
    }
}
