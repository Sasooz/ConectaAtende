using ConectaAtende.Application.Services;
using ConectaAtende.Communication.Requests.Contact;
using Microsoft.AspNetCore.Mvc;

namespace ConectaAtende.Api.Controllers;

[ApiController]
[Route("contacts")]
public class ContactsController : ControllerBase
{
    private readonly ContactService _service;

    public ContactsController(ContactService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateContactRequest request)
    {
        var id = await _service.CreateAsync(
            request.Name,
            request.Phone);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var contact = await _service.GetByIdAsync(id);

        if (contact == null)
            return NotFound();

        return Ok(contact);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateContactRequest request)
    {
        await _service.UpdateAsync(
            id,
            request.Name,
            request.Phone);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var contacts =
            await _service.GetPagedAsync(page, pageSize);

        return Ok(contacts);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] string? phone)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            var result =
                await _service.SearchByNameAsync(name);

            return Ok(result);
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            var result =
                await _service.SearchByPhoneAsync(phone);

            return Ok(result);
        }

        return BadRequest("Provide name or phone");
    }


    [HttpPost("undo")]
    public async Task<IActionResult> Undo()
    {
        await _service.UndoAsync();

        return NoContent();
    }

    [HttpGet("recent")]
    public IActionResult GetRecent([FromQuery] int limit = 10)
    {
        var contacts = _service.GetRecentAsync(limit);

        return Ok(contacts);
    }
}