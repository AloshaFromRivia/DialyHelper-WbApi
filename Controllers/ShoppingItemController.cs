using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DailyHelper.Entity;
using DailyHelper.Models;
using DailyHelper.Models.ViewModels.Requests;

namespace DailyHelper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShoppingItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopItem>>> GetShopItems([FromQuery]Guid listId)
        {
            return await _context.ShopItems
                .Where(i=>i.ListId==listId)
                .ToListAsync();
        }

        // GET: api/ShoppingItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShopItem>> GetShopItem(Guid id,[FromQuery]Guid listId)
        {
            var shopItem = await _context.ShopItems
                .Where(i=>i.ListId==listId)
                .FirstOrDefaultAsync(i=>i.Id==id);

            if (shopItem == null)
            {
                return NotFound();
            }

            return shopItem;
        }

        // PUT: api/ShoppingItem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShopItem(Guid id, ShopItem shopItem)
        {
            if (id != shopItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(shopItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopItemExists(id))
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

        // POST: api/ShoppingItem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShopItem>> PostShopItem(ShopItemRequest shopItem,[FromQuery]Guid listId)
        {
            
            var item = new ShopItem()
            {
                Id = Guid.NewGuid(),
                ListId = listId,
                Completed = false,
                Count = shopItem.Count,
                Name = shopItem.Name
            };

            _context.ShopItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShopItem", new { id = item.Id }, item);
        }

        // DELETE: api/ShoppingItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShopItem(Guid id)
        {
            var shopItem = await _context.ShopItems.FindAsync(id);
            if (shopItem == null)
            {
                return NotFound();
            }

            _context.ShopItems.Remove(shopItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShopItemExists(Guid id)
        {
            return _context.ShopItems.Any(e => e.Id == id);
        }
    }
}
