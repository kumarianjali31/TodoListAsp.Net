using Microsoft.EntityFrameworkCore;
using TodoListAPI.Model;

namespace TodoListAPI.DbContextData
{
    public class TodoListDbContext : DbContext
    {
        public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
        {
            
        }
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
