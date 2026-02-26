using ConectaAtende.Application.Undo;
using ConectaAtende.Domain.Entities;
using ConectaAtende.Domain.Repositories;

namespace ConectaAtende.Application.Undo.Actions;

public class UndoUpdateContactAction : IUndoAction
{
    private readonly IContactRepository _repository;
    private readonly Contact _backup;

    public UndoUpdateContactAction(
        IContactRepository repository,
        Contact backup)
    {
        _repository = repository;
        _backup = backup;
    }

    public async Task ExecuteAsync()
    {
        await _repository.UpdateAsync(_backup);
    }
}