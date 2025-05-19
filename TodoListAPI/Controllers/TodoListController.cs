using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TodoListAPI.DbContextData;
using TodoListAPI.Model;

namespace TodoListAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly TodoListDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public TodoListController(TodoListDbContext _context , IMemoryCache memoryCache)
        {
            this._context = _context;
            this._memoryCache = memoryCache;
        }
        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItems()
        {
            const string cacheKey = "ToDoItemList";

            // Try to get the data from the cache
            if (!_memoryCache.TryGetValue(cacheKey, out List<ToDoItem> toDoItems))
            {
                // Not in cache, so get from database
                toDoItems = await _context.ToDoItems.ToListAsync();

                // Set cache options
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1)); // Refresh if accessed

                // Save the data in cache
                _memoryCache.Set(cacheKey, toDoItems, cacheOptions);
            }

            return toDoItems;
        }

        // GET: api/ToDoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetToDoItem(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);

            if (toDoItem == null)
            {
                return NotFound();
            }

            return toDoItem;
        }

        // POST: api/ToDoItems
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostToDoItem(ToDoItem toDoItem)
        {
            _context.ToDoItems.Add(toDoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoItem", new { id = toDoItem.Id }, toDoItem);
        }

        // PUT: api/ToDoItems/5
        [HttpPut]
        public async Task<IActionResult> PutToDoItem(ToDoItem toDoItem)
        {
            _context.Entry(toDoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ToDoItems/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(toDoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
