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
    public class TodoController : ControllerBase
    {
        private readonly IRepository<ToDoTask> _repository;

        public TodoController(IRepository<ToDoTask> repository)
        {
            _repository = repository;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetTasks()
        {
            return await _repository.Items
                .Where(t=>t.UserId==HttpContext.GetUserId())
                .ToListAsync();
        }
        
        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetToDoTask(Guid id)
        {
            var toDoTask = await _repository.Items
                .Where(t=>t.UserId==HttpContext.GetUserId())
                .FirstOrDefaultAsync(t=>t.Id==id);

            if (toDoTask == null)
            {
                return NotFound();
            }

            return toDoTask;
        }

        // PUT: api/Todo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask(Guid id, ToDoTask toDoTask)
        {
            var userOwnsNote = toDoTask.UserId == HttpContext.GetUserId();

            if (!userOwnsNote)
            {
                return Unauthorized(new []{"You do not own this post"});
            }
            
            if (id != toDoTask.Id)
            {
                return BadRequest();
            }

            _repository.PutAsync(id, toDoTask);

            return NoContent();
        }

        // POST: api/Todo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDoTask>> PostToDoTask(TodoTaskRequest toDoTaskRequest)
        {
            var task = new ToDoTask()
            {
                Id = Guid.NewGuid(),
                Title = toDoTaskRequest.Title,
                CreationDate = DateTime.Now,
                DueDate = toDoTaskRequest.DueDate,
                Description = toDoTaskRequest.Description,
                Completed = false,
                UserId = HttpContext.GetUserId()
            };

            _repository.PostAsync(task);

            return CreatedAtAction("GetToDoTask", new { id = task.Id }, task);
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(Guid id)
        {
            var toDoTask = await _repository.GetAsync(id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            _repository.RemoveAsync(toDoTask);

            return NoContent();
        }
    }
}
