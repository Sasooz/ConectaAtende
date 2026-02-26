using ConectaAtende.Domain.Entities;
using ConectaAtende.Domain.Repositories;
using ConectaAtende.Domain.Services;

namespace ConectaAtende.Application.Services;

public class TicketService
{
    private readonly ITicketRepository _repository;

    private readonly ITicketTriagePolicy _triagePolicy;

    public TicketService(
        ITicketRepository repository,
        ITicketTriagePolicy triagePolicy)
    {
        _repository = repository;
        _triagePolicy = triagePolicy;
    }

    public async Task<Guid> CreateAsync(
        string title,
        string description,
        Guid contactId)
    {
        var ticket = new Ticket(
            title,
            description,
            contactId);

        var priority =
            _triagePolicy.DefinePriority(ticket);

        var category =
            _triagePolicy.DefineCategory(ticket);

        ticket.ApplyTriage(priority, category);

        await _repository.CreateAsync(ticket);

        return ticket.Id;
    }

    public async Task EnqueueAsync(Guid id)
    {
        var ticket =
            await _repository.GetByIdAsync(id);

        if (ticket == null)
            throw new Exception("Ticket not found");

        ticket.Enqueue();

        await _repository.UpdateAsync(ticket);
    }

    public async Task<Ticket?> DequeueAsync()
    {
        var all =
            await _repository.GetAllAsync();

        var ticket =
            all.FirstOrDefault(
                x => x.Status ==
                     Domain.Enums.TicketStatus.Queued);

        if (ticket == null)
            return null;

        ticket.StartService();

        await _repository.UpdateAsync(ticket);

        return ticket;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
    public async Task<Ticket?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Ticket>> GetPagedAsync(
        int page,
        int pageSize)
    {
        var all =
            await _repository.GetAllAsync();

        return all
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }

    public async Task CloseAsync(Guid id)
    {
        var ticket =
            await _repository.GetByIdAsync(id);

        if (ticket == null)
            throw new Exception("Ticket not found");

        ticket.Close();

        await _repository.UpdateAsync(ticket);
    }

    public async Task DeleteAsync(Guid id)
    {
        var ticket =
            await _repository.GetByIdAsync(id);

        if (ticket == null)
            throw new Exception("Ticket not found");

        await _repository.DeleteAsync(id);
    }
}