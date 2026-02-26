using ConectaAtende.Domain.Enums;

namespace ConectaAtende.Domain.Entities;

public class Ticket
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public Guid ContactId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? ClosedAt { get; private set; }

    public TicketPriority Priority { get; private set; }

    public string Category { get; private set; } = string.Empty;

    public TicketStatus Status { get; private set; }

    public Ticket(
        string title,
        string description,
        Guid contactId)
    {
        Id = Guid.NewGuid();

        Title = title;
        Description = description;
        ContactId = contactId;

        CreatedAt = DateTime.UtcNow;

        Status = TicketStatus.Open;
    }

    public void Enqueue()
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException(
                "Only open tickets can be queued");

        Status = TicketStatus.Queued;
    }

    public void StartService()
    {
        if (Status != TicketStatus.Queued)
            throw new InvalidOperationException(
                "Ticket must be queued");

        Status = TicketStatus.InService;
    }
    public void Close()
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException(
                "Ticket already closed");

        Status = TicketStatus.Closed;

        ClosedAt = DateTime.UtcNow;
    }

    public void Update(
        string title,
        string description)
    {
        Title = title;
        Description = description;
    }

    public void ApplyTriage(
        TicketPriority priority,
        string category)
    {
        Priority = priority;
        Category = category;
    }
}