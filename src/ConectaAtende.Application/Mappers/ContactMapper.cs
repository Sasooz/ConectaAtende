using ConectaAtende.Communication.Responses.Contact;
using ConectaAtende.Domain.Entities;

namespace ConectaAtende.Application.Mappers;

public static class ContactMapper
{
    public static ContactResponse ToResponse(Contact contact)
    {
        return new ContactResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            Phone = contact.Phone
        };
    }

    public static IEnumerable<ContactResponse> ToResponseList(
        IEnumerable<Contact> contacts)
    {
        return contacts.Select(ToResponse);
    }
}