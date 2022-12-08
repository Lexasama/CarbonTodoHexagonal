using CarbonTodo.Api.ApiModels.Todo;
using CarbonTodo.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarbonTodo.Api.Controllers
{
    [ApiController]
    [Route("todos")]
    [Produces("application/json")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;
        private readonly LinkGenerator _linkGenerator;

        public TodoController(ITodoService todoService, LinkGenerator linkGenerator)
        {
            _service = todoService;
            _linkGenerator = linkGenerator;
        }

        [HttpGet(Name = "GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TodoViewModel>>> FindAll()
        {
            var todos = await _service.FindAll();
            return Ok(todos.Select(t => TodoViewModel.From(t, GetUrl(t.Id))));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoViewModel>> FindOne(int id)
        {
            var todo = await _service.FindById(id);

            return Ok(TodoViewModel.From(todo, GetUrl(todo.Id)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateDto dto)
        {
            var todo = await _service.Create(dto.Title);

            string url = GetUrl(todo.Id);

            TodoViewModel result = TodoViewModel.From(todo, url);
            return Created(result.Url, result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(bool? completed)
        {
            if (completed is null)
            {
                await _service.DeleteAll();
                return NoContent();
            }

            await _service.DeleteCompleted();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDto dto)
        {
            var todo = await _service.Update(id, dto.Completed, dto.Title, dto.Order);

            return Ok(TodoViewModel.From(todo, GetUrl(id)));
        }

        [HttpPut("complete-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CompleteAll()
        {
            await _service.CompleteAll();
            return Ok();
        }

        private string GetUrl(int id)
        {
            return _linkGenerator.GetUriByAction(HttpContext, nameof(FindOne), "Todo", new { id })!;
        }
    }
}