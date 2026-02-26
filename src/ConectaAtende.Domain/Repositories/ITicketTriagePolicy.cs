using ConectaAtende.Domain.Entities;
using ConectaAtende.Domain.Enums;

namespace ConectaAtende.Domain.Services;

public interface ITicketTriagePolicy
{
    TicketPriority DefinePriority(Ticket ticket);

    string DefineCategory(Ticket ticket);
}