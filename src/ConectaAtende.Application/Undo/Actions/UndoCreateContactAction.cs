using ConectaAtende.Application.Undo;
using ConectaAtende.Domain.Repositories;

namespace ConectaAtende.Application.Undo.Actions;

public class UndoCreateContactAction : IUndoAction
{
    private readonly IContactRepository _repository;
    private readonly Guid _contactId;

    public UndoCreateContactAction(
        IContactRepository repository,
        Guid contactId)
    {
        _repository = repository;
        _contactId = contactId;
    }

    public async Task ExecuteAsync()
    {
        await _repository.DeleteAsync(_contactId);
    }
}