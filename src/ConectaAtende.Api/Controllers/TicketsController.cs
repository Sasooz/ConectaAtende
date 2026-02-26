using ConectaAtende.Application.Services;
using ConectaAtende.Communication.Tickets.Requests;
using ConectaAtende.Communication.Tickets.Responses;
using ConectaAtende.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ConectaAtende.Api.Controllers;

[ApiController]
[Route("tickets")]
public class TicketsController : ControllerBase
{
    private readonly TicketService _ticketService;
    private readonly TriageService _triageService;

    public TicketsController(
        TicketService ticketService,
        TriageService triageService)
    {
        _ticketService = ticketService;
        _triageService = triageService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTicketRequest request)
    {
        var id = await _ticketService.CreateAsync(
            request.Title,
            request.Description,
            request.ContactId);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);

        if (ticket == null)
            return NotFound();

        return Ok(MapToResponse(ticket));
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged(
        int page = 1,
        int pageSize = 10)
    {
        var tickets =
            await _ticketService.GetPagedAsync(page, pageSize);

        return Ok(tickets
            .Select(MapToResponse)
            .ToList());
    }

    [HttpPut("{id}/close")]
    public async Task<IActionResult> Close(Guid id)
    {
        try
        {
            await _ticketService.CloseAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _ticketService.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet("next")]
    public async Task<IActionResult> GetNext()
    {
        var ticket = await _triageService.GetNextAsync();

        if (ticket == null)
            return NotFound("No tickets in queue");

        return Ok(MapToResponse(ticket));
    }

    [HttpPost("enqueue/{id}")]
    public async Task<IActionResult> Enqueue(Guid id)
    {
        await _ticketService.EnqueueAsync(id);

        return NoContent();
    }

    [HttpPost("dequeue")]
    public async Task<IActionResult> Dequeue()
    {
        var ticket =
            await _ticketService.DequeueAsync();

        if (ticket == null)
            return NotFound("No queued tickets");

        return Ok(MapToResponse(ticket));
    }

    private static TicketResponse MapToResponse(Ticket ticket)
    {
        return new TicketResponse
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            ContactId = ticket.ContactId,
            Status = ticket.Status.ToString(),
            CreatedAt = ticket.CreatedAt,
            Priority = ticket.Priority.ToString(),
            Category = ticket.Category
        };
    }
}