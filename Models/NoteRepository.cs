using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyHelper.Entity;
using Microsoft.EntityFrameworkCore;

namespace DailyHelper.Models
{
    public class NoteRepository : IRepository<Note>
    {
        private ApplicationDbContext _context;

        public NoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Note> Items => _context.Notes;
        
        public async Task<IEnumerable<Note>> GetAsync()
        {
            return await _context.Notes.ToListAsync();
        }

        public async Task<Note> GetAsync(Guid id)
        {
            return await _context.Notes.FindAsync(id);
        }

        public async Task PutAsync(Guid id, Note item)
        {
            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (ItemExist(id)) throw;
            }
        }

        public async Task<Note> PostAsync(Note item)
        {
            _context.Notes.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task RemoveAsync(Note item)
        {
            _context.Notes.Remove(item);
            await _context.SaveChangesAsync();
        }

        public bool ItemExist(Guid id)
        {
            return _context.Notes.Any(n => n.Id == id);
        }
    }
}