using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Domain
{
    public interface ITodoDbContext
    {
        DbSet<TodoItem> TodoItems { get; set; }
        DbSet<TodoTag> TodoTags { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
