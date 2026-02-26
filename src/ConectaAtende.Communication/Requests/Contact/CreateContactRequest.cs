namespace ConectaAtende.Communication.Requests.Contact;

public class CreateContactRequest
{
    public string Name { get; set; } = default!;
    public string Phone { get; set; } = default!;
}