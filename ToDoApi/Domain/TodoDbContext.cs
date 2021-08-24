using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Domain
{
    public class TodoDbContext : DbContext, ITodoDbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<TodoTag> TodoTags { get; set; } = null!;

        public TodoDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
