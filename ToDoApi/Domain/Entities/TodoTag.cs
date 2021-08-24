using System;
using System.Collections.Generic;

namespace ToDoApi.Domain.Entities
{
    public class TodoTag
    {
        public int? Id { get; set; }
        public string Title { get; set; } = "";
        public string? Color { get; set; }
        public DateTime ModifiedOn { get; set; }

        public IList<TodoItem> Items { get; set; } = new List<TodoItem>();
    }
}
