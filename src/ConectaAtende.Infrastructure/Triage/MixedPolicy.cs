using ConectaAtende.Domain.Entities;
using ConectaAtende.Domain.Enums;
using ConectaAtende.Domain.Policies;

namespace ConectaAtende.Infrastructure.Triage;

public class MixedPolicy : ITriagePolicy
{
    public Ticket? GetNext(IEnumerable<Ticket> tickets)
    {
        return tickets
            .Where(t => t.Status == TicketStatus.Queued)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.CreatedAt)
            .FirstOrDefault();
    }
}