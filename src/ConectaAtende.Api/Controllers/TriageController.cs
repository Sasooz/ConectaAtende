using ConectaAtende.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConectaAtende.Presentation.Controllers;

[ApiController]
[Route("triage")]
public class TriageController : ControllerBase
{
    private readonly TriageService _service;

    public TriageController(TriageService service)
    {
        _service = service;
    }

    [HttpGet("next")]
    public async Task<IActionResult> GetNext()
    {
        var ticket = await _service.GetNextAsync();

        if (ticket == null)
            return NotFound();

        return Ok(ticket);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> ApplyTriage(
        Guid id,
        [FromBody] ApplyTriageRequest request)
    {
        await _service.ApplyTriageAsync(
            id,
            request.Priority,
            request.Category);

        return Ok();
    }
}

public class ApplyTriageRequest
{
    public int Priority { get; set; }

    public string Category { get; set; } = string.Empty;
}