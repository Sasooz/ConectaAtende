using System.Text.RegularExpressions;

namespace ConectaAtende.Domain.Entities;

public class Contact
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Phone { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    private Contact() { }

    public Contact(string name, string phone)
    {
        ValidateName(name);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Phone = NormalizePhone(phone);

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string phone)
    {
        ValidateName(name);

        Name = name.Trim();
        Phone = NormalizePhone(phone);

        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");
    }

    private static string NormalizePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required");

        return Regex.Replace(phone, "[^0-9]", "");
    }
    public Contact Clone()
    {
        return new Contact(Name, Phone)
        {
            Id = Id
        };
    }
}