using ConectaAtende.Domain.Entities;

namespace ConectaAtende.Domain.Repositories;

public interface ITicketRepository
{
    Task CreateAsync(Ticket ticket);

    Task<Ticket?> GetByIdAsync(Guid id);

    Task<List<Ticket>> GetAllAsync();

    Task UpdateAsync(Ticket ticket);

    Task DeleteAsync(Guid id);

    Task<IEnumerable<Ticket>> GetQueuedAsync();
}