using ConectaAtende.Domain.Entities;
using ConectaAtende.Domain.Repositories;
using Projeto.Infrastructure.Persistence.InMemory;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

namespace Projeto.Infrastructure.Repositories;

public class InMemoryContactRepository : IContactRepository
{
    private readonly ConcurrentDictionary<Guid, Contact> _contacts = new();

    private readonly RecentContactsList _recentContacts = new(20);

    public Task AddAsync(Contact contact)
    {
        if (!_contacts.TryAdd(contact.Id, contact))
            throw new InvalidOperationException("Contact already exists");

        return Task.CompletedTask;
    }

    public Task<Contact?> GetByIdAsync(Guid id)
    {
        if (_contacts.TryGetValue(id, out var contact))
        {
            _recentContacts.Add(contact);

            return Task.FromResult<Contact?>(contact);
        }

        return Task.FromResult<Contact?>(null);
    }

    public Task UpdateAsync(Contact contact)
    {
        if (!_contacts.ContainsKey(contact.Id))
            throw new InvalidOperationException("Contact not found");

        _contacts[contact.Id] = contact;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _contacts.TryRemove(id, out _);

        _recentContacts.Remove(id);

        return Task.CompletedTask;
    }

    public Task<bool> ExistsByPhoneAsync(string normalizedPhone)
    {
        var exists = _contacts.Values
            .Any(c => NormalizePhone(c.Phone) == normalizedPhone);

        return Task.FromResult(exists);
    }

    public Task<IEnumerable<Contact>> GetPagedAsync(int page, int pageSize)
    {
        var result = _contacts.Values
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult<IEnumerable<Contact>>(result);
    }

    public Task<IEnumerable<Contact>> SearchByNameAsync(string name)
    {
        var normalizedSearch = NormalizeText(name);

        var result = _contacts.Values
            .Where(c =>
                NormalizeText(c.Name)
                .Contains(normalizedSearch))
            .OrderBy(c => c.Name)
            .ToList();

        return Task.FromResult<IEnumerable<Contact>>(result);
    }

    public Task<IEnumerable<Contact>> SearchByPhoneAsync(string phone)
    {
        var normalizedPhone = NormalizePhone(phone);

        var result = _contacts.Values
            .Where(c =>
                NormalizePhone(c.Phone)
                .Contains(normalizedPhone))
            .OrderBy(c => c.Name)
            .ToList();

        return Task.FromResult<IEnumerable<Contact>>(result);
    }

    public Task<int> CountAsync()
    {
        return Task.FromResult(_contacts.Count);
    }

    public Task<IEnumerable<Contact>> GetRecentAsync(int limit)
    {
        var result = _recentContacts
            .GetRecent(limit)
            .ToList();

        return Task.FromResult<IEnumerable<Contact>>(result);
    }

    private static string NormalizeText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        text = text
            .ToLowerInvariant()
            .Normalize(NormalizationForm.FormD);

        var builder = new StringBuilder();

        foreach (var c in text)
        {
            var category = Char.GetUnicodeCategory(c);

            if (category != UnicodeCategory.NonSpacingMark)
                builder.Append(c);
        }

        return builder
            .ToString()
            .Normalize(NormalizationForm.FormC);
    }

    private static string NormalizePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return string.Empty;

        return new string(phone
            .Where(char.IsDigit)
            .ToArray());
    }
}