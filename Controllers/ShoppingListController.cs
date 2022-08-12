using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ShoppingListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShoppingListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingList>>> GetShoppingLists()
        {
            return await _context.ShoppingLists
                .Where(s=>s.UserId==HttpContext.GetUserId())
                .ToListAsync();
        }

        // GET: api/ShoppingList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingList>> GetShoppingList(Guid id)
        {
            var shoppingList = await _context.ShoppingLists
                .Where(s => s.UserId == HttpContext.GetUserId())
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shoppingList == null)
            {
                return NotFound();
            }

            return shoppingList;
        }

        // PUT: api/ShoppingList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingList(Guid id, ShoppingList shoppingList)
        {
            var userOwnsNote = shoppingList.UserId == HttpContext.GetUserId();

            if (!userOwnsNote)
            {
                return Unauthorized(new []{"You do not own this shopping list"});
            }
            
            if (id != shoppingList.Id)
            {
                return BadRequest();
            }

            _context.Entry(shoppingList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingListExists(id))
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

        // POST: api/ShoppingList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShoppingList>> PostShoppingList(ShoppingListRequest shoppingList)
        {
            var sl = new ShoppingList()
            {
                Id = Guid.NewGuid(),
                Title = shoppingList.Title,
                Completed = false,
                UserId = HttpContext.GetUserId()
            };
            
            _context.ShoppingLists.Add(sl);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingList", new { id = sl.Id }, sl);
        }

        // DELETE: api/ShoppingList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingList(Guid id)
        {
            var shoppingList = await _context.ShoppingLists.FindAsync(id);
            if (shoppingList == null)
            {
                return NotFound();
            }

            _context.ShoppingLists.Remove(shoppingList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShoppingListExists(Guid id)
        {
            return _context.ShoppingLists.Any(e => e.Id == id);
        }
    }
}
