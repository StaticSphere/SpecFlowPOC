using AutoMapper;
using ToDoApi.Domain.Entities;
using ToDoApi.ViewModels;

namespace ToDoApi.Mapping
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItem, TodoItemViewModel>();
        }
    }
}
