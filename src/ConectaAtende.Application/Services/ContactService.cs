using ConectaAtende.Domain.Entities;
using ConectaAtende.Application.Undo.Actions;
using ConectaAtende.Application.Mappers;
using ConectaAtende.Application.Undo;
using ConectaAtende.Domain.Repositories;
using ConectaAtende.Communication.Responses.Contact;


namespace ConectaAtende.Application.Services;

public class ContactService
{
    private readonly IContactRepository _repository;
    private readonly UndoService _undoService;

    public ContactService(
        IContactRepository repository,
        UndoService undoService)
    {
        _repository = repository;
        _undoService = undoService;
    }

    public async Task<Guid> CreateAsync(string name, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        var normalizedPhone = NormalizePhone(phone);

        if (string.IsNullOrWhiteSpace(normalizedPhone))
            throw new ArgumentException("Phone is required");

        var exists = await _repository.ExistsByPhoneAsync(normalizedPhone);

        if (exists)
            throw new InvalidOperationException("Phone already exists");

        var contact = new Contact(name, normalizedPhone);

        await _repository.AddAsync(contact);

        _undoService.Register(
            new UndoCreateContactAction(_repository, contact.Id)
        );

        return contact.Id;
    }

    public async Task<ContactResponse?> GetByIdAsync(Guid id)
    {
        var contact = await _repository.GetByIdAsync(id);

        if (contact == null)
            return null;

        return ContactMapper.ToResponse(contact);
    }

    public async Task UpdateAsync(Guid id, string name, string phone)
    {
        var contact = await _repository.GetByIdAsync(id);

        if (contact == null)
            throw new InvalidOperationException("Contact not found");

        var normalizedPhone = NormalizePhone(phone);

        var backup = contact.Clone();

        contact.Update(name, normalizedPhone);

        await _repository.UpdateAsync(contact);

        _undoService.Register(
            new UndoUpdateContactAction(
                _repository,
                backup
            )
        );
    }

    public async Task DeleteAsync(Guid id)
    {
        var contact = await _repository.GetByIdAsync(id);

        if (contact == null)
            throw new InvalidOperationException("Contact not found");

        await _repository.DeleteAsync(id);

        _undoService.Register(
            new UndoDeleteContactAction(
                _repository,
                contact
            )
        );
    }

    public async Task<IEnumerable<ContactResponse>> GetPagedAsync(
    int page,
    int pageSize)
    {
        var contacts =
            await _repository.GetPagedAsync(page, pageSize);

        return ContactMapper.ToResponseList(contacts);
    }

    public async Task<IEnumerable<ContactResponse>> SearchByNameAsync(
    string name)
    {
        var contacts =
            await _repository.SearchByNameAsync(name);

        return ContactMapper.ToResponseList(contacts);
    }

    public async Task<IEnumerable<ContactResponse>> SearchByPhoneAsync(
    string phone)
    {
        var contacts =
            await _repository.SearchByPhoneAsync(phone);

        return ContactMapper.ToResponseList(contacts);
    }

    public async Task<IEnumerable<Contact>> GetRecentAsync(int limit)
    {
        return await _repository.GetRecentAsync(limit);
    }

    public async Task UndoAsync()
    {
        var success = await _undoService.UndoAsync();

        if (!success)
            throw new InvalidOperationException("Nothing to undo");
    }

    private static string NormalizePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return string.Empty;

        return new string(
            phone
            .Where(char.IsDigit)
            .ToArray());
    }
}