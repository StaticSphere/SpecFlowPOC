using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Domain.Entities;
using ToDoApi.Services;
using ToDoApi.ViewModels;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly IMapper _mapper;
        private readonly string _baseUrl;

        public TodoItemsController(ITodoItemService todoItemService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _todoItemService = todoItemService;
            _mapper = mapper;

            var request = httpContextAccessor.HttpContext?.Request;
            _baseUrl = $"{request?.Scheme}://{request?.Host}";
        }

        [HttpGet]
        public async Task<ActionResult> GetTodoItemsAsync([FromQuery] bool includeCompleted = false, [FromQuery] IEnumerable<string> tags = null!)
        {
            var todoItems = tags.Any()
                ? await _todoItemService.GetTodoItemsByTagAsync(tags, includeCompleted)
                : await _todoItemService.GetTodoItemsAsync(includeCompleted);

            var mappedTodoItems = _mapper.Map<TodoItemViewModel[]>(todoItems);
            
            return Ok(mappedTodoItems);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetTodoItemAsync([FromRoute] int id)
        {
            var todoItem = await _todoItemService.GetTodoItemAsync(id);
            if (todoItem == null)
                return NotFound();

            return Ok(todoItem);
        }

        [HttpPost]
        public async Task<ActionResult> AddTodoItemAsync([FromBody] TodoItem todoItem)
        {
            var id = await _todoItemService.AddTodoItemAsync(todoItem);

            return Created($"{_baseUrl}/todoItems/{id}", id);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTodoItemAsync([FromBody] TodoItem todoItem)
        {
            await _todoItemService.UpdateTodoItemAsync(todoItem);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTodoItemAsync([FromRoute] int id)
        {
            await _todoItemService.DeleteTodoItemAsync(id);

            return NoContent();
        }
    }
}
