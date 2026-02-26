namespace ConectaAtende.Communication.Responses.Contact;

public class ContactResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Phone { get; set; } = default!;
}