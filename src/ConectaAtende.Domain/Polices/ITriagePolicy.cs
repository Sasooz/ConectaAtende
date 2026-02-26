using ConectaAtende.Domain.Entities;

namespace ConectaAtende.Domain.Policies;

public interface ITriagePolicy
{
    Ticket? GetNext(IEnumerable<Ticket> tickets);
}