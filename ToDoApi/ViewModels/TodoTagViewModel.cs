namespace ToDoApi.ViewModels
{
    public record TodoTagViewModel
    {
        public int? Id { get; init; }
        public string Title { get; init; } = "";
        public string? Color { get; init; }
    }
}
