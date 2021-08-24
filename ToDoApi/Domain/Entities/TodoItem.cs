using System;
using System.Collections.Generic;

namespace ToDoApi.Domain.Entities
{
    public class TodoItem
    {
        public int? Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime ModifiedOn { get; set; }

        public IList<TodoTag> Tags { get; set; } = new List<TodoTag>();
    }
}
