using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync(bool includeCompleted = false);
        Task<IEnumerable<TodoItem>> GetTodoItemsByTagAsync(IEnumerable<string> tags, bool includeCompleted = false);
        Task<TodoItem> GetTodoItemAsync(int id);
        Task<int> AddTodoItemAsync(TodoItem item);
        Task<int> UpdateTodoItemAsync(TodoItem item);
        Task DeleteTodoItemAsync(int id);
    }
}
