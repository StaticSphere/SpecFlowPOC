using AutoMapper;
using ToDoApi.Domain.Entities;
using ToDoApi.ViewModels;

namespace ToDoApi.Mapping
{
    public class TodoTagProfile : Profile
    {
        public TodoTagProfile()
        {
            CreateMap<TodoTag, TodoTagViewModel>();
        }
    }
}
