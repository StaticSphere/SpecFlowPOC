using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Domain;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoDbContext _dbContext;

        public TodoItemService(ITodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(bool includeCompleted = false)
        {
            return await _dbContext.TodoItems
                .Include(x => x.Tags)
                .Where(x => includeCompleted || !x.Completed)
                .ToListAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsByTagAsync(IEnumerable<string> tags, bool includeCompleted = false)
        {
            if (tags is null)
                return new List<TodoItem>();

            return await _dbContext.TodoItems
                .Include(x => x.Tags)
                .Where(x => includeCompleted || !x.Completed)
                .Where(x => x.Tags.Any(t => tags.Contains(t.Title)))
                .ToListAsync();
        }

        public async Task<TodoItem> GetTodoItemAsync(int id)
        {
            return await _dbContext.TodoItems
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> AddTodoItemAsync(TodoItem item)
        {
            _dbContext.TodoItems.Add(item);
            await _dbContext.SaveChangesAsync();

            return item.Id!.Value;
        }

        public async Task<int> UpdateTodoItemAsync(TodoItem item)
        {
            _dbContext.TodoItems.Attach(item);
            await _dbContext.SaveChangesAsync();

            return item.Id!.Value;
        }

        public async Task DeleteTodoItemAsync(int id)
        {
            var todoItem = await _dbContext.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
            if (todoItem is null)
                return;

            _dbContext.TodoItems.Remove(todoItem);

            await _dbContext.SaveChangesAsync();
        }
    }
}
