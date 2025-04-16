using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TodoController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new InvalidOperationException("User ID claim not found");
        return int.Parse(userIdClaim.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
    {
        var userId = GetUserId();
        return await _context.Todos
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetTodo(int id)
    {
        var userId = GetUserId();
        var todo = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (todo == null)
            return NotFound();

        return todo;
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> CreateTodo(Todo todo)
    {
        todo.UserId = GetUserId();
        todo.CreatedAt = DateTime.UtcNow;

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, Todo todo)
    {
        var userId = GetUserId();
        var existingTodo = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (existingTodo == null)
            return NotFound();

        existingTodo.Title = todo.Title;
        existingTodo.Description = todo.Description;
        existingTodo.IsCompleted = todo.IsCompleted;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var userId = GetUserId();
        var todo = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (todo == null)
            return NotFound();

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
} 