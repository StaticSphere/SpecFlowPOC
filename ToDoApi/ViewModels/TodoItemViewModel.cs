using System;
using System.Collections.Generic;

namespace ToDoApi.ViewModels
{
    public record TodoItemViewModel
    {
        public int? Id { get; init; }
        public string Title { get; init; } = "";
        public string? Description { get; init; }
        public bool Completed { get; init; }
        public DateTime? DueDate { get; init; }
        public DateTime ModifiedOn { get; init; }

        public IEnumerable<TodoTagViewModel> Tags { get; init; } = new List<TodoTagViewModel>();
    }
}
