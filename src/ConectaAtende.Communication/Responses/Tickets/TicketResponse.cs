namespace ConectaAtende.Communication.Tickets.Responses;

public class TicketResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid ContactId { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string Priority { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;
}