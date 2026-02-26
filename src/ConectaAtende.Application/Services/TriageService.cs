using ConectaAtende.Domain.Entities;
using ConectaAtende.Domain.Repositories;
using ConectaAtende.Domain.Policies;

namespace ConectaAtende.Application.Services;

public class TriageService
{
    private readonly ITicketRepository _repository;
    private readonly ITriagePolicy _policy;

    public TriageService(
        ITicketRepository repository,
        ITriagePolicy policy)
    {
        _repository = repository;
        _policy = policy;
    }

    public async Task<Ticket?> GetNextAsync()
    {
        var queued = await _repository.GetQueuedAsync();

        return _policy.GetNext(queued);
    }

    public async Task ApplyTriageAsync(
        Guid ticketId,
        int priority,
        string category)
    {
        var ticket = await _repository.GetByIdAsync(ticketId);

        if (ticket == null)
            throw new Exception("Ticket not found");

        ticket.ApplyTriage(
            (Domain.Enums.TicketPriority)priority,
            category);

        await _repository.UpdateAsync(ticket);
    }
}