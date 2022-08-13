using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyHelper.Entity;
using Microsoft.EntityFrameworkCore;

namespace DailyHelper.Models
{
    public class TodoRepository : IRepository<ToDoTask>
    {
        private ApplicationDbContext _context;

        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<ToDoTask> Items => _context.Tasks;

        public async Task<IEnumerable<ToDoTask>> GetAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<ToDoTask> GetAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task PutAsync(Guid id, ToDoTask item)
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

        public async Task<ToDoTask> PostAsync(ToDoTask item)
        {
            _context.Tasks.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task RemoveAsync(ToDoTask item)
        {
            _context.Tasks.Remove(item);
            await _context.SaveChangesAsync();
        }

        public bool ItemExist(Guid id)
        {
            return _context.Tasks.Any(n => n.Id == id);
        }
    }
}