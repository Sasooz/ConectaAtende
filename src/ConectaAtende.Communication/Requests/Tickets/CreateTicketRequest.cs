namespace ConectaAtende.Communication.Tickets.Requests;

public class CreateTicketRequest
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid ContactId { get; set; }
}