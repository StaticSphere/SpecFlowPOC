using System;
using System.Collections.Generic;

namespace ToDoApi.ViewModels
{
    public class TodoItemViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime ModifiedOn { get; set; }

        public IEnumerable<TodoTagViewModel> Tags { get; set; } = new List<TodoTagViewModel>();
    }
}
