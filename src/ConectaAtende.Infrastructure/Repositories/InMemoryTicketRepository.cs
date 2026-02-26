using ConectaAtende.Domain.Entities;
using ConectaAtende.Domain.Enums;
using ConectaAtende.Domain.Repositories;
using System.Collections.Concurrent;

namespace ConectaAtende.Infrastructure.Repositories;

public class InMemoryTicketRepository : ITicketRepository
{
    private readonly ConcurrentDictionary<Guid, Ticket> _tickets = new();

    public Task CreateAsync(Ticket ticket)
    {
        if (!_tickets.TryAdd(ticket.Id, ticket))
            throw new InvalidOperationException("Ticket already exists");

        return Task.CompletedTask;
    }

    public Task<Ticket?> GetByIdAsync(Guid id)
    {
        _tickets.TryGetValue(id, out var ticket);

        return Task.FromResult(ticket);
    }

    public Task<List<Ticket>> GetAllAsync()
    {
        var result = _tickets.Values.ToList();

        return Task.FromResult(result);
    }

    public Task UpdateAsync(Ticket ticket)
    {
        if (!_tickets.ContainsKey(ticket.Id))
            throw new InvalidOperationException("Ticket not found");

        _tickets[ticket.Id] = ticket;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _tickets.TryRemove(id, out _);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Ticket>> GetQueuedAsync()
    {
        var result = _tickets.Values
            .Where(t => t.Status == TicketStatus.Queued);

        return Task.FromResult(result);
    }
}